using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject freeCamera;

    public GameObject currentCamera;
    public int celNumber = 0;



    // Start is called before the first frame update
    void Start()
    {
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera");
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        


        // Enables FreeCam on WASD Input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            celNumber = -1;
            currentCamera = freeCamera;
            freeCamera.SetActive(true);

            // All non used cameras must be set to false
            mainCamera.SetActive(false);
        }

        // Updates Position and Rotation of FreeCam
        if (currentCamera != freeCamera)
        {
            freeCamera.transform.position = currentCamera.transform.position;
            freeCamera.transform.rotation = currentCamera.transform.rotation;
        }

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

        if (celNumber >-1) // Focus onto celestial[celNumber]
        {
            currentCamera = mainCamera;
            currentCamera.SetActive(true);
            freeCamera.SetActive(false);

            // Creates short-script lines
            Vector3 objectPosition = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.position;
            Vector3 objectScale = gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform.localScale;

            // Aligns camera to focus on celestial
            currentCamera.transform.LookAt(gameObject.GetComponent<SimulationScript>().celestials[celNumber].transform);

            currentCamera.transform.position = objectPosition + 1.5f * objectScale;

        }

    }

}
