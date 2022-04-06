using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRMovement : MonoBehaviour
{
    public XRNode inputSource;
    public float speed = 1f;
    public GameObject head;
    public GameObject flightHand;

    private XRRig rig;

    private Vector3 flyDirection;
    private CharacterController character;
    
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        flyDirection = (flightHand.transform.position - head.transform.position);
        device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        transform.position += flyDirection.normalized * triggerValue * speed * Time.deltaTime;
        //character.Move(flyDirection * Time.fixedDeltaTime * speed * triggerValue);
    }

    private void FixedUpdate()
    {
        //Quaternion headYaw = Quaternion.Euler(0, rig.Camera.transform.eulerAngles.y, 0);
        //Vector3 direction = new Vector3(inputAxis.x, inputAxis * headYaw.eulerAngles, inputAxis.y);

        
    }
}
