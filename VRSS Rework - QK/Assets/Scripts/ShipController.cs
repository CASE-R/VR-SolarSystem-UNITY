using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour // Tutorial: https://www.youtube.com/watch?v=J6QR4KzNeJU&t=1187s + taken from FreeCam.cs
{
    [Header("Realistic Variables")]
    [Tooltip("Enter the speed you want to compare to in km/s")]
    public float rocketSpeed;
    [Tooltip("If Rocket Speed is too slow but you want a comparison, use this as a speed multiplier")]
    public float speedMultiplier = 1f;

    [Header("Rocket Properties")]
    public float forwardSpeed = 5f;
    public float sideSpeed = 3.5f;
    public float hoverSpeed = 3f;
    public float lookRotateSpeed = 90f;
    public float rollSpeed = 90f, rollAccel = 3.5f;

    /// <summary>
    /// Sensitivity for free look.
    /// </summary>
    public float freeLookSensitivity = 3f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel.
    /// </summary>
    public float zoomSensitivity = 10f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel (fast mode).
    /// </summary>
    public float fastZoomSensitivity = 50f;

    private float activeForwardSpeed, activeSideSpeed, activeHoverSpeed;
    private float forwardAccel = 2.5f, sideAccel = 2f, hoverAccel = 2f;
    private float rollInput;

    /// <summary>
    /// Set to true when free looking (on right mouse button).
    /// </summary>
    private bool looking = false;

    public ParticleSystem[] partSys;

    // Start is called before the first frame update
    void OnValidate()
    {
        partSys = GetComponentsInChildren<ParticleSystem>();
        forwardSpeed = (rocketSpeed * speedMultiplier) * ((24f * 60f * 60f) / 149598000f) / 100f;
        sideSpeed = (rocketSpeed * speedMultiplier) * ((24f * 60f * 60f) / 149598000f) / 100f;
        hoverSpeed = (rocketSpeed * speedMultiplier) * ((24f * 60f * 60f) / 149598000f) / 100f;
    }

    // Update is called once per frame
    void Update()
    {


        rollInput = Mathf.Lerp(rollInput, Input.GetAxis("Roll"), rollAccel * Time.deltaTime);

        // Take keyboard input for movement
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAccel * Time.deltaTime);
        activeSideSpeed = Mathf.Lerp(activeSideSpeed, Input.GetAxisRaw("Horizontal") * sideSpeed, sideAccel * Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAccel * Time.deltaTime);

        // Apply input as transform; changed from tutorial to follow FreeCam.cs logic
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = transform.position + (transform.right * activeSideSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = transform.position + (transform.forward * activeForwardSpeed * Time.deltaTime);
            //ParticleSystem.MainModule psMain = gameObject.GetComponent<ParticleSystem>().main;
            foreach (ParticleSystem obj in partSys)
            {
                var psMain = obj.main;
                psMain.startSpeed = -activeForwardSpeed;
            }

        }
        else
        {
            foreach (ParticleSystem obj in partSys)
            {
                var psMain = obj.main;
                psMain.startSpeed = 0;
            }
        }

        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, 0, rollInput * rollSpeed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.PageDown) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.PageUp))
        {
            transform.position = transform.position + (Vector3.up * activeHoverSpeed * Time.deltaTime);
        }

        if (looking)
        {
            float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
            float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        float axis = Input.GetAxis("Mouse ScrollWheel");
        if (axis != 0)
        {

            transform.position = transform.position + transform.forward * axis * zoomSensitivity;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartLooking();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopLooking();
        }
    }

    void OnDisable()
    {
        StopLooking();
    }

    /// <summary>
    /// Enable free looking.
    /// </summary>
    public void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Disable free looking.
    /// </summary>
    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
