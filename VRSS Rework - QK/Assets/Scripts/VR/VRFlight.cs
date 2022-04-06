using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class VRFlight : MonoBehaviour
{
    public GameObject head;
    public GameObject flightHand; // Choosing this to be right controller

    private float flySpeed = 0.8f;
    private bool isFlying = false;

	public bool showController = false;
	public InputDeviceCharacteristics controllerCharacteristics;
	public List<GameObject> controllerPrefabs;

	private InputDevice targetDevice;
	public InputDevice rightHand;

	// Start is called before the first frame update
	void Start()
	{
		TryInitialize();
	}

	void TryInitialize()
	{
		List<InputDevice> devices = new List<InputDevice>();

		InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

		foreach (var item in devices)
		{
			Debug.Log(item.name + item.characteristics);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!targetDevice.isValid)
		{
			TryInitialize();
		}

		if (rightHand.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0f)
        {
			isFlying = true;
			Vector3 flyDirection = flightHand.transform.position - head.transform.position;
			transform.position += flyDirection.normalized * flySpeed * triggerValue * Time.deltaTime;
		}
		else if (triggerValue <= 0f)
        {
			isFlying=false;
        }

	}

}
