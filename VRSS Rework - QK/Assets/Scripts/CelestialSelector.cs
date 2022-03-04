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
        camFocus = GetComponent<CameraFocus>();
        UpdateCelNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCelNumber()
    {
        camFocus.celNumber = dropdown.GetComponent<Dropdown>().value;
    }
}
