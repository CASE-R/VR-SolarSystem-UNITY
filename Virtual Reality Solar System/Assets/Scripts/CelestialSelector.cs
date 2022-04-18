using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CelestialSelector : MonoBehaviour
{
    public GameObject dropdown;
    private CameraFocus camFocus;

    // Start is called before the first frame update
    void Start()
    {
        //we update the camera at the beginning to focus the camera on the first celestial (sun)
        camFocus = GetComponent<CameraFocus>();
        UpdateCelNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //this function gets called whenever a new value is picked from the dropdown menu
    public void UpdateCelNumber()
    {
        //if the value is set to 18 (which is freeCam) we set the celestial number to -1 which corresponds to no celestial
        if (dropdown.GetComponent<Dropdown>().value == 18)
        {
            camFocus.celNumber = -1;
        }
        else
        {
            //in any other case we set the celestial number to the value in the celestial menu which are indexed accordingly
            camFocus.celNumber = dropdown.GetComponent<Dropdown>().value;
        }
    }
}
