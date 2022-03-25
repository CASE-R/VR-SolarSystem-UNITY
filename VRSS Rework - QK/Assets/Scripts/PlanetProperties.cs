using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetProperties : MonoBehaviour
{
    public GameObject properties;
    CameraFocus cameraFocus;
    SimulationScript simulation;

    public InputField massInput;
    public InputField radiusInput;
    public InputField velocityInput;

    // Start is called before the first frame update
    void Start()
    {
        cameraFocus = gameObject.GetComponent<CameraFocus>();
        simulation = gameObject.GetComponent<SimulationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraFocus.celNumber > -1)
        {
            properties.SetActive(true);
        }

        else
        {
            properties.SetActive(false);
        }
    }

    public void RemovePlanet()
    {
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().mass = 0;
        simulation.celestials[cameraFocus.celNumber].transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        cameraFocus.celNumber = -1;
    }

    public void ChangeMass()
    {
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().mass = float.Parse(massInput.text);
    }

    public void ChangeVelocity()
    {
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().velocity = simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().velocity.normalized * float.Parse(velocityInput.text);
    }
}
