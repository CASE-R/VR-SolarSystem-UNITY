using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour //Using https://www.youtube.com/watch?v=wWTOuggRvgc
{
    public GameObject camSun;
    public GameObject camMercury;
    public GameObject camVenus;
    public GameObject camEarth;
    public GameObject camMoon;
    public GameObject camMars;
    public GameObject camJupiter;
    public GameObject camSaturn;
    public GameObject camUranus;
    public GameObject camNeptune;
    public GameObject camFree;
    public GameObject currentCamera;

    private void Start()
    {
        currentCamera = camSun;
    }


    // Update is called once per frame
    void Update()
    {
        // Enables FreeCam on WASD Input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            currentCamera = camFree;
            camFree.SetActive(true);

            // All non used cameras must be set to false
            camSun.SetActive(false);
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
        }

        // Updates Position and Rotation of FreeCam
        if (currentCamera != camFree)
        {
            camFree.transform.position = currentCamera.transform.position;
            camFree.transform.rotation = currentCamera.transform.rotation;
        }

        // Below code swaps between main 9 celestial bodies + Earth's Moon using 0-9 keys and sets the currentCamera
        if (Input.GetButtonDown("1Key"))
        {
            currentCamera = camSun;
            camSun.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("2Key"))
        {
            currentCamera = camMercury;
            camMercury.SetActive(true);

            // All non used cameras must be set to false
            camSun.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("3Key"))
        {
            currentCamera = camVenus;
            camVenus.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camSun.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("4Key"))
        {
            currentCamera = camEarth;
            camEarth.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camSun.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("5Key"))
        {
            currentCamera = camMoon;
            camMoon.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camSun.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("6Key"))
        {
            currentCamera = camMars;
            camMars.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camSun.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("7Key"))
        {
            currentCamera = camJupiter;
            camJupiter.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camSun.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("8Key"))
        {
            currentCamera = camSaturn;
            camSaturn.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSun.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("9Key"))
        {
            currentCamera = camUranus;
            camUranus.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camSun.SetActive(false);
            camNeptune.SetActive(false);
            camFree.SetActive(false);
        }

        if (Input.GetButtonDown("0Key"))
        {
            currentCamera = camNeptune;
            camNeptune.SetActive(true);

            // All non used cameras must be set to false
            camMercury.SetActive(false);
            camVenus.SetActive(false);
            camEarth.SetActive(false);
            camMoon.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camSun.SetActive(false);
            camFree.SetActive(false);
        }
    }
}
