using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A script to initialise most parameters for a Celestial, this includes things like orbital parameters which are read by other scripts
/// </summary>
public class CelestialProperty : MonoBehaviour
{
    [Header("Initial Rigid Body Parameters")]

    [Tooltip("Initial Spherical Radius of Object")]
    public Vector3 volumetricMeanRadius;
    [Tooltip("Initial Mass of Object")]
    public float Mass = 1f;


    [Header("Orbital Parameters")]

    [Tooltip("Largest Radius of Elliptical Orbit (Editing this will affect orbits)")]
    public float semiMajorAxis;
        //[38.70974211f, 72.33385473f, 99.99799463f, 0.256955307f, 152.3790425f, 520.3806201f, 957.2594553f, 1916.498215f, 3018.05706f];

    //[Tooltip("Smallest Radius of Elliptical Orbit")]
    //public float semiMinorAxis = 1f;
    //[Tooltip("Eccentricity of Orbit")]
    //public float eccentricity;

    [Tooltip("Orbit Velocity")]
    public Vector3 orbitalVelocity;

    //[Tooltip("Expected Orbital Period in realtime seconds")]
    //public float orbitalPeriod;

    private void InitialiseParameters()
    {
        orbitalVelocity = new Vector3(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.z);
        volumetricMeanRadius = new Vector3(GetComponent<Transform>().localScale.x, GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
        Mass = GetComponent<Rigidbody>().mass;
    }




    // Start is called before the first frame update
    void Start()
    {
        InitialiseParameters();
        //eccentricity = Mathf.Sqrt(1 - Mathf.Pow(semiMajorAxis,2) / Mathf.Pow(semiMinorAxis, 2) );
    }
   
    
    // Update is called once per frame
    void FixedUpdate()
    {
        
        // orbitalPeriod = Mathf.Sqrt( Mathf.Pow(2 * Mathf.PI, 2) / (GetComponent<EllipticalTest>().G * (GetComponent<Rigidbody>().mass + Sun.GetComponent<Rigidbody>().mass)) ) *Mathf.Pow(semiMajorAxis, 3);
    }
}
