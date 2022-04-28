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

    //these 2 functions link the slider and the input field together
    //this way changing one will adjust the other one to keep them linked
    public void updateTimeInput()
    {
        timeInput.GetComponent<InputField>().text = slider.GetComponent<Slider>().value.ToString();
    }

    public void updateSlider()
    {
        slider.GetComponent<Slider>().value = float.Parse(timeInput.GetComponent<InputField>().text);
    }

    //called whenever a timeScaleUnit is changed
    public void updateTimeUnit()
    {
        //the menu values correspon to different timescales per real-life second
        //the if statements are simple unit conversions from 1 earth day per second
        if (timeUnitMenu.value == 0)
        {
            simulation.timeUnitMultiplier = 1f / (24 * 60 * 60);
            Time.fixedDeltaTime = simulation.initialFixedTimeStep / 7f;
        }

        else if (timeUnitMenu.value == 1)
        {
            simulation.timeUnitMultiplier = 1;
            Time.fixedDeltaTime = simulation.initialFixedTimeStep;
        }

        else if (timeUnitMenu.value == 2)
        {
            simulation.timeUnitMultiplier = 1 * 7;
            Time.fixedDeltaTime = simulation.initialFixedTimeStep * 7f;
        }
    }

    //gets called whenever the slider value changes
    public void updateTimescale()
    {
        if (slider.GetComponent<Slider>().value == 0)
        {
            //if the slider is moved all the way to the left, set it to a very low number to avoid dividing by 0
            simulation.initialTimeScale = 0.01f;
        }

        else
        {

            //the timescale of the simulation is adjusted to be multiplies by the slider value set by the user
            simulation.initialTimeScale = slider.GetComponent<Slider>().value;

            ///we also adjust the time between calculations so that higher timescales can be simulated without lag
            ///this has a slight effect on the accuracy of the simulation but no big deviations can be seen with the fastest timescale the UI offers
            //Time.fixedDeltaTime = simulation.initialFixedTimeStep * simulation.initialTimeScale * simulation.timeUnitMultiplier;

        }
    }
}
