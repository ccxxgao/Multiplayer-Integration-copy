using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandMenu : MonoBehaviour
{
    public GameObject handMenu;

    public GameObject console;
    public Toggle consoleToggle;

    public GameObject colorPicker;
    public Button chooseColorButton;

    public Text text;


    // Start is called before the first frame update
    void Start()
    {
        handMenu.SetActive(false);
        colorPicker.SetActive(false);

        consoleToggle.onValueChanged.AddListener (delegate {this.ToggleConsole(); });
		chooseColorButton.onClick.AddListener(delegate {this.ToggleColorSelector(); });
    }

    // Update is called once per frame
    void Update()
    {
        // If colorPicker is active, hide colorPicker and set variable so that the next time
        // the start button is pressed, the main menu is displayed
        // ----------------------------------------------------------------------------------
        // If mainMenu is active, hide mainMenu and set variable so that the next time
        // the start button is pressed, the main menu is displayed
        if (OVRInput.GetUp(OVRInput.RawButton.Start)) {
            if (colorPicker.activeSelf || handMenu.activeSelf) {
                colorPicker.SetActive(false);
                handMenu.SetActive(false);
            } else {
                handMenu.SetActive(true);
                text.text += "\nMenu Toggled";
            }
        }
        // toggle back to main menu;
        else if (OVRInput.GetUp(OVRInput.RawButton.X)) {
            if (colorPicker.activeSelf) {
                colorPicker.SetActive(false);
                handMenu.SetActive(true);
            }
        }
    }

    void ToggleConsole() {
        console.SetActive(consoleToggle.isOn);
    }

    void ToggleColorSelector() {
        colorPicker.SetActive(true);
        handMenu.SetActive(false);
    }
}
