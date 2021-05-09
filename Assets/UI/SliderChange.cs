using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
// using System;


public class SliderChange : MonoBehaviour 
{
	public Slider mainSlider;

    public Text degreeText;

	public string storedState;

	private float initValue;

	public Button resetButton;

	private	Hashtable hash = new Hashtable();

    private Player localPlayer;

	public Toggle toggle;

	private bool liveMode;

	public Text consoleText;

	public LaserPointer laser;
	private Vector3 previousLaserPosition;

	public GameObject sphere;

	private int playerID;
	
    // public const byte AnnotationsRequest = 1;
	
	public void Start()
	{
		this.LoadState();
		localPlayer = PhotonNetwork.LocalPlayer;
		playerID = localPlayer.ActorNumber % 4;
		degreeText.text = ((int)(initValue*360f)).ToString() + "°";
		mainSlider.value = initValue;
		liveMode = false;
		sphere.SetActive(liveMode);
		// consoleText = resetButton.GetComponentInChildren<Text>();

		//Adds a listener to the main slider and invokes a method when the value changes.
		mainSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
		resetButton.onClick.AddListener(delegate {this.SendMessageToOtherUser (); });
		toggle.onValueChanged.AddListener (delegate {this.InitiateLiveMode (); });

		// Network stuff
        hash.Add("Rotation", initValue);
		hash.Add("LaserPosition", Vector3.zero);
		hash.Add("LiveMode", false);
        localPlayer.SetCustomProperties(hash);

		// Create a deactive sphere to activate when live mode is turned on
		// sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		// sphere.material = 
        // sphere.transform.position = new Vector3(0, 1.5f, 0);
	}


	public void OnEnable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
	}

	public void OnDisable()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
	}	

	void SendMessageToOtherUser() {
		object[] content = new object[] { "Hello" }; // Array contains the target position and the IDs of the selected units
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(Globals.ANNOTATIONS_REQUEST, content, raiseEventOptions, SendOptions.SendReliable);
	}

	public void OnEvent(EventData photonEvent)
	{
		byte eventCode = photonEvent.Code;

		if (eventCode == Globals.TEST_TEXT)
		{
			object[] data = (object[])photonEvent.CustomData;
			consoleText.text = (string) data[0];
		}
	}


	void InitiateLiveMode() {
		foreach (Player player in PhotonNetwork.PlayerList) {
			if (this.localPlayer != player) {
				if ((bool) player.CustomProperties["LiveMode"] == true) {
					// consoleText.text = "Another Player in Live Mode";
					toggle.isOn = false;
					return;
				}
			}
		}

		liveMode = toggle.isOn;
		sphere.SetActive(liveMode);
		consoleText.text = liveMode.ToString();
		hash["LiveMode"] = liveMode;
		localPlayer.SetCustomProperties(hash);
	}

	void Update() {
		// Store laser position
		// if (Vector3.Distance(other.position, ))
		if (laser._hitTarget) {
			hash["LaserPosition"] = laser._endPoint;
        	localPlayer.SetCustomProperties(hash);
		}

		if (!liveMode) return;
		foreach (Player player in PhotonNetwork.PlayerList) {
			if (this.localPlayer != player) {
				int networkPlayerID = player.ActorNumber % 4;
				mainSlider.value = (float) player.CustomProperties["Rotation"];
				Vector3 pointerPosition = (Vector3) player.CustomProperties["LaserPosition"];
				// consoleText.text = pointerPosition.ToString();
				sphere.transform.position = pointerPosition;
       		 	sphere.transform.RotateAround(Vector3.zero, Vector3.up, 90*(networkPlayerID - playerID));
				return;
			}
		}
	}

	void SyncWithOtherUser() {
		foreach (Player player in PhotonNetwork.PlayerList) {
			if (this.localPlayer != player) {
				// Get Rotation
				mainSlider.value = (float) player.CustomProperties["Rotation"];

				// Get Annotations
				// var meshVertices = DeserializeVector3Array((string) player.CustomProperties["Annotations"]);
				// Color color = (Color) player.CustomProperties["AnnotationsColor"];
				// regenerateMesh(meshVertices, color);
				return;
			}
		}
	}

	void ResetSlider () {
		if (mainSlider.value != 0.5f)
			mainSlider.value = 0.0f;
		if (mainSlider.value == 0f)
			mainSlider.value = 0.5f;
		// Text text = resetButton.GetComponent<Text>();
		// text.text = "Clicked";
		// resetButton.Text = "CLICKED";
		
	}
	
	// Invoked when the value of the slider changes.
	public void ValueChangeCheck()
	{  
        var degree = (int) (mainSlider.value*360.0f);
        degreeText.text = degree.ToString() + "°";
		// Debug.Log (mainSlider.value);

        hash["Rotation"] = mainSlider.value;
        localPlayer.SetCustomProperties(hash);
	}

    public void LoadState() {
        initValue = PlayerPrefs.GetFloat(storedState, 0.0f);
    }
}
