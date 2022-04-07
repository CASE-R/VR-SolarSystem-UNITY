using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class VRContinuousMovement : MonoBehaviour //https://www.youtube.com/watch?v=5NRTT8Tbmoc&list=PLrk7hDwk64-a_gf7mBBduQb3PEBYnG4fU&index=5
{
    public XRNode inputSourceLeft;
    public XRNode inputSourceRight;
    public float speed = 2f;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public Transform cameraTransform;
    public LayerMask groundLayer;

    private Vector2 inputAxis;
    private bool leftStickPressed;
    private bool rightStickPressed;
    private float leftTriggerValue;
    private float rightTriggerValue;

    private XROrigin rig;
    private float additionalHeight = 0.15f;

    private Vector3 flyDirection;
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

        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis); // Gives 2D motion from left joystick
        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftStickPressed); // Checks for press on left stick
        deviceRight.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rightStickPressed); // Checks for press on right stick
        deviceLeft.TryGetFeatureValue(CommonUsages.trigger, out leftTriggerValue); // Checks for how pressed left trigger is
        deviceRight.TryGetFeatureValue(CommonUsages.trigger, out rightTriggerValue); // Checks for how pressed right trigger is
    }

    private void FixedUpdate()
    {
        // Horizontal
        Vector3 direction = cameraTransform.rotation * new Vector3(inputAxis.x, 0f, inputAxis.y); // Gives a resultant direction to transform the VR Player, using the directino they are facing. This also allows 3D movement

        character.Move(direction * ((speed*rightTriggerValue) + (speed*(1-leftTriggerValue)))); // Applies motion, this is however affected by timeScale


        // Vertical movement independent of look direction
        if (leftStickPressed)
        {
            character.Move(Vector3.up * -speed * Time.fixedDeltaTime);
        }
        if (rightStickPressed)
        {
            character.Move(Vector3.up * speed * Time.fixedDeltaTime);
        }
    }

}
