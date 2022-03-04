using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTimeScale : MonoBehaviour
{
    public GameObject slider;
    private SimulationScript simulation;

    // Start is called before the first frame update
    void Start()
    {
        simulation = GetComponent<SimulationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTimescale()
    {
        simulation.initialTimeScale = slider.GetComponent<Slider>().value;
    }
}
