using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class VRCamSwitch : MonoBehaviour
{
    public GameObject focusCamera;
    public GameObject HMDCamera;

    public GameObject currentCamera;
    public int celNumber;

    public Vector3 objectPosition;
    public Vector3 objectScale;
    public Vector3 offset;

    public Dropdown celestialMenu;
    SimulationScript simulation;
    VRPlanetProperties VRplanetProperties;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera");

        simulation = gameObject.GetComponent<SimulationScript>();
        VRplanetProperties = gameObject.GetComponent<VRPlanetProperties>();

        // Initialises VR player view
        celNumber = -1;
        currentCamera = HMDCamera;
        HMDCamera.SetActive(true);
        focusCamera.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        // Switches between celestial bodies using Ctrl + < or > keys
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
        {
            if (Input.GetKeyDown(KeyCode.Comma) && celNumber > -1) // uses '<'
            {
                celNumber--;
                celestialMenu.value = celNumber;
            }
            if (Input.GetKeyDown(KeyCode.Period) && celNumber < gameObject.GetComponent<SimulationScript>().celestials.Length - 1) // uses '>'
            {
                celNumber++;
                celestialMenu.value = celNumber;
            }
        }

        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 0) // Speeds up update rate for focusCam in seconds/sec timeframe
        {
            UpdateFocusCamera();
        }
    }

    
    void FixedUpdate()
    {

        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value != 0) // Focus onto celestial[celNumber] in faster timeframes
        {
            UpdateFocusCamera();
        }

    }

    public void UpdateFocusCamera()
    {

        focusCamera.transform.LookAt(gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform);

        objectPosition = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.position;
        objectScale = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.lossyScale;
        offset = objectScale * simulation.celestials[celNumber].transform.GetChild(0).GetComponent<Transform>().localScale.x * 2f; // Multiplying by localScale.x allows camera to scale outwards when radius is changed via UI


        focusCamera.transform.position = objectPosition + offset;

        if (VRplanetProperties.massInput.isFocused == false)
        {
            VRplanetProperties.massInput.text = simulation.celestials[celNumber].GetComponent<Rigidbody>().mass.ToString();
        }

        if (VRplanetProperties.velocityInput.isFocused == false)
        {
            VRplanetProperties.velocityInput.text = simulation.celestials[celNumber].GetComponent<Rigidbody>().velocity.magnitude.ToString();
        }

        if (VRplanetProperties.radiusInput.isFocused == false)
        {
            VRplanetProperties.radiusInput.text = (simulation.celestials[celNumber].transform.GetChild(0).GetComponent<Transform>().localScale.x).ToString();
        }
    }

}