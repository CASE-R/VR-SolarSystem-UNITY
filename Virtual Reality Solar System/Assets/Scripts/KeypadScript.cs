using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeypadScript : MonoBehaviour
{
    public GameObject system;
    CameraFocus camFoc;

    public GameObject keypad;

    public InputField velocity;
    public InputField mass;
    public InputField radius;

    public InputField activeInputField;

    public Toggle ShowUI;

    string[] characters;

    // Start is called before the first frame update
    void Start()
    {
        camFoc = system.GetComponent<CameraFocus>();
    }

    // Update is called once per frame
    void Update()
    {
        //only activate the keypad if we're not in freeCam and UI is on
        if (ShowUI.isOn == true && camFoc.celNumber != -1)
        {

            //select an input field which is being typed into after clicking on it
            //we then only perform keypad inputs on activeInputField
            if (velocity.isFocused)
            {
                activeInputField = velocity;
                keypad.SetActive(true);
            }

            else if (mass.isFocused)
            {
                activeInputField = mass;
                keypad.SetActive(true);
            }
            else if (radius.isFocused)
            {
                activeInputField = radius;
                keypad.SetActive(true);
            }

            //since clicking on anything other than the input field will deactivate it:
            //we have to reactivate the input field whenever the keypad is still on
            if (keypad.active && activeInputField != null)
            {
                activeInputField.ActivateInputField();
            }
        }

        //if none of the input fields are focused on, we don't want the keypad to be visible
        else
        {
            keypad.SetActive(false);
        }
    }

    //whenever an input button is pressed (0-9 or .) run this function
    //in the inspector we attach the button itself onto the component
    //this way we have info on the button "btn" which is being pressed
    public void InputKeyPress(Button btn)
    {
        //all the buttons are named after their value they add to the input field
        //this allows for quick appending of the input field that we selected

        //in the special case the the input field is at 0, we want the keypad to behave differently
        if (activeInputField.text == "0")
        {
            //unless it's a decimal point being added, we want the 0 to be overriden by the digit pressed
            if (btn.name != ".")
            {
                activeInputField.text = btn.name;
            }

            //if it is a . being added, it should work as normal since we might want 0.56 etc.
            else
            {
                activeInputField.text = (activeInputField.text + btn.name);
            }    
        }

        //if the input field isn't 0, we want the digit pressed to be added to the end
        else
        {
            activeInputField.text = (activeInputField.text + btn.name);
        }
        //after clicking elsewhere, we again select the last activated input field
        activeInputField.Select();
    }


    public void PressClose()
    {
        //we run the function that gets called whenever an input field input is submitted
        //these could be: updating the mass, radius or velocity of the object
        activeInputField.onEndEdit.Invoke(activeInputField.text);

        //when we press close, we stop typing into the input field and deactivate the keypad
        //we also set active field to none so the keypad doesn't show up based on above if statements
        activeInputField.DeactivateInputField();
        activeInputField = null;
        keypad.SetActive(false);

    }

    public void PressDelete()
    {
        //we want to remove the last character ( (text.length -1) since the index starts at 0)
        activeInputField.text = activeInputField.text.Remove(activeInputField.text.Length - 1);
        //if the text length is 0 after removing the last character, we simply set the text to 0
        if(activeInputField.text.Length == 0)
        {
            activeInputField.text = "0";
        }
    }
}
