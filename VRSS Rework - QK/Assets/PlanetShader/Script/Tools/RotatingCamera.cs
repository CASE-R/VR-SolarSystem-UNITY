using UnityEngine;
using System.Collections;

public class RotatingCamera : MonoBehaviour {
	
	public float		speed = 1.0f;

	void Update()
	{
		transform.RotateAround(Vector3.zero, Vector3.up, speed * Time.deltaTime);
	}
}