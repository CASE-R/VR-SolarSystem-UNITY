using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimerScript : MonoBehaviour
{
    private DateTime startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Text>().text = startTime.ToString();
    }
}
