using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyProperties : MonoBehaviour
{
    SimulationScript simScript;
    
    [Tooltip("The Empty GameObject which the Solar System simulation runs from")]
    public GameObject systemObj; // This is searched for later on by name

    [Tooltip("The GameObject that this object is attached to")]
    public GameObject parentObj;

    [Header("Rigid Body Parameters")]

    [Tooltip("('Scaled') Radius of Sphere = Half of Scale Component")]
    public float volumetricMeanRadius; // Celestial Size/Radius

    [Tooltip("Mass of Body in Earth Masses.")]
    public float mass; // Mass of Celestial

    [Tooltip("Length of Solar Day in realtime seconds.")]
    public float dayPeriod; // Length of SOLAR Day in realtime seconds (remember 1t = 0.1 Earth Days)

    [Tooltip("The angle between the body's equator and the body's orbital plane, with north defined by the right - hand rule. (J2000).")]
    public float obliquityToOrbit;

    [Header("Orbital Parameters (Input)")]
    [Tooltip("Closest distance between body and host.")]
    public float periapsis; // Closest orbital distance to host
    [Tooltip("Furthest distance between body and host.")]
    public float apoapsis; // Furthest orbital distance to host
    [Tooltip("Calculated time taken in realtime seconds to orbit around the host")]
    public float orbitalPeriod; // Time taken in realtime seconds to orbit around the host

    [Header("Shape (Read Only)")]
    public float periapsisLocal;
    public float apoapsisLocal;
    public float semiMajor;
    public float eccentricity;

    [Tooltip("Vector components of the Specific Angular Momentum, a perpendicular vector to the orbital plane.")]
    public Vector3 angularMomentum; // Calculated upon InitialVelocity calculation in SimulationScript.cs

    [Header("Orientation (Input)")]
    [Tooltip("The inclination of the orbit to the ecliptic, in degrees.")]
    public float inclination;
    public float rightAscension;
    public float argumentOfPeriapsis;

    [Header("Value Checks (Read Only)")]
    public float dotProductOfVelAndRadial;
    public float dotProductOfAngMomAndVel;

    public Vector3 initDirection;

    private void Start()
    {
        //gameObject.GetComponent<Transform>().position = new Vector3(periapsis, 0f, 0f); // Placing this here prevents reseting position when periapsis is changed in editor. Changing Orbital Parameters mid-sim should not change the trajectory in realtime
        

    }

    void PropertyUpdate()
    {
        if (gameObject.transform.parent != null)
        {
            parentObj = gameObject.transform.parent.gameObject;
        }
        else
        {
            parentObj = null;
        }

        systemObj = GameObject.Find("System");
        simScript = systemObj.GetComponent<SimulationScript>();


        // Below contains the main properties to be updated
        if (parentObj.CompareTag("Celestial") || gameObject.name == "Sun" || gameObject.name.Contains("Grabbable Celestial"))
        {
            gameObject.GetComponent<Rigidbody>().mass = mass;
            gameObject.GetComponent<Transform>().localScale = new Vector3(volumetricMeanRadius, volumetricMeanRadius, volumetricMeanRadius) * 2f; // Radius of Sphere is 0.5 Scale/Diameter, and we are treating these as perfect spheres

            periapsisLocal = periapsis * parentObj.transform.lossyScale.x;
            apoapsisLocal = apoapsis * parentObj.transform.lossyScale.x;


            semiMajor = 0.5f * (periapsisLocal + apoapsisLocal); // Same as saying 2a = r_P + r_A as explained in report/notes etc.

            eccentricity = (apoapsis - periapsis) / (periapsis + apoapsis); // Changed 09/04/22 to match derivations

            angularMomentum = Quaternion.Euler(0, rightAscension, inclination) * Vector3.up; // Rotates specific angular momentum vector from the initial 'up' (Y) position (which comes from first setting the positions of bodies in the XZ plane)

            Vector3 posVectorResult = Quaternion.Euler(0, 0, inclination) * Quaternion.Euler(0, rightAscension, 0) * Quaternion.AngleAxis(argumentOfPeriapsis, angularMomentum) * new Vector3(periapsis, 0, 0); // transforms/rotates periapsis position vector to not be aligned in the XZ plane with other celestials
            initDirection = Quaternion.Euler(0, 0, inclination) * Quaternion.Euler(0, rightAscension, 0) * Quaternion.AngleAxis(argumentOfPeriapsis, angularMomentum) * Vector3.forward; // applies same transform/rotation as applied to periapsis rotation vector where velocity was originally in 'forward' direction (used in SimulationScript.cs)

            if (parentObj.CompareTag("Celestial"))
            {
                orbitalPeriod = Mathf.Sqrt(4 * Mathf.Pow(Mathf.PI, 2) * Mathf.Pow((semiMajor), 3f) / (simScript.gravitationalConstant * (mass + parentObj.GetComponent<Rigidbody>().mass)));
            }
            else 
            { 
                orbitalPeriod = 0;
            }

            Vector3 radDist = parentObj.transform.position - gameObject.transform.position;


            gameObject.transform.localPosition = posVectorResult;

            dotProductOfAngMomAndVel = Vector3.Dot(angularMomentum, initDirection);
            dotProductOfVelAndRadial = Vector3.Dot(initDirection, radDist);

            Vector3 angularVelocity = (2 * Mathf.PI / dayPeriod) * Vector3.up;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Quaternion.AngleAxis(obliquityToOrbit, Vector3.right) * angularVelocity;
        }

    }

    // Assign the above parameters to the gameObject
    void OnValidate()
    {
        PropertyUpdate();
    }

}
