using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class VRMovement : MonoBehaviour
{
    public XRNode inputSourceLeft;
    public XRNode inputSourceRight;
    public float speed = 1f;
    public float fallingSpeed = 10f;
    public float gravity = -9.81f;
    public float additionalHeight = 0.15f;
    public GameObject head;
    public GameObject leftHand;
    public GameObject rightHand;
    public LayerMask groundLayer;

    private Vector2 inputAxis;
    private bool verticalDown = false;
    private bool verticalUp = false;

    private XROrigin rig;

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

        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool verticalDown);
        deviceRight.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool verticalUp);

        ////flyDirection = (flightHand.transform.position - head.transform.position);
        ////device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        ////transform.position += flyDirection.normalized * triggerValue * speed * Time.deltaTime;

        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
        //{
        //    flyDirection = (flightHand.transform.position - head.transform.position);
        //    device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        //    transform.position += flyDirection.normalized * triggerValue * speed * Time.deltaTime;
        //}
    }

    private void FixedUpdate()
    {
        // Horizontal
        Quaternion headYaw = Quaternion.Euler(0, rig.Camera.gameObject.transform.position.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0f, inputAxis.y);

        character.Move(direction * Time.fixedDeltaTime * speed);

        // Vertical
        if (verticalDown)
        {
            character.Move(Vector3.up * -speed * Time.fixedDeltaTime);
        }
        if (verticalUp)
        {
            character.Move(Vector3.up * speed * Time.fixedDeltaTime);
        }

        // GRAVITY
        bool isGrounded = CheckIfGrounded();
        if (isGrounded)
        {
            fallingSpeed = 0f;
        }
        else if (!isGrounded)
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
        }
        
        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.gameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height/2, capsuleCenter.z);
    }

    bool CheckIfGrounded()
    {
        // tells if grounded
        Vector3 rayStart = transform.TransformPoint(character.center);
        float raylength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, raylength, groundLayer);
        return hasHit;
    }
}
