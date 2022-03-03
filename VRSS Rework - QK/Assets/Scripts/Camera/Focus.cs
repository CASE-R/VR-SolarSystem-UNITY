using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject freeCamera;

    public GameObject currentCamera;
    [Range(-1, 100)]
    public int celNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
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
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.RightCommand) )
        {
            if (Input.GetKeyDown(KeyCode.Comma)) // uses '<'
            {
                celNumber--;
            }
            if (Input.GetKeyDown(KeyCode.Period)) // uses '>'
            {
                celNumber++;
            }
        }

        if (celNumber == 0) // Focus onto Sun
        {

        }

    }

    private void FixedUpdate()
    {
        
    }
}
