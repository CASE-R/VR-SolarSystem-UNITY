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
        //Whenever not in freeCam (in other words: when focused on a celestial) we want to show the properties menu
        if (VRCamSwitch.celNumber >= -1)
        {
            properties.SetActive(true);
        }
        // Otherwise hide the properties menu
        else
        {
            properties.SetActive(false);
        }
    }

    /// <summary>
    /// Method called whenever "Remove" button is pressed when focused on a celestial. Method will attempt to remove all relevant physics and visual components possible.
    /// </summary>
    public void RemovePlanet()
    {
        // Setting mass and velocity to 0 should stop any ongoing motion and effect on other celestials
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().mass = 0f;
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().velocity = Vector3.zero;
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation; // Added from new additions in PlanetProperties.cs which restricts any motion of the RigidBody of the celestial

        // Disabling renderers and colliders should hide the focused celestial, effectively removing them without deleting them from the hierarchy disrupting the hierarchy structure.
        simulation.celestials[VRCamSwitch.celNumber].transform.GetChild(0).gameObject.GetComponent<Renderer>().enabled = false;
        simulation.celestials[VRCamSwitch.celNumber].transform.GetChild(0).gameObject.GetComponent<SphereCollider>().enabled = false;
        simulation.celestials[VRCamSwitch.celNumber].GetComponentInChildren<TrailRenderer>().enabled = false;
        // For celestials like the sun, additional components must be disabled like light and particle effects
        simulation.celestials[VRCamSwitch.celNumber].GetComponentInChildren<Light>().enabled = false;
        simulation.celestials[VRCamSwitch.celNumber].GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
        simulation.celestials[VRCamSwitch.celNumber].GetComponentInChildren<ParticleSystemForceField>().gameObject.SetActive(false);
        VRCamSwitch.celNumber = 0;
    }

    public void RemoveAllCelestials()
    {
        for (int i = 0; i < simulation.celestials.Length; i++)
        {
            Destroy(simulation.celestials[i]);
        }
    }

    // The ChangeProperty functions are called whenever their respective input fields are "submitted", this could be by pressing enter or clicking away

    /// <summary>
    /// Method to change the RigidBody mass of a celestial.
    /// </summary>
    public void ChangeMass()
    {
        // Mass can be directly changed by accessing the rigidbody of the focused celestial
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().mass = float.Parse(massInput.text);
    }

    /// <summary>
    /// Method to change the RigidBody velocity's magnitude of a celestial.
    /// </summary>
    public void ChangeVelocity()
    {
        // The velocity gets changed to the input field value and then gets multiplied by its last velocity direction unit vector
        simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().velocity = simulation.celestials[VRCamSwitch.celNumber].GetComponent<Rigidbody>().velocity.normalized * float.Parse(velocityInput.text);
    }

    /// <summary>
    /// Method to change the renderer scale of a celestial. This does not change the scale of the entire celestial, only the object which makes it visible i.e. the "Sphere" child.
    /// </summary>
    public void ChangeRadius()
    {
        // To avoid changing the children (the moons) of a celestial, we change the local scale of the sphere we set all x,y and z values of the local scale to the radius chosen by the user
        float newRadius = float.Parse(radiusInput.text);
        simulation.celestials[VRCamSwitch.celNumber].transform.GetChild(0).gameObject.transform.localScale = new Vector3 (newRadius, newRadius, newRadius);
    }
}
