using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SimulationScript : MonoBehaviour
{
    [Header("Simulation Settings")]
    [SerializeField] public int frameRate = 60;
    [Range(0f, 100f)]
    [SerializeField] public float initialTimeScale = 1f;
    [Range(0.00000001f, 1f)]
    [SerializeField] public float initialFixedTimeStep = 0.02f; // Set's physics clock to update ~50 times a second

    public float timeUnitMultiplier = 1; // Depending on which time unit is chosen, the time will be adjusted. This sets default to Earth Days/second

    //Gives visual timers (NOT TO BE CHANGED IN EDITOR)
    [SerializeField] private float timeStart;
    [SerializeField] private float physTimeStart;
    private DateTime startTime;
    public Text timer;
    private DateTime currentTime;
    TimeSpan timeToAdd;
    public Scene currentScene;

    [Header("Simulation Parameters")]
    ///<summary>
    /// G is recalculated to be in the new unity dimensions
    /// </summary>
    public float gravitationalConstant;
    public float timeUnit = 1f;
    public float massUnit = 1f;
    public float lengthUnit = 100f;

    public GameObject[] celestials; // [Sun, Merc, Ven, Earth, Moon, Mars, Jup, Sat, Uran, Nep, Plut] are the main celestials
    
    public GameObject[] particleSystems; // Used to easily disable particles for faster timescales

    // Start is called before the first frame update
    public void Start()
    {
        // Caps/Syncs Simulation FPS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;

        startTime = System.DateTime.Now;
        currentTime = startTime;
        timer.GetComponent<Text>().text = startTime.ToString();

        currentScene = SceneManager.GetActiveScene(); // As this script is used for multiple scenes during development, this is used later to reload the current active scene
        gravitationalConstant = 4f * Mathf.Pow(Mathf.PI / (365.26f), 2f) * Mathf.Pow(lengthUnit, 3f) * Mathf.Pow(massUnit * celestials[0].GetComponent<Rigidbody>().mass, -1f) * Mathf.Pow(timeUnit, -2f); // Calculates the G constant based off K3L using the custom Unity scale/units

        //celestials = GameObject.FindGameObjectsWithTag("Celestial"); // Collates all GameObjects w/ "Celestial" tag into an array
        //particleSystems = GameObject.FindGameObjectsWithTag("ParticleSystem"); // Collates any GameObjects w/ "ParticleSystem" tag like above. This is used specifically for particle systems or anything that should be treated as one

        InitialVelocity(); // Executes Velocity method to give Celestials initial velocities to start orbits from initial conditions
    }

    // Update is called once per rendered frame
    void Update()
    {
        Time.timeScale = initialTimeScale * timeUnitMultiplier; // Scales time to run faster/slower at seconds/second, days/second or weeks/second

        timeStart += Time.deltaTime; // Used for an in-editor runtime counter

        updateInGameTimer();
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        Gravity(); // Calls method to calculate gravitational forces between celestials
        physTimeStart += Time.fixedDeltaTime; // Used for an in-editor runtime counter for physics clock

        if ((initialTimeScale >= 2 && gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 1) || gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 2) // SetActive=false for asteroid belt (and other particle systems) in faster timescales
        {
            foreach (GameObject partSys in particleSystems)
            {
                partSys.SetActive(false);
            }
            
        }
        else
        {
            foreach (GameObject partSys in particleSystems)
            {
                partSys.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Calculates velocity from initially set conditions in Editor and Vis Viva equation to be applied to celestial via RigidBody component.
    /// </summary>
    /// Modelled off the example in this video: https://www.youtube.com/watch?v=kUXskc76ud8
    /// Heavily conditioned to be specific to the hierarchy put in place for elliptical orbits
    private void InitialVelocity()
    {
        foreach (GameObject parentObj in celestials) // Begin with the Sun, which we know is a 'parent'/'host'
        {
            int noOfChildren = parentObj.transform.childCount;

            GameObject parentParent = parentObj.transform.parent.gameObject; // Attempt to find parent of parent object (allowing for 3-tier system)
            bool celTagParent = parentParent.CompareTag("Celestial"); // Check to see if this ancestor is a Celestial object to apply physics to

            for (int i = 0; i < noOfChildren; i++)
            {
                GameObject child = parentObj.transform.GetChild(i).gameObject; // Chooses child GameObject of index i
                bool celTag = child.CompareTag("Celestial"); // Check to see if the child is a Celestial object to apply physics to

                if (celTag)
                {
                    Debug.Log("Child of " + parentObj + " is " + child);

                    // Below is the velocity script
                    float mass1 = parentObj.GetComponent<Rigidbody>().mass; // Mass of the host body
                    float mass2 = child.GetComponent<Rigidbody>().mass; // Mass of the satellite

                    float semiMajor = child.GetComponent<BodyProperties>().semiMajor; // Transforms semiMajor input value from BodyProperties.cs to the correct Global value

                    float distance = Vector3.Distance(parentObj.transform.position, child.transform.position); //Radial Distance between 2-body. Doesn't need rescaling due to function of Vector3.Distance

                    // Using original visViva
                    Vector3 parentObjVelocity = parentObj.GetComponent<Rigidbody>().velocity;

                    Vector3 velocityDirection = child.GetComponent<BodyProperties>().initDirection; // Defines temporary vector direction for velocity at periapsis from BodyProperties.cs

                    child.GetComponent<Rigidbody>().velocity += parentObjVelocity + velocityDirection * Mathf.Sqrt((gravitationalConstant * (mass1 + mass2)) * ((2 / distance) - (1 / semiMajor))); // Gives celestial calculated velocity plus current velocity of host celestial so satellite moves correctly relative to host

                    Debug.Log("Distance is " + distance + " || " + "SemiMajor is " + semiMajor + " || " + "Velocity of " + child + " is " + child.GetComponent<Rigidbody>().velocity.magnitude + " || " + "Mass of Parent = " + mass1 + " Mass of Child = " + mass2);
                }

            }

        }
    }

    /// <summary>
    /// Calculates Gravitational Force between 1 celestial and all other celestials in the array "celestials".
    /// </summary>
    /// Based off gravity method in this video: https://www.youtube.com/watch?v=kUXskc76ud8
    void Gravity()
    {
        foreach (GameObject celestial1 in celestials) // Chooses a celestial from celestial array
        {
            foreach (GameObject celestial2 in celestials) // Chooses another celestial to treat as a 'satellite' of celestial1 i.e. Sun-Planet, Planet-Moon etc.
            {
                if (!celestial1.Equals(celestial2)) // Prevents calc error when calculating for force applied to itself (r=0 --> F = undefined)
                {
                    float mass1 = celestial1.GetComponent<Rigidbody>().mass; // Mass of BodyA
                    float mass2 = celestial2.GetComponent<Rigidbody>().mass; // Mass of BodyB

                    float distance = Vector3.Distance(celestial1.transform.position, celestial2.transform.position); // Finds 'r' value, this is distance

                    celestial1.GetComponent<Rigidbody>().AddForce((celestial2.transform.position - celestial1.transform.position).normalized * (gravitationalConstant * mass1 * mass2 / (distance * distance))); // Applies gravitational force to BodyA from BodyB. This sums up in the loop to create a resultant gravitational force from different celestials
                }

            }
        }
    }


    /// <summary>
    /// Method that updates ingame date/clock based on timeframe and timescale.
    /// </summary>
    public void updateInGameTimer()
    {
        timeToAdd = TimeSpan.FromSeconds(Time.deltaTime*24*60*60);
        currentTime = currentTime.Add(timeToAdd);
        timer.text = currentTime.ToString();
    }

    /// <summary>
    /// Restarts loaded scene to initial conditions when "Restart" button is played.
    /// </summary>
    public void restartSimulation()
    {
        SceneManager.LoadScene(currentScene.name);
    }

}


