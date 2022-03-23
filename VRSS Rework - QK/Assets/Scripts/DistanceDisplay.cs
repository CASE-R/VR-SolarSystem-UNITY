using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DistanceDisplay : MonoBehaviour
{
    public GameObject sun;
    public GameObject freeCam;

    float xdistance;
    float ydistance;
    float zdistance;

    Vector3 lastFramePos;
    Vector3 thisFramePos;

    float velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        thisFramePos = freeCam.transform.position;

        velocity = Vector3.Distance(thisFramePos, lastFramePos) / Time.fixedDeltaTime;

        xdistance = (sun.transform.position.x - freeCam.transform.position.x)/100;
        ydistance = (sun.transform.position.y - freeCam.transform.position.y)/100;
        zdistance = (sun.transform.position.z - freeCam.transform.position.z)/100;

        gameObject.GetComponent<Text>().text = ("Coordinates from sun:\nX: "
                                                + xdistance.ToString("n5") + " AU\nY: " 
                                                + ydistance.ToString("n5") + " AU\nZ: " 
                                                + zdistance.ToString("n5") + "AU\n"
                                                + "\nCurrent velocity: " + (velocity * 149.598073 / 100).ToString("n5") + "million km/s");

        lastFramePos = freeCam.transform.position;
    }
}
