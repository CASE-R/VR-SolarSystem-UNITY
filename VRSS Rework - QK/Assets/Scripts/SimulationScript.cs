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

    public float timeUnitMultiplier = 1;        //depending on which time unit is chosen, the time will be adjusted

    //Gives visual timers (NOT TO BE CHANGED IN EDITOR)
    [SerializeField] private float timeStart;
    [SerializeField] private float physTimeStart;

    [Header("Simulation Parameters")]
    public float G = 0.08892541f;
    public GameObject[] celestials; // [Sun, Mercury, V, E, Moon, Mars, J, S, U, Pluto, N] are the main celestials

    // Start is called before the first frame update
    public void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial"); // Lists all gameObjects w/ "Celestial" tag into an array

        InitialVelocity(); // Executes Velocity method to give Celestials initial velocities to induce orbits
       
        // Caps/Syncs Simulation FPS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
    }

    /*
public void FindAncestry(GameObject root)
{
    GameObject host = celestials[0]; // Host body is the Sun, scale of "System" gameObject = 1 so can be ignored. Replaced "foreach (host in celestials)" for this to avoid repeat statements
    int noOfChildren = host.transform.childCount; // Finds length of child indexer
    for (int i = 1; i < noOfChildren; i++) // Making list of children starting from index 1
    {
        GameObject root = host; // PLacing this within the for loop rather than before allows to print if statement for parent check

        if (root.transform.GetChild(i).CompareTag("Celestial")) // Checks child is a "Celestial" gameObject
        {
            GameObject child = root.transform.GetChild(i).gameObject; // Chooses child gameObject

            //Debug.Log("Child of " + root + " is " + child);
            /*
            int noOfGrandChildren = child.transform.childCount;
            for (int j = 1; j < noOfGrandChildren; j++)
            {
                parent = child;

                if (parent.transform.GetChild(j).CompareTag("Celestial"))
                {
                    GameObject newChild = child.transform.GetChild(j).gameObject;
                    Debug.Log("Child of " + parent + " is " + newChild);

                    int noOfGrandGrandChildren = newChild.transform.childCount;
                    for (int k = 1; k < noOfGrandGrandChildren; k++)
                    {
                        parent = newChild;

                        if (parent.transform.GetChild(k).CompareTag("Celestial"))
                        {
                            GameObject newGrandChild = newChild.transform.GetChild(j).gameObject;
                            Debug.Log("Child of " + newChild + " is " + newGrandChild); //
                        }
                    }
                }
            }
            // Now we check how many parents the child has
            //while (parent.transform.parent) // Using while will check for all parents before proceeding, 'if' only checks once
            //{
            //    Debug.Log("Higher parent found for " + child + ": " + parent);
            //    parent = parent.transform.parent.gameObject;
            //}

    List<GameObject> list = new List<GameObject>();
        list.Insert(0, root);
        if (root.transform.childCount > 0)
        {
            for (int n = 1; n < root.transform.childCount; n++)
            {
                if (root.transform.GetChild(n).CompareTag("Celestial"))
                {
                    GameObject Child = root.transform.GetChild(n).gameObject;
                    list.Add(Child);
                    if (n == root.transform.childCount - 1) //
                    {
                        Debug.Log(string.Join(", ", list));
                        
                        for (int i = 1; i < list.Count; i++)
                        {
                            // Below is the velocity script
                            float mass1 = list[0].GetComponent<PlanetProperties>().mass;
                            float mass2 = list[i].GetComponent<PlanetProperties>().mass;
                            float parentScale = list[0].GetComponent<PlanetProperties>().volumetricMeanRadius;

                            semiMajor = list[i].GetComponent<PlanetProperties>().semiMajor * (parentScale);
                            float distance = Vector3.Distance(list[0].transform.position, list[i].transform.position); //Radial Distance between 2-body. Doesn't need rescaling due to function of Vector3.Distance

                            // Using original visViva
                            Vector3 parentObjVelocity = list[0].GetComponent<Rigidbody>().velocity;

                            list[i].GetComponent<Rigidbody>().velocity += parentObjVelocity + Vector3.forward * Mathf.Sqrt((G * (mass1 + mass2)) * ((2 / distance) - (1 / semiMajor))); // Adds required orbit velocity to host's velocity so it moves w/ correct relative velocity.

                            Debug.Log("Distance is " + distance + " || " + "SemiMajor is " + semiMajor + " || " + "Velocity of " + list[i] + " is " + list[i].GetComponent<Rigidbody>().velocity.magnitude);
                        }

                    }
                }
            }
        }
    }
    */

    // Update is called once per rendered frame
    void Update()
    {
        Time.timeScale = initialTimeScale * timeUnitMultiplier; // Scales time to run faster/slower at seconds/second, days/second or weeks/second

        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 0) // Check for seconds/second setting
        {
            initialFixedTimeStep = timeUnitMultiplier; // increases physics update rate to match new timescale
            Time.fixedDeltaTime = timeUnitMultiplier; // Sets physUpdates to equal to realtimeSecond multiplier to simulate 1 Earth second/realtime second
        }
        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value != 0) // This statement will reset updates to the default setting
        {
            initialFixedTimeStep = 0.02f;
            Time.fixedDeltaTime = initialFixedTimeStep;
        }

        timeStart += Time.deltaTime; // Used for an in-editor runtime counter
    }

    void FixedUpdate()
    {
        Gravity();
        physTimeStart += Time.fixedDeltaTime; // USed for an in-editor runtime counter for physics clock
    }

    // Sets initial orbital velocities of celestials OLD SYSTEM W/ BAD MOON ORBIT
    /*
    void InitialVelocity()
    {
        for (int COi = 0; COi < 1; COi++) // Want only Sun-Celestial pairing, hence we pick index 0 from celestials[] and break once length exceeds 1
        {
            for (int COj = 1; COj < celestials.Length; COj++) // Coupling Host-Orbiter
            {
                if (COi != COj)
                {
                    massCOi = celestials[COi].GetComponent<PlanetProperties>().mass;
                    massCOj = celestials[COj].GetComponent<PlanetProperties>().mass;
                    Debug.Log("COiMass = " + massCOi + " COjMass = " + massCOj);

                    float semiMajor = celestials[COj].GetComponent<PlanetProperties>().semiMajor;
                            //float semiMajor = (perihelion[COj - 1] + aphelion[COj - 1]) / 2; //Can be proven that 2a = r_p + r_A



                    float distance = Vector3.Distance(celestials[COi].transform.position, celestials[COj].transform.position); //Radial Distance between 2-body

                    celestials[COi].transform.LookAt(celestials[COj].transform);
                    Debug.Log("Distance is " + distance);

                    // Using original visViva
                    celestials[COj].GetComponent<Rigidbody>().velocity += Vector3.forward * Mathf.Sqrt((G * (massCOi + massCOj)) * ((2 / distance) - (1 / (semiMajor)))); //Applies Vis Viva Orbital Velocity Equation, this is wrt. whatever "COi" is in the anti-clockwise direction

                    Debug.Log("Velocity of " + COj + " is " + celestials[COj].GetComponent<Rigidbody>().velocity);

                }
            }
        }
    }
    */

    // Original Velocity Script for Elliptical orbits edited for parent-child systems. THIS SYSTEM ALLOWS FOR A GOOD MOON ORBIT
    private void InitialVelocity()
    {
        foreach (GameObject parentObj in celestials)
        {
            int noOfChildren = parentObj.transform.childCount; // Gets children count
            //GameObject[] subCelestials = new GameObject[noOfChildren]; // Initialises array of children to loop through parentObj-Child

            // Checks for parent-child pairing and assigns array index to subCelestial[i]
            GameObject parentParent = parentObj.transform.parent.gameObject; // Attempt to find parent of parent object (allowing for 3-tier system)
            //Debug.Log("Parent of " + parentObj + " is " + parentParent); // Debug statement to state parent relations during runtime
            bool celTagParent = parentParent.CompareTag("Celestial"); // Check to see if this ancestor is a Celestial object to apply physics to

            for (int i = 0; i < noOfChildren; i++)
            {
                GameObject child = parentObj.transform.GetChild(i).gameObject; // Chooses child gameObject of index i
                bool celTag = child.CompareTag("Celestial"); // Check to see if the child is a Celestial object to apply physics to

                if (celTag)
                {
                    Debug.Log("Child of " + parentObj + " is " + child + " of index " + i);

                    // Below is the velocity script
                    float mass1 = parentObj.GetComponent<Rigidbody>().mass;
                    float mass2 = child.GetComponent<Rigidbody>().mass;
                    float parentScale = parentObj.GetComponent<PlanetProperties>().volumetricMeanRadius * 2f;
                    float scaleMultiplier = parentObj.transform.lossyScale.x; // Using lossyScale gives 'global scale of the object chosen', however is read only

                    if (celTagParent && celTag)
                    {
                        float parentParentScale = parentParent.GetComponent<PlanetProperties>().volumetricMeanRadius * 2f;
                        Debug.Log("Scales are " + parentScale + " and " + parentParentScale + ". Whereas lossyscale gives: " + scaleMultiplier);

                        //semiMajor = child.GetComponent<PlanetProperties>().semiMajor * ((parentScale) * (parentParentScale)); // Correctly scales semiMajor value from PlanetProperties.cs to a value in global space for a child of a 3 tier system i.e. Sun-Earth-Moon
                        //Debug.Log("Conditions are " + celTag + " and " + celTagParent);
                    }
                    if (celTag && !celTagParent)
                    {
                        // Rescales semiMajor value put into PlanetProperties.cs fields
                        // Correctly scales semiMajor value from PlanetProperties.cs to a value in global space for a child of a 2 tier system i.e. Sun-Earth
                        //Debug.Log("Scale >= 1");
                        //Debug.Log("Conditions are " + celTag + " and " + celTagParent);
                        Debug.Log("Scales are " + parentScale + ". Whereas lossyscale gives: " + scaleMultiplier);
                    }

                    float semiMajor = child.GetComponent<PlanetProperties>().semiMajor * (scaleMultiplier);

                    float distance = Vector3.Distance(parentObj.transform.position, child.transform.position); //Radial Distance between 2-body. Doesn't need rescaling due to function of Vector3.Distance

                    // Using original visViva
                    Vector3 parentObjVelocity = parentObj.GetComponent<Rigidbody>().velocity;
                    child.GetComponent<Rigidbody>().velocity += parentObjVelocity + Vector3.forward * Mathf.Sqrt((G * (mass1 + mass2)) * ((2 / distance) - (1 / semiMajor))); // Adds required orbit velocity to host's velocity so it moves w/ correct relative velocity.

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

                    celestial1.GetComponent<Rigidbody>().AddForce((celestial2.transform.position - celestial1.transform.position).normalized * (G * mass1 * mass2 / (distance * distance)));
                }

            }
        }
    }

    public void restartSimulation()
    {
        SceneManager.LoadScene(0);
    }

}


