using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystem : MonoBehaviour
{
    [Header("Sub-System Parameters")]
    [SerializeField]
    public float G;

    public GameObject parentObj; // Host of subsystem
    public GameObject[] subCelestials; // Array of moons/satellites

    public float parentMass; // mass of Parent Object
    public float orbiterMass; // mass of Child Object


    private void OnValidate()
    {
        G = gameObject.GetComponentInParent<SimulationScript>().gravitationalConstant;
        parentObj = gameObject.GetComponentInParent<BodyProperties>().gameObject;

        // Initialises size of subCelestial array
        int noOfchildren = gameObject.transform.childCount;
        subCelestials = new GameObject[noOfchildren];

        // Checks for parent-child pairing and assigns array index to subCelestial[i]
        for (int i = 0; i < noOfchildren; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            //Debug.Log("Child of " + gameObject + " is " + child + " of index " + i);

            subCelestials[i] = child;
        }

        InitialiseVelocity();
    }

    void FixedUpdate()
    {
        Gravity();
    }

    // Sets initial orbital velocities of subCelestials
    void InitialiseVelocity()
    {
        for (int COj = 0; COj < subCelestials.Length; COj++) // Coupling parent-Orbiter
        {
            parentMass = parentObj.GetComponent<BodyProperties>().mass;
            orbiterMass = subCelestials[COj].GetComponent<OrbiterProperties>().mass;
            Debug.Log("Parent Mass = " + parentMass + " Orbiter Mass = " + orbiterMass);

            float semiMajor = subCelestials[COj].GetComponent<OrbiterProperties>().semiMajor;

            float distance = Vector3.Distance(parentObj.transform.position, subCelestials[COj].transform.position); //Radial Distance between 2-body

            //subCelestials[COj].transform.LookAt(parentObj.transform);
            Debug.Log("Distance is " + distance);

            // Using original visViva
            subCelestials[COj].GetComponent<Rigidbody>().velocity += (Vector3.forward * Mathf.Sqrt((G * (parentMass + orbiterMass)) * ((2 / distance) - (1 / (semiMajor))))) + parentObj.GetComponent<Rigidbody>().velocity;
            //Applies Vis Viva Orbital Velocity Equation, this is wrt. whatever "COi" is in the anti-clockwise direction

            //Debug.Log("Velocity of " + COj + " is " + subCelestials[COj].GetComponent<Rigidbody>().velocity);
            Debug.Log("Parent Velocity = " + parentObj.GetComponent<Rigidbody>().velocity + " Child Velocity " + subCelestials[COj] + " = " + subCelestials[COj].GetComponent<Rigidbody>().velocity);

        }
    }

    // Calculates Gravitational Force of Attraction between 2 bodies
    void Gravity()
    {
        foreach (GameObject orbiter in subCelestials)
        {
            float parentMass = parentObj.GetComponent<BodyProperties>().mass;
            float orbiterMass = orbiter.GetComponent<OrbiterProperties>().mass;

            float distance = Vector3.Distance(parentObj.transform.position, orbiter.transform.position);

            parentObj.GetComponent<Rigidbody>().AddForce((orbiter.transform.position - parentObj.transform.position).normalized * (G * parentMass * orbiterMass / (distance * distance)));
        }
    }


}


