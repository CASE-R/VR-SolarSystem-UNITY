/*
For Oculus Touch and Quest controllers.
This code is a modified version of the wonderful Flight Navigation script by
AncientC https://ancientc.com/ac/ concept by Kevin Mack http://www.kevinmackart.com/
Unity asset store: "Flight Navigation for HTC Vive controller" 
https://assetstore.unity.com/packages/tools/flight-navigation-for-htc-vive-controller-61830
Note: there may be some unnecessary code bits (like "Show Thrust Mockup") that are not needed,
but I left them in, just incase they are of use to you. Also, I don't know what the "FixedJoint"
does, so left it in as well. Hey I'm an artist not a developer! What the heck do I know?
Someone smart can probably figure out how to add a funtion to check if the user is on a Vive or 
Oculus and make one script. (please share if you do!)
-scobot 2019
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFlight : MonoBehaviour
{
    [SerializeField]
    private OVRInput.Controller m_controller;
    [SerializeField]
    private Animator m_animator = null;

    public Rigidbody NaviBase;
    public Vector3 ThrustDirection;
    public float ThrustForce;
    public bool ShowTrustMockup = true;
    public GameObject ThrustMockup;

    FixedJoint joint;
    GameObject attachedObject;
    Vector3 tempVector;

    void FixedUpdate()
    {
        // add force
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, m_controller))
        {
            tempVector = Quaternion.Euler(ThrustDirection) * Vector3.forward;
            NaviBase.AddForce(transform.rotation * tempVector * ThrustForce);
            NaviBase.maxAngularVelocity = 2f;

            m_animator.SetFloat("Trigger", OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, m_controller));
        }

        // show trust mockup
        if (ShowTrustMockup && ThrustMockup != null)
        {

            if (attachedObject == null && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, m_controller))
            {
                attachedObject = (GameObject)GameObject.Instantiate(ThrustMockup, Vector3.zero, Quaternion.identity);
                attachedObject.transform.SetParent(this.transform, false);
                attachedObject.transform.Rotate(ThrustDirection);
            }

            else if (attachedObject != null && OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, m_controller))
            {
                Destroy(attachedObject);
            }
        }
    }
}