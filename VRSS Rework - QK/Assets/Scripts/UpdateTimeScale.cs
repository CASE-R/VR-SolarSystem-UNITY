using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTimeScale : MonoBehaviour
{
    public GameObject slider;
    public GameObject timeInput;
    private SimulationScript simulation;

    public Dropdown timeUnitMenu;

    // Start is called before the first frame update
    void Start()
    {
        simulation = GetComponent<SimulationScript>();
        updateTimescale();
        updateTimeUnit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTimeInput()
    {
        timeInput.GetComponent<InputField>().text = slider.GetComponent<Slider>().value.ToString();
    }

    public void updateSlider()
    {
        slider.GetComponent<Slider>().value = float.Parse(timeInput.GetComponent<InputField>().text);
    }

    public void updateTimeUnit()
    {
        if (timeUnitMenu.value == 0)
        {
            simulation.timeUnitMultiplier = 1f / (24 * 60 * 60);
            //Time.fixedDeltaTime = simulation.initialFixedTimeStep * (1 / (24 * 60 * 60));
        }

        else if (timeUnitMenu.value == 1)
        {
            simulation.timeUnitMultiplier = 1;
            //Time.fixedDeltaTime = simulation.initialFixedTimeStep;
        }

        else if (timeUnitMenu.value == 2)
        {
            simulation.timeUnitMultiplier = 1 * 7;
            //Time.fixedDeltaTime = simulation.initialFixedTimeStep * 7;
        }
    }

    public void updateTimescale()
    {
        if (slider.GetComponent<Slider>().value == 0)
        {
            simulation.initialTimeScale = 0.00000001f;   //if the slider is moved all the way to the left, set it to a very low number to avoid dividing by 0
        }

        else
        {
            simulation.initialTimeScale = slider.GetComponent<Slider>().value;
        }
    }
}
