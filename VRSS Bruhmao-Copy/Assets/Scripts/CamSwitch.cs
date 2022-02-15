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
    public GameObject cam6;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("1Key"))
        {
            cam1.SetActive(true);

            // All non used cameras must be set to false
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            cam6.SetActive(false);

        }

        if (Input.GetButtonDown("2Key"))
        {
            cam2.SetActive(true);

            // All non used cameras must be set to false
            cam1.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            cam6.SetActive(false);

        }
        if (Input.GetButtonDown("3Key"))
        {
            cam3.SetActive(true);

            // All non used cameras must be set to false
            cam2.SetActive(false);
            cam1.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            cam6.SetActive(false);

        }
        if (Input.GetButtonDown("4Key"))
        {
            cam4.SetActive(true);

            // All non used cameras must be set to false
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam1.SetActive(false);
            cam5.SetActive(false);
            cam6.SetActive(false);

        }
        if (Input.GetButtonDown("5Key"))
        {
            cam5.SetActive(true);

            // All non used cameras must be set to false
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam1.SetActive(false);
            cam6.SetActive(false);

        }
        if (Input.GetButtonDown("0Key"))
        {
            cam6.SetActive(true);

            // All non used cameras must be set to false
            cam2.SetActive(false);
            cam3.SetActive(false);
            cam4.SetActive(false);
            cam5.SetActive(false);
            cam1.SetActive(false);

        }
    }
}
