using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyProperties : MonoBehaviour
{
    [Header("Rigid Body Parameters")]
    public float volumetricMeanRadius; // Celestial Size/Radius
    public float mass; // Mass of Celestial
    public float dayPeriod; // Length of SOLAR Day in realtime seconds (remember 1t = 0.1 Earth Days)
    public float obliquityToOrbit; // Angle from perpendicular axis of orbit

    [Header("Orbital Parameters")]
    public float periDistance; // Closest orbital distance to host
    public float apDistance; // Furthest orbital distance to host
    public float orbitalPeriod; // Time taken in realtime seconds to orbit around the host
    [Header("Shape (Read Only)")]
    public float semiMajor;
    public float eccentricity;
    [Header("Orientation")]
    public float inclination;
    public float rightAscension;
    public float argumentOfPeriapsis;

    private void Start()
    {
        //gameObject.GetComponent<Transform>().position = new Vector3(periDistance, 0f, 0f); // Placing this here prevents reseting position when periDistance is changed in editor. Changing Orbital Parameters mid-sim should not change the trajectory in realtime
        
    }

    void PropertyUpdate()
    {
        gameObject.GetComponent<Rigidbody>().mass = mass;
        gameObject.GetComponent<Transform>().localScale = new Vector3(volumetricMeanRadius, volumetricMeanRadius, volumetricMeanRadius) * 2f; // Radius of Sphere is 0.5 Scale/Diameter
        

        semiMajor = 0.5f * (periDistance + apDistance);

        eccentricity = -1f * (periDistance - apDistance)/(periDistance+apDistance);

        orbitalPeriod = Mathf.Sqrt( 4* Mathf.PI* Mathf.PI* semiMajor * semiMajor * semiMajor / (1f) );

        /// <summary>
        /// From Unity Cartesian to Spherical
        /// x = rho * sin(phi) * cos(theta)
        /// z = rho * sin(phi) * sin(theta)
        /// y = rho * cos(phi)
        /// rho = distance btwn point and origin
        /// theta = angle in XZ plane
        /// phi = angle from positive y to rho line = 90 - inclination
        /// </summary>

        //// Next 2 lines will give the orbital inclination of the planet, rotating along the z-axis once setting initial position
        Vector3 posVector = Quaternion.Euler(0, 0, inclination) * new Vector3(periDistance, 0, 0);
        gameObject.transform.localPosition = posVector;
        //gameObject.transform.localPosition = new Vector3(periDistance, 0, 0);

        Vector3 angularVelocity = (2 * Mathf.PI / dayPeriod) * Vector3.up;
        gameObject.GetComponent<Rigidbody>().angularVelocity =  Quaternion.AngleAxis(obliquityToOrbit, Vector3.right) * angularVelocity;


    }

    // Assign the above parameters to the gameObject
    void OnValidate()
    {
        PropertyUpdate();
    }

}
