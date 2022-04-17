using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class VRContinuousMovement : MonoBehaviour 
    // Based off Valem's VR Tutorial Playlist, Video 4: https://www.youtube.com/watch?v=5NRTT8Tbmoc&list=PLrk7hDwk64-a_gf7mBBduQb3PEBYnG4fU&index=5 and this thread: https://answers.unity.com/questions/1700053/how-to-walk-in-the-direction-the-player-is-looking.html
{
    public XRNode inputSourceLeft;
    public XRNode inputSourceRight;
    public float speed = 2f;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public Transform cameraTransform;
    public LayerMask groundLayer;

    private Vector2 inputLeftAxis;
    private Vector2 inputRightAxis;
    private bool leftStickPressed;
    private bool rightStickPressed;
    private float leftTriggerValue;
    private float rightTriggerValue;

    private XROrigin rig;
    private float additionalHeight = 0.15f;

    private CharacterController character;
    
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        
        InputDevice deviceLeft = InputDevices.GetDeviceAtXRNode(inputSourceLeft);
        InputDevice deviceRight = InputDevices.GetDeviceAtXRNode(inputSourceRight);

        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputLeftAxis); // Gives 2D motion from left joystick
        deviceRight.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputRightAxis); // Gives 2D motion from left joystick
        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftStickPressed); // Checks for press on left stick
        deviceRight.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rightStickPressed); // Checks for press on right stick
        deviceLeft.TryGetFeatureValue(CommonUsages.trigger, out leftTriggerValue); // Checks for how pressed left trigger is
        deviceRight.TryGetFeatureValue(CommonUsages.trigger, out rightTriggerValue); // Checks for how pressed right trigger is

        // Horizontal
        Vector3 direction = cameraTransform.rotation * new Vector3(inputLeftAxis.x, 0f, inputLeftAxis.y); // Gives a resultant direction to transform the VR Player, using the directino they are facing. This also allows 3D movement

        character.Move(direction * ((speed * rightTriggerValue) + (speed * (1 - leftTriggerValue)))); // Applies motion, this is however affected by timeScale


        // Vertical movement independent of look direction. leftStick gives downwards movement with speed in opposite (negative) direction to rightStick which gives upwards movement
        if (leftStickPressed)
        {
            character.Move(Vector3.up * -speed * Time.unscaledDeltaTime);
        }
        if (rightStickPressed)
        {
            character.Move(Vector3.up * speed * Time.unscaledDeltaTime);
        }

        // Rotate VR Rig
        character.transform.localEulerAngles += new Vector3(Mathf.Clamp(-inputRightAxis.y,-60f, 60f), Mathf.Clamp(inputRightAxis.x, -60f, 60f), 0f);

    }

    private void FixedUpdate()
    {
        
    }

}
