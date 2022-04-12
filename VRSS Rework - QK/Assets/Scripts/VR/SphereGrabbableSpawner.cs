using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereGrabbableSpawner : MonoBehaviour
{
    public GameObject grabbablePreFab;
    public GameObject parent;
    public Transform spawnLocation;
    
    public float spawnCooldown = 2f;

    private float cooldownResetTime = 0f;

    private bool isButtonPressed = false;

    VRCelestialSelector VRCelSel;
    SimulationScript simulationScript;
    public Dropdown VRCelSelDropDown;
    GameObject spawnedObj;
    public GameObject[] celestialArrayCheck; // Used for checking if celestials array updates
    private int celNewLength;

    // Start is called before the first frame update
    void Start()
    {
        VRCelSel = GetComponent<VRCelestialSelector>();
        simulationScript = GetComponent<SimulationScript>();
        parent = gameObject;
        celNewLength = simulationScript.celestials.Length;
        Debug.Log("CelNewLength is " + celNewLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (CooledDown() && isButtonPressed)
        {
            Spawn();
        }
        
        if (celestialArrayCheck.Length != simulationScript.celestials.Length)
        {
            celestialArrayCheck = simulationScript.celestials;
            PopulateDropdown(VRCelSelDropDown, celestialArrayCheck);
        }
    }

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

    private void Spawn()
    {
        spawnedObj = (GameObject)Instantiate(grabbablePreFab, spawnLocation.position + (2f * grabbablePreFab.transform.localScale), spawnLocation.rotation, parent.transform); // Creates object 2x radii of the object away
        spawnedObj.name = "Grabbable Celestial " + Time.time;

        gameObject.GetComponent<SimulationScript>().celestials = GameObject.FindGameObjectsWithTag("Celestial");
        Debug.Log("Spawned in grabbable PreFab whilst " + isButtonPressed);

        cooldownResetTime = Time.time + spawnCooldown; // Updates time to compare cooldown to, which is set higher than Time.time for 'spawnCooldown' seconds
        isButtonPressed = false; // Resets isButtonPressed
        Debug.Log("Button pressed is " + isButtonPressed);
        celNewLength += 1;
        Debug.Log("CelNewLength is " + celNewLength);

    }
    private bool CooledDown()
    {
        return Time.time > cooldownResetTime; // True whenever the cooldown has happened
    }

    public void SpawnButtonPressed() // Tied to a button which runs this on activation
    {
        isButtonPressed = true;
    }

}
