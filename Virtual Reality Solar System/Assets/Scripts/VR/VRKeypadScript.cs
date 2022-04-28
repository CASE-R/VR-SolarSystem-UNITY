using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VRKeypadScript : MonoBehaviour
{
    public GameObject system;
    VRCamSwitch camSwitch;

    public GameObject keypad;

    public InputField velocity;
    public InputField mass;
    public InputField radius;
    public InputField timeScale;

    public InputField activeInputField;

    public Toggle ShowUI;

    string[] characters;

    // Start is called before the first frame update
    void Start()
    {
        camSwitch = system.GetComponent<VRCamSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShowUI.isOn == true && camSwitch.celNumber != -1)
        {
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
            else if (timeScale.isFocused)
            {
                activeInputField=timeScale;
                keypad.SetActive(true);
            }

            if (keypad.activeInHierarchy && activeInputField != null)
            {
                activeInputField.ActivateInputField();
            }
        }

        else
        {
            keypad.SetActive(false);
        }
    }

    public void InputKeyPress(Button btn)
    {
        activeInputField.text = (activeInputField.text + btn.name);
        activeInputField.Select();
    }


    public void PressClose()
    {
        activeInputField.onEndEdit.Invoke(activeInputField.text);
        activeInputField.DeactivateInputField();
        activeInputField = null;
        keypad.SetActive(false);

    }

    public void PressDelete()
    {
        //characters = activeInputField.text.Split();
        activeInputField.text = activeInputField.text.Remove(activeInputField.text.Length - 1);
        if (activeInputField.text.Length == 0)
        {
            activeInputField.text = "0";
        }
    }
}
