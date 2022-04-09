using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGrabbableSpawner : MonoBehaviour
{
    public GameObject grabbablePreFab;
    public float spawnCooldown = 5f;

    private float cooldownResetTime = 0f;

    private bool isButtonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Instantiate(grabbablePreFab, transform.position, transform.rotation); // Creates object
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
