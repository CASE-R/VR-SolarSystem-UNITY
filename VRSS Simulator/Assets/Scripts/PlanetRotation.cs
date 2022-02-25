using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField]
    float massOfSphere = 0f; // Total mass of the sphere that is rotating (i.e. celestial mass)
    [SerializeField]
    float radiusOfSphere = 0f; // Radius of the rotating sphere (i.e. celestial scale)
    [SerializeField]
    float lengthOfDay = 0f; // Based on the 'Solar Earth Day', provided in planetaryDataSheet.xlsx
    [SerializeField]
    Vector3 axisOfRotation = Vector3.up; // Should align with parent rotation
    [SerializeField]
    Vector3 angularVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //massOfSphere = gameObject.GetComponent<Rigidbody>().mass;
        //radiusOfSphere = gameObject.GetComponent<Transform>().localScale.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    private void OnValidate()
    {
        UpdateRotation();
    }
    private void UpdateRotation()
    {
        massOfSphere = gameObject.GetComponent<Rigidbody>().mass;
        radiusOfSphere = gameObject.GetComponent<Transform>().localScale.magnitude;
        axisOfRotation = gameObject.GetComponent<Transform>().rotation.eulerAngles;

        angularVelocity = (2 * Mathf.PI / lengthOfDay) * axisOfRotation;
        gameObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;

    }
}
