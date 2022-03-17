using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyProperties : MonoBehaviour
{
    [Header("Rigid Body Parameters")]

    [Tooltip("(Scaled) Radius of Sphere = Half of Scale Component")]
    public float volumetricMeanRadius; // Celestial Size/Radius

    [Tooltip("Mass of Body in Earth Masses.")]
    public float mass; // Mass of Celestial

    [Tooltip("Length of Solar Day in realtime seconds.")]
    public float dayPeriod; // Length of SOLAR Day in realtime seconds (remember 1t = 0.1 Earth Days)

    [Tooltip("The angle between the body's equator and the body's orbital plane, with north defined by the right - hand rule. (J2000).")]
    public float obliquityToOrbit;

    [Header("Orbital Parameters")]
    [Tooltip("Closest distance between body and host.")]
    public float periapsis; // Closest orbital distance to host
    [Tooltip("Furthest distance between body and host.")]
    public float apoapsis; // Furthest orbital distance to host
    [Tooltip("")]
    public float orbitalPeriod; // Time taken in realtime seconds to orbit around the host

    [Header("Shape (Read Only)")]
    public float semiMajor;
    public float eccentricity;

    [Tooltip("Vector components of the Specific Angular Momentum, a perpendicular vector to the orbital plane.")]
    public Vector3 angularMomentum; // Calculated upon InitialVelocity calculation in SimulationScript.cs

    [Header("Orientation")]
    [Tooltip("The inclination of the orbit to the ecliptic, in degrees.")]
    public float inclination;

    public float rightAscension;
    public float argumentOfPeriapsis;

    private void Start()
    {
        //gameObject.GetComponent<Transform>().position = new Vector3(periapsis, 0f, 0f); // Placing this here prevents reseting position when periapsis is changed in editor. Changing Orbital Parameters mid-sim should not change the trajectory in realtime
        
    }

    void PropertyUpdate()
    {
        gameObject.GetComponent<Rigidbody>().mass = mass;
        gameObject.GetComponent<Transform>().localScale = new Vector3(volumetricMeanRadius, volumetricMeanRadius, volumetricMeanRadius) * 2f; // Radius of Sphere is 0.5 Scale/Diameter
        

        semiMajor = 0.5f * (periapsis + apoapsis);

        eccentricity = -1f * (periapsis - apoapsis)/(periapsis+apoapsis);


        orbitalPeriod = Mathf.Sqrt( 4* Mathf.PI* Mathf.PI* (semiMajor * gameObject.transform.parent.lossyScale.x ) * (semiMajor * gameObject.transform.parent.lossyScale.x) * (semiMajor * gameObject.transform.parent.lossyScale.x) / (1f) );

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
        Vector3 posVector = Quaternion.Euler(0, 0, inclination) * new Vector3(periapsis, 0, 0);
        gameObject.transform.localPosition = posVector;
        //gameObject.transform.localPosition = new Vector3(periapsis, 0, 0);

        Vector3 angularVelocity = (2 * Mathf.PI / dayPeriod) * Vector3.up;
        gameObject.GetComponent<Rigidbody>().angularVelocity =  Quaternion.AngleAxis(obliquityToOrbit, Vector3.right) * angularVelocity;


    }

    // Assign the above parameters to the gameObject
    void OnValidate()
    {
        PropertyUpdate();
    }

}
