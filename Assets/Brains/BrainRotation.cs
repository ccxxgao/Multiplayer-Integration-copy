using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
// using Photon.Pun;

public class BrainRotation : MonoBehaviour 
{

     // Assign in the inspector
     public GameObject brain;
     public Slider slider;

     public Text text;

    //  public Button button;

    //  private PhotonNetwork.Player local = PhotonNetwork.LocalPlayer;
    //  public Slider sliderUp;
    //  public Text textValue;
 
     // Preserve the original and current orientation
     [SerializeField] private float previousValue;
    //  [SerializeField] private float previousValueUp;

    void Awake ()
    {
        slider.onValueChanged.AddListener(this.OnSliderChanged);
        previousValue = slider.value;
     }

    // void Update() {
    //     text.text += "previousValue: " + previousValue;
    // }

    void OnSliderChanged(float value)
     {
         // How much we've changed
         text.text += "\nChanging rotation value to " + value;
         float delta = value - previousValue;
         brain.transform.Rotate(Vector3.forward * delta * 360f);
 
         // Set our previous value for the next change
         previousValue = value;
        //  text.text =
        //  this.SaveState("BrainRotationForward", previousValue);

        // Hashtable hash = new Hashtable();
        // hash.Add("Rotation", previousValueForward);
        // local.SetCustomProperties(hash);
     }

    //  void OnSliderUpChanged(float value)
    //  {
    //      // How much we've changed
    //      float delta = value - this.previousValueUp;
    //      this.brain.transform.Rotate(Vector3.up * delta * 360);
 
    //      // Set our previous value for the next change
    //      this.previousValueUp = value;
    //      this.SaveState("BrainRotationUp", previousValueUp);
    //  }

    // public void SaveState(string variableName, float variable) {
    //     PlayerPrefs.SetFloat(variableName, variable);
    //     PlayerPrefs.Save();
    //     // Debug.Log(previousValueForward);
    // }
}
