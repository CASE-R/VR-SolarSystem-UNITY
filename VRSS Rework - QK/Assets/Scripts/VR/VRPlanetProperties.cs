using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRPlanetProperties : MonoBehaviour
{
    public GameObject properties;
    VRCamSwitch VRCamSwitch;
    SimulationScript simulation;

    public InputField massInput;
    public InputField radiusInput;
    public InputField velocityInput;

    // Start is called before the first frame update
    void Start()
    {
        VRCamSwitch = gameObject.GetComponent<VRCamSwitch>();
        simulation = gameObject.GetComponent<SimulationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (VRCamSwitch.celNumber >= -1)
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
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().mass = 0f;
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().velocity = Vector3.zero;
        simulation.celestials[VRCamSwitch.celNumber].transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        VRCamSwitch.celNumber = -1;
    }

    public void ChangeMass()
    {
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().mass = float.Parse(massInput.text);
    }

    public void ChangeVelocity()
    {
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().velocity = simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().velocity.normalized * float.Parse(velocityInput.text);
    }

    public void ChangeRadius()
    {
        float newRadius = float.Parse(radiusInput.text);
        simulation.celestials[VRCamSwitch.celNumber].transform.GetChild(0).gameObject.transform.localScale = new Vector3 (newRadius, newRadius, newRadius);
    }
}
