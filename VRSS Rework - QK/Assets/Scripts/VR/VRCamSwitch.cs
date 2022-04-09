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
    BodyProperties bodyProperties;

    Vector3 previousPosition;
    float distanceToTarget;
    // Start is called before the first frame update
    void Start()
    {
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera");

        simulation = gameObject.GetComponent<SimulationScript>();
        VRplanetProperties = gameObject.GetComponent<VRPlanetProperties>();

        celNumber = -1;
        currentCamera = HMDCamera;
        HMDCamera.SetActive(true);
        focusCamera.SetActive(true);

    }

    void Update()
    {
        //// Enables FreeCam on WASD Input
        //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        //{
        //    celNumber = -1;
        //    currentCamera = HMDCamera;
        //    HMDCamera.SetActive(true);
        //}

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

        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 0) // Focus onto celestial[celNumber] for smaller timeframes at faster rate
        {
            UpdateFocusCamera();
        }

        //if (celNumber > -1)
        //{ // Taken from: https://emmaprats.com/p/how-to-rotate-the-camera-around-an-object-in-unity3d/
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        previousPosition = currentCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
        //    }
        //    else if (Input.GetMouseButton(1))
        //    {
        //        Vector3 newPosition = currentCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
        //        Vector3 direction = previousPosition - newPosition;
        //        distanceToTarget = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.lossyScale.x * simulation.celestials[celNumber].transform.GetChild(0).GetComponent<Transform>().localScale.x * 2f; // Multiplying by localScale.x allows camera to scale outwards when radius is changed via UI

        //        float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
        //        float rotationAroundXAxis = direction.y * 180; // camera moves vertically

        //        currentCamera.transform.position = simulation.celestials[celNumber].transform.position;

        //        currentCamera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
        //        currentCamera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

        //        currentCamera.transform.Translate(new Vector3(0, 0, -distanceToTarget));

        //        previousPosition = newPosition;
        //    }

        //}



    }
    // Update is called once per frame
    void FixedUpdate()
    {

        // Updates Position and Rotation of FreeCam
        if (currentCamera != HMDCamera)
        {
            HMDCamera.transform.position = currentCamera.transform.position;
            HMDCamera.transform.rotation = currentCamera.transform.rotation;
        }

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