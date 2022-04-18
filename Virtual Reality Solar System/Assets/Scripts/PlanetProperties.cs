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

    public Dropdown celDropdown;
    // Start is called before the first frame update
    void Start()
    {
        cameraFocus = gameObject.GetComponent<CameraFocus>();
        simulation = gameObject.GetComponent<SimulationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //whenever not in freeCam (in other words: when focused on a celestial):
        //we want to show the properties menu
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
        //removePlanet button grabs the sphere GameObject of the celestial that we're currently focused on
        GameObject SphereChildObject = simulation.celestials[cameraFocus.celNumber].transform.GetChild(0).gameObject;
        
        //we then set the mass of that sphere to to 0 and disable its renderer to make it invisible
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().mass = 0;
        SphereChildObject.GetComponent<Renderer>().enabled = false;
        for (int i = 0; i < SphereChildObject.transform.childCount; i++)
        {
            //if the object has more than 1 renderer (eg. Saturn and its rings) disable all renderers
            SphereChildObject.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
        }

        

        //set the celestial's velocity to 0 and restricts its movement + rotation so it doesn't accelerate for any reason
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().velocity = Vector3.zero;
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        //we now want to go to freeCam so that the planet is no longer followed
        cameraFocus.celNumber = -1;
    }

    //the changeProperty functions are called whenever their respective input fields are "submitted"
    //this could be by pressing enter or clicking away
    public void ChangeMass()
    {
        //mass can be directly changed by accessing the rigidbody of the focused celestial
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().mass = float.Parse(massInput.text);
    }

    public void ChangeVelocity()
    {
        //the velocity gets changed to the input field value
        //and then gets multiplied by its last velocity direction unit vector
        simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().velocity 
            = simulation.celestials[cameraFocus.celNumber].GetComponent<Rigidbody>().velocity.normalized
            * float.Parse(velocityInput.text);
    }

    public void ChangeRadius()
    {
        //to avoid changing the children (the moons) of a celestial, we change the local scale of the sphere
        //we set all x,y and z values of the local scale to the radius chosen by the user
        float newRadius = float.Parse(radiusInput.text);
        simulation.celestials[cameraFocus.celNumber].transform.GetChild(0).gameObject.transform.localScale
            = new Vector3 (newRadius, newRadius, newRadius);
    }
}
