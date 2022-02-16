using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour //Using https://www.youtube.com/watch?v=wWTOuggRvgc
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject cam5;
    public GameObject camFree;
    public GameObject camMars;
    public GameObject camJupiter;
    public GameObject camSaturn;
    public GameObject camUranus;
    public GameObject camNeptune;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("0Key"))
        {
            camFree.SetActive(true);

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetButtonDown("1Key"))
        {
            cam1.SetActive(true);
            camFree.transform.position = cam1.transform.position;
            camFree.transform.rotation = cam1.transform.rotation;

            // All non used cameras must be set to false
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }

        if (Input.GetButtonDown("2Key"))
        {
            cam2.SetActive(true);
            camFree.transform.position = cam2.transform.position;
            camFree.transform.rotation = cam2.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetButtonDown("3Key"))
        {
            cam3.SetActive(true);
            camFree.transform.position = cam3.transform.position;
            camFree.transform.rotation = cam3.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetButtonDown("4Key"))
        {
            cam4.SetActive(true);
            camFree.transform.position = cam4.transform.position;
            camFree.transform.rotation = cam4.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetButtonDown("5Key"))
        {
            cam5.SetActive(true);
            camFree.transform.position = cam5.transform.position;
            camFree.transform.rotation = cam5.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            camMars.SetActive(true);
            camFree.transform.position = camMars.transform.position;
            camFree.transform.rotation = camMars.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            camJupiter.SetActive(true);
            camFree.transform.position = camJupiter.transform.position;
            camFree.transform.rotation = camJupiter.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            camSaturn.SetActive(true);
            camFree.transform.position = camSaturn.transform.position;
            camFree.transform.rotation = camSaturn.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camUranus.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            camUranus.SetActive(true);
            camFree.transform.position = camUranus.transform.position;
            camFree.transform.rotation = camUranus.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camNeptune.SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            camNeptune.SetActive(true);
            camFree.transform.position = camNeptune.transform.position;
            camFree.transform.rotation = camNeptune.transform.rotation;

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            camFree.SetActive(false);
            camMars.SetActive(false);
            camJupiter.SetActive(false);
            camSaturn.SetActive(false);
            camUranus.SetActive(false);

        }
    }
}
