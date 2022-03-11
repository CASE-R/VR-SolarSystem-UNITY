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
    [Range(0.002f, 1f)]
    [SerializeField] public float initialFixedTimeStep = 0.02f;

    public float timeUnitMultiplier = 1;        //depending on which time unit is chosen, the time will be adjusted

    //Gives visual timers (NOT TO BE CHANGED IN EDITOR)
    [SerializeField] private float timeStart;
    [SerializeField] private float physTimeStart;

    [Header("Simulation Parameters")]
    public float G = 0.08892541f;
    public GameObject[] celestials; // [Sun, M, V, E, Moo, Mars, J, S, U, Pluto, N] are the main celestials

    public float massCOi;
    public float massCOj;
    public float semiMajor;

    // Start is called before the first frame update
    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        InitialVelocity();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
        
    }


    // Update is called once per rendered frame
    void Update()
    {
        Time.timeScale = initialTimeScale * timeUnitMultiplier;
        Time.fixedDeltaTime = initialFixedTimeStep;

        timeStart += Time.deltaTime;
    }
    void FixedUpdate()
    {
        Gravity();
        physTimeStart += Time.fixedDeltaTime;
    }


    // Sets initial orbital velocities of celestials
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
    // Original Velocity Script for Elliptical orbits and parent-child systems
    private void InitialVelocity()
    {
        foreach (GameObject parentObj in celestials)
        {
            // Initialises size of subCelestial array
            int noOfChildren = parentObj.transform.childCount; // Gets children count
            //GameObject[] subCelestials = new GameObject[noOfChildren]; // Initialises array of children to loop through parentObj-Child

            // Checks for parent-child pairing and assigns array index to subCelestial[i]
            GameObject parentParent = parentObj.transform.parent.gameObject;
            Debug.Log("Parent of " + parentObj + " is " + parentParent);
            bool celTagParent = parentParent.CompareTag("Celestial");
            for (int i = 0; i < noOfChildren; i++)
            {
                GameObject child = parentObj.transform.GetChild(i).gameObject;
                bool celTag = child.CompareTag("Celestial");
                if (celTag)
                {
                    Debug.Log("Child of " + parentObj + " is " + child + " of index " + i);

                    // Below is the velocity script
                    float mass1 = parentObj.GetComponent<PlanetProperties>().mass;
                    float mass2 = child.GetComponent<PlanetProperties>().mass;
                    float parentScale = parentObj.GetComponent<PlanetProperties>().volumetricMeanRadius *2f;
                    
                    if (celTagParent && celTag)
                    {
                        float parentParentScale = parentParent.GetComponent<PlanetProperties>().volumetricMeanRadius * 2f;
                        Debug.Log("Scaled are " + parentScale + " and " + parentParentScale);

                        semiMajor = child.GetComponent<PlanetProperties>().semiMajor * ((parentScale) * (parentParentScale));
                        Debug.Log("Conditions are " + celTag + " and " + celTagParent);
                    }
                    if (celTag && !celTagParent)
                    {
                        // Rescales semiMajor value put into PlanetProperties.cs fields
                        semiMajor = child.GetComponent<PlanetProperties>().semiMajor * (parentScale);
                        //Debug.Log("Scale >= 1");
                        Debug.Log("Conditions are " + celTag + " and " + celTagParent);
                    }

                    float distance = Vector3.Distance(parentObj.transform.position, child.transform.position); //Radial Distance between 2-body. Doesn't need rescaling due to function of Vector3.Distance

                    // Using original visViva
                    Vector3 parentObjVelocity = parentObj.GetComponent<Rigidbody>().velocity;
                    child.GetComponent<Rigidbody>().velocity += parentObjVelocity + Vector3.forward * Mathf.Sqrt((G * (mass1 + mass2)) * ((2 / distance) - (1 / semiMajor)));

                    //Applies Vis Viva Orbital Velocity Equation, this is wrt. whatever "COi" is in the anti-clockwise direction

                    Debug.Log("Distance is " + distance + " || " + "SemiMajor is " + semiMajor + " || " + "Velocity of " + child + " is " + child.GetComponent<Rigidbody>().velocity.magnitude);


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


