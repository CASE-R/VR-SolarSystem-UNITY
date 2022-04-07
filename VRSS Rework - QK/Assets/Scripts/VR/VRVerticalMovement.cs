using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class VRVerticalMovement : MonoBehaviour
{
    public XRNode inputSourceLeft;
    public XRNode inputSourceRight;
    public float speed = 1f;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public LayerMask groundLayer;

    private bool leftStickPressed;
    private bool rightStickPressed;

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
        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out leftStickPressed);
        deviceRight.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rightStickPressed);
    }

    private void FixedUpdate()
    {
        // Horizontal
        Quaternion headYaw = Quaternion.Euler(0, rig.Camera.gameObject.transform.position.y, 0);

        // Vertical
        if (leftStickPressed)
        {
            character.Move(Vector3.up * -speed * Time.fixedDeltaTime);
        }
        if (rightStickPressed)
        {
            character.Move(Vector3.up * speed * Time.fixedDeltaTime);
        }
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.gameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height/2, capsuleCenter.z);
    }
}
