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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xdistance = (sun.transform.position.x - freeCam.transform.position.x)/100;
        ydistance = (sun.transform.position.y - freeCam.transform.position.y)/100;
        zdistance = (sun.transform.position.z - freeCam.transform.position.z)/100;

        gameObject.GetComponent<Text>().text = ("Coordinates from sun:\nX:" + xdistance + " AU\nY:" + ydistance + " AU\nZ:" + zdistance + "AU");
    }
}
