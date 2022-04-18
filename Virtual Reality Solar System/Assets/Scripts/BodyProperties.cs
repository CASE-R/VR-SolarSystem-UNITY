using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyProperties : MonoBehaviour
{
    SimulationScript simScript;
    
    [Tooltip("The Empty GameObject which the Solar System simulation runs from.")]
    public GameObject systemObj; // This is searched for later on by name

    [Tooltip("The GameObject that this object is attached to.")]
    public GameObject parentObj;

    [Header("Rigid Body Parameters")]

    [Tooltip("('Scaled') Radius of Sphere = Half of Scale Component.")]
    public float volumetricMeanRadius; // Celestial Size/Radius independent of parent's global scale, found by parentObj.transform.lossyScale

    [Tooltip("Mass of Body in Earth Masses.")]
    public float mass; // Mass of Celestial

    [Tooltip("Length of Solar Day relative to Earth Solar Day per realtime second. Negative values imply opposite/anti-clockwise rotation (i.e. 175 means 175 seconds to do one revolution).")]
    public float dayPeriod; // Length of SOLAR Day in realtime seconds (remember 1t = 1 Earth Days)

    [Tooltip("The angle between the body's equator and the body's orbital plane, with north defined by the right - hand rule. (J2000).")]
    public float obliquityToOrbit;

    [Header("Shape (Input)")]
    [Tooltip("Closest distance between body and host.")]
    public float periapsis; // Closest orbital distance to host
    [Tooltip("Furthest distance between body and host.")]
    public float apoapsis; // Furthest orbital distance to host

    [Header("Orientation (Input)")]
    [Tooltip("Angle of current orbital plane to ecliptic plane, in degrees.")]
    public float inclination;
    [Tooltip("Angle in the ecliptic plane where the satellite crosses the plane in an ascending direction.")]
    public float rightAscension;
    [Tooltip("Angle in the orbital plane between the Right Ascension and the periapsis measured in the direction of motion.")]
    public float argumentOfPeriapsis;

    [Header("Shape (Read Only)")]
    [Tooltip("Value in Global Space (independent of Parent's scale).")]
    public float periapsisGlobal;
    [Tooltip("Value in Global Space (independent of Parent's scale).")]
    public float apoapsisGlobal;
    [Tooltip("Semimajor axis of orbital path.")]
    public float semiMajor;
    public float eccentricity;
    [Tooltip("Calculated time taken in realtime seconds to orbit around the host.")]
    public float orbitalPeriod; // Time taken in realtime seconds to orbit around the host

    [Tooltip("Vector components of the Specific Angular Momentum, a perpendicular vector to the orbital plane.")]
    public Vector3 angularMomentum; // Calculated upon InitialVelocity calculation in SimulationScript.cs

    

    [Header("Value Checks (Read Only)")]
    public float dotProductOfVelAndRadial;
    public float dotProductOfAngMomAndVel;

    public Vector3 initDirection;

    private void Start()
    {
        PropertyUpdate();
    }

    /// <summary>
    /// Updates all properties in the Editor that are dependent on each other when OnValidate() executes. This keeps all positions, directions and times updated when necessary.
    /// </summary>
    void PropertyUpdate()
    {
        // Check for a parent object, which usually will be a "Celestial" object
        if (gameObject.transform.parent != null)
        {
            parentObj = gameObject.transform.parent.gameObject;
        }
        else
        {
            parentObj = null;
        }

        systemObj = GameObject.Find("System"); // Finds GameObject with this name, this is the object in hierarchy w/ all simulation settings
        simScript = systemObj.GetComponent<SimulationScript>();


        // Below contains the main properties to be updated
        if (parentObj.CompareTag("Celestial") || gameObject.name == "Sun" || gameObject.name.Contains("Grabbable Celestial")) // True if body this is attached to requires these properties to be updated. Doing such a condition prevents null errors without lots of conditionals. Only want updated properties for these conditions
        {
            gameObject.GetComponent<Rigidbody>().mass = mass;
            gameObject.GetComponent<Transform>().localScale = new Vector3(volumetricMeanRadius, volumetricMeanRadius, volumetricMeanRadius) * 2f; // Radius of Sphere is 0.5 Scale/Diameter, and we are treating these as perfect spheres

            periapsisGlobal = periapsis * parentObj.transform.lossyScale.x;
            apoapsisGlobal = apoapsis * parentObj.transform.lossyScale.x;


            semiMajor = 0.5f * (periapsisGlobal + apoapsisGlobal); // Same as saying 2a = r_P + r_A as explained in report/notes etc.

            eccentricity = (apoapsis - periapsis) / (periapsis + apoapsis); // Changed 09/04/22 to match derivations

            angularMomentum = Quaternion.Euler(0, rightAscension, inclination) * Vector3.up; // Rotates specific angular momentum vector from the initial 'up' (Y) position (which comes from first setting the positions of bodies in the XZ plane)

            Vector3 posVectorResult = Quaternion.Euler(0, 0, inclination) * Quaternion.Euler(0, rightAscension, 0) * Quaternion.AngleAxis(argumentOfPeriapsis, angularMomentum) * new Vector3(periapsis, 0, 0); // transforms/rotates periapsis position vector to not be aligned in the XZ plane with other celestials
            initDirection = Quaternion.Euler(0, 0, inclination) * Quaternion.Euler(0, rightAscension, 0) * Quaternion.AngleAxis(argumentOfPeriapsis, angularMomentum) * Vector3.forward; // applies same transform/rotation as applied to periapsis rotation vector where velocity was originally in 'forward' direction (used in SimulationScript.cs)

            if (parentObj.CompareTag("Celestial"))
            {
                orbitalPeriod = Mathf.Sqrt(4 * Mathf.Pow(Mathf.PI, 2) * Mathf.Pow((semiMajor), 3f) / (simScript.gravitationalConstant * (mass + parentObj.GetComponent<Rigidbody>().mass))); // Using K3L
            }
            else 
            { 
                orbitalPeriod = 0;
            }

            Vector3 radDist = parentObj.transform.position - gameObject.transform.position;


            gameObject.transform.localPosition = posVectorResult;

            dotProductOfAngMomAndVel = Vector3.Dot(angularMomentum, initDirection);
            dotProductOfVelAndRadial = Vector3.Dot(initDirection, radDist); // Should be 0 as h = v x r, all three are orthogonal

            Vector3 angularVelocity = (2 * Mathf.PI / dayPeriod) * Vector3.up; // Causes planet to rotate about its 'up' axis.
            gameObject.GetComponent<Rigidbody>().angularVelocity = Quaternion.AngleAxis(obliquityToOrbit, Vector3.right) * angularVelocity; // Rotates north pole axis
        }

    }

    // Assign the above parameters to the gameObject
    void OnValidate()
    {
        PropertyUpdate();
    }

}
