using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DistanceDisplay : MonoBehaviour
{
    public GameObject sun;
    public GameObject freeCam;

    CameraFocus camFoc;
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
        camFoc = system.GetComponent<CameraFocus>();
        simulation = system.GetComponent<SimulationScript>();
    }

    // Update is called once per frame
    void FixedUpdate() //use this for any motion related calculation since it takes into account the timeScale
    {
        
        if (camFoc.celNumber == -1)
        {
            thisFramePos = freeCam.transform.position;

            velocity = Vector3.Distance(thisFramePos, lastFramePos) / (0.02f);

            xdistance = (sun.transform.position.x - thisFramePos.x) / 100;
            ydistance = (sun.transform.position.y - thisFramePos.y) / 100;
            zdistance = (sun.transform.position.z - thisFramePos.z) / 100;

            gameObject.GetComponent<Text>().text = ("Coordinates from sun:\nX: "
                                                    + xdistance.ToString("n4") + " AU\nY: "
                                                    + ydistance.ToString("n4") + " AU\nZ: "
                                                    + zdistance.ToString("n4") + "AU\n"
                                                    + "\nCurrent velocity: " + (velocity * 149.598073 / 100).ToString("n3") + "million km/in game second");
            lastFramePos = freeCam.transform.position;
        }
        else
        {
            thisFramePos = simulation.celestials[camFoc.celNumber].transform.position;

            if (thisFramePos != lastFramePos)
            {

                velocity = Vector3.Distance(thisFramePos, lastFramePos) / (0.02f);

                xdistance = (sun.transform.position.x - thisFramePos.x) / 100;
                ydistance = (sun.transform.position.y - thisFramePos.y) / 100;
                zdistance = (sun.transform.position.z - thisFramePos.z) / 100;

                gameObject.GetComponent<Text>().text = ("Coordinates from sun:\nX: "
                                                        + xdistance.ToString("n4") + " AU\nY: "
                                                        + ydistance.ToString("n4") + " AU\nZ: "
                                                        + zdistance.ToString("n4") + "AU\n"
                                                        + "\nCurrent velocity: " + (velocity * 149.598073 / 100).ToString("n3") + "million km/in game second");
            }
            lastFramePos = simulation.celestials[camFoc.celNumber].transform.position;
        }
        
    }
}
