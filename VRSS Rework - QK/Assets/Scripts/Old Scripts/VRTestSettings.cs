using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTestSettings : MonoBehaviour
{
    [Header("Simulation Settings")]
    [SerializeField] public int frameRate = 60;
    [Range(0f, 100f)]
    [SerializeField] public float initialTimeScale = 1f;
    [Range(0.00000001f, 1f)]
    [SerializeField] public float initialFixedTimeStep = 0.02f;

    

    // Start is called before the first frame update
    void Start()
    {
        // Caps/Syncs Simulation FPS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = initialTimeScale;

    }
}
