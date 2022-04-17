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
        //If we're in freeCam, we calculate the velocity and position of the camera
        if (camFoc.celNumber == -1)
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

        //If we're not in free cam, we calculate the position of the currently followed celestial
        else
        {
            thisFramePos = simulation.celestials[camFoc.celNumber].transform.position;

            //Sometimes celestials don't move every frame (in different timeScales) hence only work out the speed when they move
            //Now the same calculations follow, just like in the freeCam case
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
