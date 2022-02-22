using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystem : MonoBehaviour
{
    // This script is to assign orbital parameters to an massive gameObject that is expected to orbit. Rather than letting Newton's Law of Grav. create orbits naturally we are initialising solar system orbits from collected data and applying equations of motion.

    /// <summary>
    /// Takes scale of gameObject as 'radius'
    /// </summary>
    public float volumetricRadius;

    /// <summary>
    /// Takes mass of gameObject from RigidBody script
    /// </summary>
    public float objectMass;

    /// <summary>
    /// Sets the wanted semi-major axis length, which equals half the sum of the peri- and aph- distance
    /// </summary>
    public float semiMajor;

    /// <summary>
    /// Initial position of orbit, taken as the 'peri'-distance to the host which the body orbits around
    /// </summary>
    public Vector3 periDistance;

    /// <summary>
    /// (Maximum) Initial velocity required to allow an orbit based on periDistance and objectMass
    /// </summary>
    public float maxInitVelocity;

    public GameObject[] celestials; // celestial[0] is always the host

    private void InitialiseParameters()
    {
        float G = GetComponent<EllipticalTest2>().G;

        for (int COi = 0; COi < 1; COi++) // Want only Sun-Celestial pairing, hence we pick index 0 from celestials[] and break once length exceeds 1
        {
            for (int COj = 1; COj < celestials.Length; COj++) // Coupling Celestial-semiMajor
            {
                if (COi != COj)
                {
                    // Set variables for COi
                    float objectMass1 = celestials[COi].GetComponent<Rigidbody>().mass;
                    //float volumetricRadius1 = celestials[COi].GetComponent<Transform>().localScale.magnitude;
                    Vector3 periDistance1 = celestials[COi].GetComponent<Transform>().position;

                    // Set variables for COj
                    float objectMass2 = celestials[COj].GetComponent<Rigidbody>().mass;
                    //float volumetricRadius2 = celestials[COj].GetComponent<Transform>().localScale.magnitude;
                    Vector3 periDistance2 = celestials[COj].GetComponent<Transform>().position;

                    // Set distance and direction btwn COi + COj
                    float r = Vector3.Distance(periDistance1, periDistance2);
                    celestials[COj].transform.LookAt(celestials[COi].transform);

                    celestials[COj].GetComponent<Rigidbody>().velocity += celestials[COj].transform.forward * Mathf.Sqrt(G * (objectMass1 + objectMass2) * ((2 / r) - 1 / semiMajor));
                }
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        InitialiseParameters();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
