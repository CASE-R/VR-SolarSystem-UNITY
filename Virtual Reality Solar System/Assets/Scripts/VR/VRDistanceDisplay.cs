using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VRDistanceDisplay : MonoBehaviour
{
    // Simplified VR version of the non-VR script which similar namespace due to redefinition of the cameras
    public GameObject sun;
    public GameObject freeCam;

    VRCamSwitch VRCamSwitch;
    SimulationScript simulation;
    public GameObject system;

    float xdistance;
    float ydistance;
    float zdistance;

    Vector3 lastFramePos;
    Vector3 thisFramePos;

    float velocity;

    // Start is called before the first frame update
    void Start()
    {
        VRCamSwitch = system.GetComponent<VRCamSwitch>();
        simulation = system.GetComponent<SimulationScript>();
    }

    // Update is called once per frame
    void FixedUpdate() //use this for any motion related calculation since it takes into account the timeScale
    {
        thisFramePos = freeCam.transform.position;

        velocity = Vector3.Distance(thisFramePos, lastFramePos) / (0.02f);
        //Velocity is calculated after thisFramePos is updated while lastFramePos is still from the previous frame

        xdistance = (sun.transform.position.x - thisFramePos.x) / 100;
        ydistance = (sun.transform.position.y - thisFramePos.y) / 100;
        zdistance = (sun.transform.position.z - thisFramePos.z) / 100;
        //Since 1 AU is 100 unity units, we divide by 100 to get the distance in AU

        gameObject.GetComponent<Text>().text = ("Coordinates from sun:\nX: "
                                                + xdistance.ToString("n4") + " AU\nY: "
                                                + ydistance.ToString("n4") + " AU\nZ: "
                                                + zdistance.ToString("n4") + "AU\n"
                                                + "\nCurrent velocity: " + (velocity * 149.598073 / 100).ToString("n3") + "million km/realtime sec");
        //All the distances are formatted as strings to 4 decimal places, velocity converted from 100v AU/s to millions of km/s
        lastFramePos = freeCam.transform.position;
        //After all the calculations are done, lastFramePos can be updated as it's now the end of the frame
    }
}
