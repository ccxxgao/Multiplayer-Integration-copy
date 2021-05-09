using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////////////////////////////////////////
// CONTROLLER SETTINGS FOR TOGGLING MENU                                    //
// Left Start Button --> Toggle Menu                                         //
//////////////////////////////////////////////////////////////////////////////

public class MenuManager : MonoBehaviour
{
    public GameObject colorPicker;

    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        colorPicker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.GetUp(OVRInput.RawButton.Start)) {
        // ;
        // if (OVRInput.Get(OVRInput.Button.Start)) {
            colorPicker.SetActive(!colorPicker.activeSelf);
            //  = !colorPicker.enabled;
            text.text += "Menu Toggled";
        }
        // OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);
        
    }
}
