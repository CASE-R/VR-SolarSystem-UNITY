using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCamSwitch : MonoBehaviour
{
    private CameraFocus camFoc;
    private SimulationScript simScript;

    // Start is called before the first frame update
    private void Start()
    {
        camFoc.currentCamera = gameObject.GetComponentInChildren<Camera>().gameObject;
        camFoc.currentCamera.tag = "MainCamera";
    }

    // Update is called once per frame
    private void Update()
    {
        if (camFoc.celNumber > -1)
        {
            gameObject.transform.position = camFoc.focusCamera.transform.position;
        }
    }
}
