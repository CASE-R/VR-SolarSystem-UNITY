using UnityEngine;
using System.Collections;

public class FreeCamera : MonoBehaviour 
{
	public float	flySpeed = 1.0f;
	public Vector2	flySpeedLimit = new Vector2(0, 10);
	public float 	rotationSpeed = 120.0f;
	public Vector2 	sensitivity = new Vector2(2, 2);
	public Vector2 	smoothing = new Vector2(2, 2);

	private Vector2	mouseDelta;
	private Vector2 mouseAbsolute;
	private Vector2 smoothMouse;
	
	void Update ()
	{
		// Camera Mouse rotation (XY)

		mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));
		smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1.0f / smoothing.x);
		smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1.0f / smoothing.y);
		mouseAbsolute = smoothMouse;

		transform.Rotate(-Vector3.right * mouseAbsolute.y * Time.deltaTime);
		transform.Rotate(Vector3.up * mouseAbsolute.x * Time.deltaTime);

		// Camera Keyboard rotation (Z)
		if (Input.GetKey(KeyCode.Q))
			transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
		else if (Input.GetKey(KeyCode.E))
			transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);

		// Camera keyboard movement

		if (Input.GetAxis("Mouse ScrollWheel") != 0)
			flySpeed = Mathf.Clamp(flySpeed + ((flySpeedLimit.y - flySpeedLimit.x) / 10.0f) * Input.GetAxis("Mouse ScrollWheel"), flySpeedLimit.x, flySpeedLimit.y);

		if (Input.GetAxis("Vertical") != 0)
			transform.Translate(transform.forward * flySpeed * Input.GetAxis ("Vertical"), Space.World);
		if (Input.GetAxis("Horizontal") != 0)
			transform.Translate(transform.right * flySpeed * Input.GetAxis ("Horizontal"), Space.World);
	}
}
