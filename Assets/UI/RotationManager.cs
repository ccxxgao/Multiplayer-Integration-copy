using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationManager : MonoBehaviour
{
    public GameObject brain;
    public Slider slider;
    public Text text;
    // public Text degreeText;

    // Preserve the original and current orientation
    [SerializeField] private float previousValue;

    void Start()
    {
        slider.onValueChanged.AddListener(this.OnSliderChanged);
        previousValue = slider.value;
        // text.text += "\nStarting rotation " + previousValue.ToString();
    }
    
    private void OnSliderChanged(float currValue)
    {
        // How much we've changed
        float delta = currValue - previousValue;
        brain.transform.Rotate(Vector3.forward * delta * 360f);

        // Set our previous value for the next change
        previousValue = currValue;
        //  this.SaveState("BrainRotationForward", previousValue);
    }

    public void RotateObject(float currValue, float initValue)
    {
        // How much we've changed
        float delta = currValue - previousValue;
        brain.transform.Rotate(Vector3.forward * delta * 360f);

        // Set our previous value for the next change
        previousValue = currValue;
        //  this.SaveState("BrainRotationForward", previousValue);
    }

    // public void SaveState(string variableName, float variable) {
    //     PlayerPrefs.SetFloat(variableName, variable);
    //     PlayerPrefs.Save();
    // }
}
