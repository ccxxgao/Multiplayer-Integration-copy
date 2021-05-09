using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
// using Photon.Pun;

public class BrainRotation : MonoBehaviour 
{
	// public Slider mainSlider;

    // public GameObject brain;
	
	// public void Start()
	// {
	// 	//Adds a listener to the main slider and invokes a method when the value changes.
	// 	mainSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
	// }
	
	// // Invoked when the value of the slider changes.
	// public void ValueChangeCheck()
	// {  
    //     var degree = (int) (mainSlider.value*360.0f);
    //     // brain.transform.Rotate(new Vector3(0, 0, degree), Space.Self);
    //     brain.transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
    //     // Vector3.up * Time.deltaTime
    //     // degreeText.text = degree.ToString() + "°";
	// 	// Debug.Log (mainSlider.value);
	// }

     // Assign in the inspector
     public GameObject brain;
     public Slider sliderForward;

    //  public Button button;

    //  private PhotonNetwork.Player local = PhotonNetwork.LocalPlayer;
    //  public Slider sliderUp;
    //  public Text textValue;
 
     // Preserve the original and current orientation
     [SerializeField] private float previousValueForward;
    //  [SerializeField] private float previousValueUp;

    void Awake ()
     {
        //  this.LoadState();
        //  textValue.text = previousValueForward.ToString();
        //  Debug.Log("previousValueForward: " + previousValueForward);
         // Assign a callback for when this slider changes
         this.sliderForward.onValueChanged.AddListener(this.OnSliderForwardChanged);
         // And current value
         this.previousValueForward = this.sliderForward.value;

        // Assign a callback for when this slider changes
        //  this.sliderUp.onValueChanged.AddListener(this.OnSliderUpChanged);
        //  // And current value
        //  this.previousValueUp = this.sliderUp.value;

     }


    void OnSliderForwardChanged(float value)
     {
         // How much we've changed
         float delta = value - this.previousValueForward;
         this.brain.transform.Rotate(Vector3.forward * delta * 360);
 
         // Set our previous value for the next change
         this.previousValueForward = value;
         this.SaveState("BrainRotationForward", previousValueForward);

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

    public void SaveState(string variableName, float variable) {
        PlayerPrefs.SetFloat(variableName, variable);
        PlayerPrefs.Save();
        // Debug.Log(previousValueForward);
    }



}
