using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : MonoBehaviour {

    //======== Key Codes ===========================================================================

    //// Buttons
    //private KeyCode a = KeyCode.JoystickButton0;
    //private KeyCode b = KeyCode.JoystickButton1;
    //private KeyCode x = KeyCode.JoystickButton2;
    //private KeyCode y = KeyCode.JoystickButton3;

    //// Bumpers
    //private KeyCode leftBumper = KeyCode.JoystickButton4;
    //private KeyCode rightBumper = KeyCode.JoystickButton5;

    //// Menu buttons
    //private KeyCode back = KeyCode.JoystickButton6;
    //private KeyCode start = KeyCode.JoystickButton7;

    //// Left Stick
    //private string leftX = "L-Horizontal";
    //private string leftY = "L-Vertical";
    //private KeyCode leftStickButton = KeyCode.JoystickButton8;

    //// Right Stick
    //private string rightX = "R-Horizontal";
    //private string rightY = "R-Vertical";
    //private KeyCode rightStickButton = KeyCode.JoystickButton9;

    //// Triggers
    //private string leftTrigger = "Left Trigger";
    //private string rightTrigger = "Right Trigger";
    ////private string sharedTrigger = "";

    //// DPAD
    //private string dpadX = "D-Horizontal";
    //private string dpadY = "D-Vertical";

    //=========================================================================================================

    // Use this for initialization
    void Start()
    {

    }


    void Update()
    {


        if (Input.GetButtonDown("Jump"))
        {
            EventManager.TriggerEvent("jump");
            Debug.Log("Jump");
        }


    }
}
