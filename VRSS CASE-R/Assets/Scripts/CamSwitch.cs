using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour //Using https://www.youtube.com/watch?v=wWTOuggRvgc
{
    public GameObject cam1;
    public GameObject cam2;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("1Key"))
        {
            cam1.SetActive(true);
            cam2.SetActive(false); // All non used cameras must be set to false
        }

        if (Input.GetButtonDown("2Key"))
        {
            cam1.SetActive(false);
            cam2.SetActive(true);  // All non used cameras must be set to false
        }
    }
}
