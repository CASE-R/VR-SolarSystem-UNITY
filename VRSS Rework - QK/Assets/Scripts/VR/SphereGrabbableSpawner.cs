using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphereGrabbableSpawner : MonoBehaviour
{
    public GameObject grabbablePreFab;
    public GameObject parent;
    public Transform spawnLocation;
    
    public float spawnCooldown = 5f;

    private float cooldownResetTime = 0f;

    private bool isButtonPressed = false;

    VRCelestialSelector VRCelSel;
    public Dropdown VRCelSelDropDown;
    GameObject spawnedObj;

    // Start is called before the first frame update
    void Start()
    {
        VRCelSel = GetComponent<VRCelestialSelector>();
        VRCelSelDropDown = VRCelSel.GetComponent<Dropdown>();
        parent = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (CooledDown() && isButtonPressed)
        {
            Spawn();
            isButtonPressed = false; // Resets isButtonPressed
        }
    }

    private void Spawn()
    {
        cooldownResetTime = Time.time + spawnCooldown; // Updates time to compare cooldown to, which is set higher than Time.time for 'spawnCooldown' seconds
        spawnedObj = (GameObject)Instantiate(grabbablePreFab, spawnLocation.position + (2f * grabbablePreFab.transform.localScale), spawnLocation.rotation, parent.transform); // Creates object 2x radii of the object away
        spawnedObj.name = "Grabbable Celestial " + Time.time;

        List<string> namesToAdd = new List<string> { spawnedObj.name };
        VRCelSelDropDown.AddOptions(namesToAdd);

        gameObject.GetComponent<SimulationScript>().celestials = GameObject.FindGameObjectsWithTag("Celestial");
        Debug.Log("Spawned in grabbable PreFab");
    }
    private bool CooledDown()
    {
        return Time.time >= cooldownResetTime; // True whenever the cooldown has happened
    }

    public void SpawnButtonPressed() // Tied to a button which runs this on activation
    {
        isButtonPressed = true;
    }

}
