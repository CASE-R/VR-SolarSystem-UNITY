using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject freeCamera;
    public GameObject playerCamera;

    public GameObject currentCamera;
    public int celNumber = 0;

    public Vector3 objectPosition;
    public Vector3 objectScale;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerCamera = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update()
    {
        // Enables FreeCam on WASD Input
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            celNumber = -1;
            currentCamera = freeCamera;
            freeCamera.SetActive(true);

            // All non used cameras must be set to false
            mainCamera.SetActive(false);
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {

        // Updates Position and Rotation of FreeCam
        if (currentCamera != freeCamera)
        {
            freeCamera.transform.position = currentCamera.transform.position;
            freeCamera.transform.rotation = currentCamera.transform.rotation;
        }

        // Used for Ship Controller testing, ignore this for now
        //if (Input.GetKeyDown(KeyCode.P) || celNumber < 0)
        //{
        //    celNumber = -1;
        //    currentCamera = playerCamera;
        //    mainCamera.SetActive(false);
        //    currentCamera.SetActive(true);
        //}

        // Switches between celestial bodies using Ctrl + L/R Arrow Key
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand))
        {
            if (Input.GetKeyDown(KeyCode.Comma) && celNumber > -1) // uses '<'
            {
                celNumber--;
            }
            if (Input.GetKeyDown(KeyCode.Period) && celNumber < gameObject.GetComponent<SimulationScript>().celestials.Length - 1) // uses '>'
            {
                celNumber++;
            }
        }

        if (celNumber > -1) // Focus onto celestial[celNumber]
        {
            currentCamera = mainCamera;
            currentCamera.SetActive(true);
            freeCamera.SetActive(false);

            // Creates short-script lines
            objectPosition = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.position;
            objectScale = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.localScale;

            // Aligns camera to focus on celestial if not looking
            currentCamera.transform.LookAt(gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform);
            currentCamera.transform.position = objectPosition + (5f * objectScale);
        }

    }

}