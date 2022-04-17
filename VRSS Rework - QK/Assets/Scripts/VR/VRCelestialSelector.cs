using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRCelestialSelector : MonoBehaviour
{
    public GameObject dropdown;
    private VRCamSwitch VRCamSwitch;

    // Start is called before the first frame update
    void Start()
    {
        VRCamSwitch = GetComponent<VRCamSwitch>();
        UpdateCelNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Method called to set the celestial number to the value in the dropdown menu which are indexed accordingly, just like the non-VR version with the exception of a seperate FreeCam as the User is perpetually in a FreeCam mode.
    /// </summary>
    public void UpdateCelNumber()
    {
        VRCamSwitch.celNumber = dropdown.GetComponent<Dropdown>().value;
    }
}
