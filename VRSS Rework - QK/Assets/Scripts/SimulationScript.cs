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
    [SerializeField] public float initialFixedTimeStep = 0.02f;

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
    
    //public float G = 0.08892541f;

    public GameObject[] celestials; // [Sun, Merc, Ven, Earth, Moon, Mars, Jup, Sat, Uran, Nep, Plut] are the main celestials
    
    public GameObject[] particleSystems;

    // Start is called before the first frame update
    public void Start()
    {
        InitialVelocity(); // Executes Velocity method to give Celestials initial velocities to induce orbits
       
        // Caps/Syncs Simulation FPS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;

        startTime = System.DateTime.Now;
        currentTime = startTime;
        timer.GetComponent<Text>().text = startTime.ToString();
    }
    public void OnValidate()
    {
        gravitationalConstant = 4f * Mathf.Pow(Mathf.PI / (365.26f), 2f) * Mathf.Pow(lengthUnit, 3f) * Mathf.Pow(massUnit * celestials[0].GetComponent<Rigidbody>().mass, -1f) * Mathf.Pow(timeUnit, -2f);

        celestials = GameObject.FindGameObjectsWithTag("Celestial"); // Collates all GameObjects w/ "Celestial" tag into an array
        particleSystems = GameObject.FindGameObjectsWithTag("ParticleSystem");
        
        currentScene = SceneManager.GetActiveScene();

    }

    // Update is called once per rendered frame
    void Update()
    {
        Time.timeScale = initialTimeScale * timeUnitMultiplier; // Scales time to run faster/slower at seconds/second, days/second or weeks/second

        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 0) // Check for seconds/second setting
        {
            //initialFixedTimeStep = timeUnitMultiplier; // increases physics update rate to match new timescale
            //Time.fixedDeltaTime = timeUnitMultiplier; // Sets physUpdates to equal to realtimeSecond multiplier to simulate 1 Earth second/realtime second
        }
        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value != 0) // This statement will reset updates to the default setting
        {
            //initialFixedTimeStep = 0.02f;
            //Time.fixedDeltaTime = initialFixedTimeStep;
        }

        timeStart += Time.deltaTime; // Used for an in-editor runtime counter

        updateInGameTimer();
    }

    void FixedUpdate()
    {
        Gravity();
        physTimeStart += Time.fixedDeltaTime; // USed for an in-editor runtime counter for physics clock

        if ((initialTimeScale >= 2 && gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 1) || gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 2) // SetActive=false for asteroid belt in faster timescales
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

    // Original Velocity Script for Elliptical orbits edited for parent-child systems. THIS SYSTEM ALLOWS FOR A GOOD MOON ORBIT
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
                    float mass1 = parentObj.GetComponent<Rigidbody>().mass;
                    float mass2 = child.GetComponent<Rigidbody>().mass;
                    //float scaleMultiplier = parentObj.transform.lossyScale.x; // Using lossyScale gives 'global scale of the object chosen'. This replaces need to manually find product of scales of ancestors.

                    float semiMajor = child.GetComponent<BodyProperties>().semiMajor; // * (scaleMultiplier); // Transforms semiMajor input value from BodyProperties.cs to the correct Global value

                    float distance = Vector3.Distance(parentObj.transform.position, child.transform.position); //Radial Distance between 2-body. Doesn't need rescaling due to function of Vector3.Distance
                    //Vector3 radialDistance = parentObj.transform.position - child.transform.position;

                    // Using original visViva
                    Vector3 parentObjVelocity = parentObj.GetComponent<Rigidbody>().velocity;

                    Vector3 velocityDirection = child.GetComponent<BodyProperties>().initDirection; // Defines temporary vector direction for velocity at periapsis

                    //Debug.Log("Vel Direction: " + velocityDirection + " || " + "Rad Direction: " + radialDistance +" || " + "dotProd(radDist,angMoment): " + dotProduct + " || " + "angMoment: " + child.GetComponent<BodyProperties>().angularMomentum);

                    child.GetComponent<Rigidbody>().velocity += parentObjVelocity + velocityDirection * Mathf.Sqrt((gravitationalConstant * (mass1 + mass2)) * ((2 / distance) - (1 / semiMajor))); // Adds required orbit velocity to host's velocity so it moves w/ correct relative velocity.

                    Debug.Log("Distance is " + distance + " || " + "SemiMajor is " + semiMajor + " || " + "Velocity of " + child + " is " + child.GetComponent<Rigidbody>().velocity.magnitude + " || " + "Mass of Parent = " + mass1 + " Mass of Child = " + mass2);
                }

            }

        }
    }

    // Calculates Gravitational Force of Attraction between 2 bodies
    void Gravity()
    {
        foreach (GameObject celestial1 in celestials)
        {
            foreach (GameObject celestial2 in celestials)
            {
                if (!celestial1.Equals(celestial2))
                {
                    float mass1 = celestial1.GetComponent<Rigidbody>().mass;
                    float mass2 = celestial2.GetComponent<Rigidbody>().mass;

                    float distance = Vector3.Distance(celestial1.transform.position, celestial2.transform.position);

                    celestial1.GetComponent<Rigidbody>().AddForce((celestial2.transform.position - celestial1.transform.position).normalized * (gravitationalConstant * mass1 * mass2 / (distance * distance)));
                }

            }
        }
    }

    public void updateInGameTimer()
    {
        timeToAdd = TimeSpan.FromSeconds(Time.deltaTime*24*60*60);
        currentTime = currentTime.Add(timeToAdd);
        timer.text = currentTime.ToString();
    }

    public void restartSimulation()
    {
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(currentScene.name);
    }

}


