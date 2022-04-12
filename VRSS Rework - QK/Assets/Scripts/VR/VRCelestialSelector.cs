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
        VRCamSwitch.celNumber = dropdown.GetComponent<Dropdown>().value;
    }
}
