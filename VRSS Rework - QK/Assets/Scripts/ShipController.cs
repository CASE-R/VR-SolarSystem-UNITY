using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float forwardSpeed = 5f, sideSpeed = 3.5f, hoverSpeed = 3f;
    private float activeForwardSpeed, activeSideSpeed, activeHoverSpeed;
    private float forwardAccel = 2.5f, sideAccel = 2f, hoverAccel = 2f;

    public float lookRotateSpeed = 90f;
    private Vector2 lookInput, screenCenter, mouseDistance;

    private float rollInput;
    public float rollSpeed = 90f, rollAccel = 3.5f;


    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width / 2;
        screenCenter.y = Screen.height / 2;

        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x)/screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y)/screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxis("Roll"), rollAccel * Time.deltaTime);
        
        gameObject.transform.Rotate(-mouseDistance.y * lookRotateSpeed * Time.deltaTime, mouseDistance.x * lookRotateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);
        
        // Take keyboard input for movement
        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAccel*Time.deltaTime);
        activeSideSpeed = Mathf.Lerp(activeSideSpeed, Input.GetAxisRaw("Horizontal") * sideSpeed, sideAccel*Time.deltaTime);
        activeHoverSpeed = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAccel*Time.deltaTime);

        // Apply input as transform
        transform.position = transform.position + new Vector3(activeSideSpeed, activeHoverSpeed, activeForwardSpeed) * Time.deltaTime;
    }
}
