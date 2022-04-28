using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereGrabbableSpawner : MonoBehaviour // Modelled off this video: https://www.youtube.com/watch?v=9KOHclqSmR4 and customised for personal use
{
    [Tooltip("PreFab that is used for instantiating a grabbable object.")]
    public GameObject grabbablePreFab;
    [Tooltip("GameObject used to use as the instantiated object.")]
    public GameObject systemObj;
    [Tooltip("Transform used to spawn relative to. Typically set to whatever object to spawn in front of the player.")]
    public Transform spawnLocation;

    [Tooltip("How long a delay there is in seconds before being able to instantiate a new object.")]
    public float spawnCooldown = 3f;

    private float cooldownResetTime = 0f; // Used as part of the Cooldown system

    private bool isButtonPressed = false; // Triggered by a UI Button press

    VRCelestialSelector VRCelSel;
    SimulationScript simulationScript;
    [Tooltip("Dropdown menu used to list and select celestials. This is so it can be updated upon creating a new celestial.")]
    public Dropdown VRCelSelDropDown;
    GameObject spawnedObj;
    [Tooltip("Used as a comparison to the original Celestial list in SimulationScript.cs")]
    public GameObject[] celestialArrayCheck; // Used for checking if celestials array updates
    private int celNewLength;

    // Start is called before the first frame update
    void Start()
    {
        VRCelSel = systemObj.GetComponent<VRCelestialSelector>();
        simulationScript = systemObj.GetComponent<SimulationScript>();
       // parent = gameObject; // This script is to be attached to the "System" GameObject used for a lot of the simulation settings
        celNewLength = simulationScript.celestials.Length;
        Debug.Log("CelNewLength is " + celNewLength);
    }

    // Update is called once per frame
    void Update()
    {
        //if (CooledDown() && isButtonPressed)
        //{
        //    Spawn();
        //}
        
        if (celestialArrayCheck.Length != simulationScript.celestials.Length)
        {
            celestialArrayCheck = simulationScript.celestials;
            PopulateDropdown(VRCelSelDropDown, celestialArrayCheck);
        }
    }

    /// <summary>
    /// Method used to update the Dropdown menu listing all active celestials by creating a new list from an array and refreshing the entire dropdown menu.
    /// </summary>
    private void PopulateDropdown(Dropdown dropdownMenu, GameObject[] optionsArray)
    {
        List<string> options = new List<string>();
        foreach (GameObject option in optionsArray)
        {
            options.Add(option.name);
        }

        dropdownMenu.ClearOptions();
        dropdownMenu.AddOptions(options);
        Debug.Log("Updated Dropdown Menu");
    }

    /// <summary>
    /// Method used to create/instantiate a PreFab model that allows the addition of a celestial object that can be grabbed via VR input. Conditions and timers are reset here also.
    /// </summary>
    //private void Spawn()
    //{
        
    //    if (CooledDown() && isButtonPressed)
    //    {
    //        spawnedObj = (GameObject)Instantiate(grabbablePreFab, new Vector3(0,1,0), Quaternion.identity, systemObj); // Creates object at origin
    //        spawnedObj.name = "Grabbable Celestial " + Time.time;


    //        gameObject.GetComponent<SimulationScript>().celestials = GameObject.FindGameObjectsWithTag("Celestial");
    //        Debug.Log("Spawned in grabbable PreFab whilst " + isButtonPressed);
    //        isButtonPressed = false; // Resets isButtonPressed

    //        cooldownResetTime = Time.time + spawnCooldown; // Updates time to compare cooldown to, which is set higher than Time.time for 'spawnCooldown' seconds
    //        CooledDown(); // Helps to reset conditions, so only one object is instantiated
    //    }
        
    //    Invoke("ReInitialise", Time.fixedDeltaTime); // Repositions spawnedObj in next fixedUpdate to player

        
    //    Debug.Log("Button pressed is " + isButtonPressed);
    //    celNewLength += 1;
    //    Debug.Log("CelNewLength is " + celNewLength);

    //}

    private void ReInitialise()
    {
        Vector3 instantiatePos = gameObject.transform.position + (2f * grabbablePreFab.transform.localScale);
        spawnedObj.transform.position = instantiatePos; // 2x radii of the object away from player


        RefreshCelestials();
    }

    public void RefreshCelestials()
    {
        systemObj.GetComponent<SimulationScript>().celestials = GameObject.FindGameObjectsWithTag("Celestial");
        PopulateDropdown(VRCelSelDropDown, celestialArrayCheck);
    }

    public void UISpawnPress()
    {
        spawnedObj = (GameObject)Instantiate(grabbablePreFab, gameObject.transform.position, Quaternion.identity, systemObj.transform); // Creates object at origin
        spawnedObj.name = "Grabbable Celestial " + Time.time;

        Invoke("ReInitialise", Time.fixedDeltaTime); // Repositions spawnedObj in next fixedUpdate to player
    }


    /// <summary>
    /// Method that checkes if enough time has passed to consider the instantiate button to have 'cooled down'.
    /// </summary>
    //private bool CooledDown()
    //{
    //    return Time.time > cooldownResetTime; // True whenever the cooldown has happened
    //}

    ///// <summary>
    ///// Method run upon UI Button press used as an indirect UI interaction event.
    ///// </summary>
    //public void SpawnButtonPressed() // Tied to a button which runs this on activation
    //{
    //    isButtonPressed = true;
    //}

}
