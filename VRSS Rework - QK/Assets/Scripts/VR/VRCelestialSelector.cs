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

    public void UpdateCelNumber()
    {
        if (dropdown.GetComponent<Dropdown>().value == 18) // True when "FreeCam" is selected in dropdown menu of UI
        {
            VRCamSwitch.celNumber = -1; // 'Sets' FreeCam mode
            // Transforms focusCam to current player position, can be commented out to leave focusCam in last position as a celestial is deleted
            VRCamSwitch.focusCamera.transform.position = VRCamSwitch.HMDCamera.transform.position;
            VRCamSwitch.focusCamera.transform.rotation = VRCamSwitch.HMDCamera.transform.rotation;
        }
        else // Just tells some scripts which index of the celestial list we are focusing on in the UI
        {
            VRCamSwitch.celNumber = dropdown.GetComponent<Dropdown>().value;
        }
    }
}
