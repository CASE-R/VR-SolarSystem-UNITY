using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbiterProperties : MonoBehaviour
{
    [Header("Rigid Body Parameters")]
    public float volumetricMeanRadius = 1f; // Celestial Size/Radius
    public float mass = 1f; // Mass of Celestial
    public float dayPeriod = 1f; // Length of SOLAR Day in realtime seconds (remember 1t = 0.1 Earth Days)
    public Vector3 axisOfRotation = Vector3.up;

    [Header("Orbital Parameters")]
    public float periDistance = 1f; // Closest orbital distance to host
    public float apDistance = 1f; // Furthest orbital distance to host
    public float orbitalPeriod = 1f; // Time taken in realtime seconds to orbit around the host
    public float semiMajor;

    private void Start()
    {
        //gameObject.GetComponent<Transform>().position = gameObject.GetComponentInParent<Transform>().position + new Vector3(periDistance, 0f, 0f); // Placing this here prevents reseting position when periDistance is changed in editor. Changing Orbital Parameters mid-sim should not change the trajectory in realtime
    }

    void PropertyUpdate()
    {
        gameObject.GetComponent<Rigidbody>().mass = mass;
        gameObject.GetComponent<Transform>().localScale = new Vector3(volumetricMeanRadius, volumetricMeanRadius, volumetricMeanRadius);
        

        semiMajor = 0.5f * (periDistance + apDistance);

        orbitalPeriod = Mathf.Sqrt( 4* Mathf.PI* Mathf.PI* semiMajor * semiMajor * semiMajor / (1f) );

        Vector3 angularVelocity = (2 * Mathf.PI / dayPeriod) * Vector3.one;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.Cross(axisOfRotation, angularVelocity);

    }

    // Assign the above parameters to the gameObject
    void OnValidate()
    {
        PropertyUpdate();
    }
}
