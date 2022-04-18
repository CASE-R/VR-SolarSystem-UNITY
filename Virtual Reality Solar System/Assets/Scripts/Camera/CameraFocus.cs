using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class CameraFocus : MonoBehaviour
{
    [Tooltip("Camera used to have a focused view of a celestial.")]
    public GameObject focusCamera;
    [Tooltip("Camera used to roam freely in 3D space.")]
    public GameObject freeCamera;

    [Tooltip("Currently active camera.")]
    public GameObject currentCamera;

    [Tooltip("Index of celestials array that is being focused on. For example celNumber = 0 would imply the Sun is being focused on.")]
    public int celNumber;

    [Tooltip("Focused GameObject's position as Vector3.")]
    public Vector3 objectPosition;

    [Tooltip("Focused GameObject's GLOBAL scale as Vector3.")]
    public Vector3 objectScale;

    [Tooltip("Offset for the Focus Camera so it is placed away from the focused object.")]
    public Vector3 offset;

    [Tooltip("Dropdown Menu used to list and selected celestials via UI.")]
    public Dropdown celestialMenu;
    SimulationScript simulation;
    PlanetProperties planetProperties;

    public Text massUnit;
    public Text radiusUnit;

    Vector3 previousPosition;
    float distanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera");

        simulation = gameObject.GetComponent<SimulationScript>();
        planetProperties = gameObject.GetComponent<PlanetProperties>();

        celNumber = 0;
        UpdatePlanetPropertyUnits();
    }

    // Update is called once per frame
    void Update()
    {
        /// Activates FreeCam on WASD input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            celNumber = -1;
            currentCamera = freeCamera;
            freeCamera.SetActive(true);

            // All non used cameras must be set to false
            focusCamera.SetActive(false);
        }

        /// FocusCam switches on 'Ctrl' + '<' or '>' input
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
        {
            if (Input.GetKeyDown(KeyCode.Comma) && celNumber > -1) // uses '<' to switch backwards in celestials[]
            {
                celNumber--;
                celestialMenu.value = celNumber;
            }
            if (Input.GetKeyDown(KeyCode.Period) && celNumber < gameObject.GetComponent<SimulationScript>().celestials.Length - 1) // uses '>' to switch forwards in in celestials[]
            {
                celNumber++;
                celestialMenu.value = celNumber;
            }
        }

        /// Increase rate of UpdateFocusCamera() in smaller timeframes (seconds/realtime second)
        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value == 0 && celNumber > -1) // Focus onto celestial[celNumber] for smaller timeframes at faster rate as timeScale affects effective rate of FixedUpdate(). The value we set is in terms of scaledTime
        {
            UpdateFocusCamera();
        }

        /// Rotate FocusCam around planet
        if (celNumber > -1)
        { // Taken from: https://emmaprats.com/p/how-to-rotate-the-camera-around-an-object-in-unity3d/ and edited to match needs
            if (Input.GetMouseButtonDown(1))
            {
                previousPosition = currentCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 newPosition = currentCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
                Vector3 direction = previousPosition - newPosition;
                distanceToTarget = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.lossyScale.x * simulation.celestials[celNumber].transform.GetChild(0).GetComponent<Transform>().localScale.x * 2f; // Multiplying by localScale.x allows camera to scale outwards when radius is changed via UI

                float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
                float rotationAroundXAxis = direction.y * 180; // camera moves vertically

                currentCamera.transform.position = simulation.celestials[celNumber].transform.position;

                currentCamera.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                currentCamera.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

                currentCamera.transform.Translate(new Vector3(0, 0, -distanceToTarget));

                previousPosition = newPosition;
            }

        }

    }

    void FixedUpdate()
    {
        /// Update Position and Rotation of FreeCam in focus mode
        if (currentCamera != freeCamera) // i.e currentCamera = focusCamera
        {
            freeCamera.transform.position = currentCamera.transform.position;
            freeCamera.transform.rotation = currentCamera.transform.rotation;
        }

        /// Update Position and Rotation of FocusCam in focus mode at rate = Time.fixedDeltaTime
        if (gameObject.GetComponent<UpdateTimeScale>().timeUnitMenu.value != 0 && !Input.GetKey(KeyCode.Mouse1)) // Focus onto celestial[celNumber] in faster timeframes
        {
            UpdateFocusCamera();
        }

    }

    /// <summary>
    /// Updates FocusCamera position, rotation and state when necessary. UI info fields are also updated here.
    /// </summary>
    public void UpdateFocusCamera()
    {
        if (celNumber >-1)
        {
            if (!Input.GetKeyDown(KeyCode.Mouse1))
            {
                currentCamera.transform.LookAt(gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform);
                currentCamera = focusCamera;
                currentCamera.SetActive(true);
                freeCamera.SetActive(false);

                objectPosition = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.position;
                objectScale = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.lossyScale;
                offset = objectScale * simulation.celestials[celNumber].transform.GetChild(0).GetComponent<Transform>().localScale.x * 2f; // Multiplying by localScale.x allows camera to scale outwards when radius is changed via UI

                currentCamera.transform.position = objectPosition + offset;
            }

            

            if (planetProperties.massInput.isFocused == false)
            {
                planetProperties.massInput.text = simulation.celestials[celNumber].GetComponent<Rigidbody>().mass.ToString();
            }

            if (planetProperties.velocityInput.isFocused == false)
            {
                planetProperties.velocityInput.text = simulation.celestials[celNumber].GetComponent<Rigidbody>().velocity.magnitude.ToString();
            }

            if (planetProperties.radiusInput.isFocused == false)
            {
                planetProperties.radiusInput.text = (simulation.celestials[celNumber].transform.GetChild(0).GetComponent<Transform>().localScale.x).ToString();
            }

            UpdatePlanetPropertyUnits();
        }

        
    }

    /// <summary>
    /// Method to update mass and radius units for PlanetProperties UI element. This gives the user a better sense of what properties they are changing relative to.
    /// </summary>
    public void UpdatePlanetPropertyUnits()
    {
        massUnit.text = "Earth masses";
        radiusUnit.text = simulation.celestials[celNumber].name + "\nradii";
    }

}