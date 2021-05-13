using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
// using System;


public class UIManager : MonoBehaviour 
{
	public Slider mainSlider;
    public Text degreeText;
	private float initValue;
	public Button syncButton;
	public Toggle toggle;
	public Text consoleText;


	private	Hashtable hash = new Hashtable();

    private Player localPlayer;


	private bool liveMode;


	public LaserPointer laser;
	private Vector3 previousLaserPosition;

	public GameObject sphere;

	private int playerID = -1;

	public Dropdown playerDropdown;	
	public Dropdown livePlayerDropdown;	


	private float previousSliderValue;

	private int liveModeActualID = -1;
	
	public void Start()
	{
		previousSliderValue = 0f;
		degreeText.text = ((int)(initValue*360f)).ToString() + "°";
		mainSlider.value = initValue;
		liveMode = false;
		sphere.SetActive(false);

		//Adds a listener to the main slider and invokes a method when the value changes.
		syncButton.onClick.AddListener(delegate { this.SendSyncRequest(); });
		toggle.onValueChanged.AddListener (delegate { this.InitiateLiveMode(); });

		// Network stuff
        hash.Add("Rotation", initValue);
		hash.Add("LaserPosition", Vector3.zero);
		hash.Add("LiveMode", false);
        // localPlayer.SetCustomProperties(hash);
	}
	
	private int checkRecipientPresence(int recipient) {
		int convertedRecipient = Globals.convert(recipient);
		// consoleText.text += "convertedRecipient:" + convertedRecipient.ToString() + "; playerID:" + playerID.ToString();
		if (convertedRecipient == playerID) {
			consoleText.text += "Error: Cannot sync with yourself.\n";
			return -1;
		}
		foreach (Player player in PhotonNetwork.PlayerList) {
			if (Globals.convert(player.ActorNumber) == convertedRecipient) {
				return player.ActorNumber;
			}
		}
		consoleText.text += "Error: Specified recipient does not exist in the room.\n";
		return -1;
	}

	private void getLocalPlayer () {
		localPlayer = PhotonNetwork.LocalPlayer;
		playerID = Globals.convert(localPlayer.ActorNumber);
		localPlayer.SetCustomProperties(hash);
	}

	void SendSyncRequest() {
		// if (localPlayer == null) {
		// 	getLocalPlayer();
		// }
		int recipient = playerDropdown.value+1;
		int actualRecipientID = checkRecipientPresence(recipient);
		if (actualRecipientID > -1) {
			object[] content = new object[] { "Requesting Annotation" }; // Array contains the target position and the IDs of the selected units
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] {actualRecipientID }};
			consoleText.text += "Sending request to Player " + actualRecipientID + "\n";
			PhotonNetwork.RaiseEvent(Globals.ANNOTATIONS_REQUEST, content, raiseEventOptions, SendOptions.SendReliable);
		}
	}

	private void InitiateLiveMode() {
		if (localPlayer == null) {
			getLocalPlayer();
		}
		// liveMode = false;

		int targetLiveID = livePlayerDropdown.value+1;

		if (targetLiveID == playerID) {
			// consoleText.text += "Error: cannot enter Live Mode with yourself";
			toggle.isOn = false;
			liveMode = false;
			return;
		}

		foreach (Player player in PhotonNetwork.PlayerList) {
			int convertedID = Globals.convert(player.ActorNumber);
			
			if (convertedID == targetLiveID) {
				liveModeActualID = player.ActorNumber;
				// if ((bool) player.CustomProperties["LiveMode"] == true) {
				// 	consoleText.text += "Error: cannot enter Live Mode (another player in currently in Live Mode)";
				// 	toggle.isOn = false;
				// 	liveMode = false;
				// 	return;
				// } else {
				// 	liveModeActualID = player.ActorNumber;
				// 	break;
				// }
			}
		}
		if (toggle.isOn == true) {
			consoleText.text += "\nLive Mode Enabled";
		} else {
			consoleText.text += "\nLive Mode Disabled";
		}

		liveMode = toggle.isOn;

		// liveMode = toggle.isOn;
		sphere.SetActive(liveMode);
		hash["LiveMode"] = liveMode;
		localPlayer.SetCustomProperties(hash);

	}

	void Update() {
		if (localPlayer == null) {
			getLocalPlayer();
		}

		// Check and update slider degree text
		if (previousSliderValue != mainSlider.value) {
			float currValue = mainSlider.value;
			var degree = (int) (currValue*360.0f);
			degreeText.text = degree.ToString() + "°";
			hash["Rotation"] = currValue;
			localPlayer.SetCustomProperties(hash);
			previousSliderValue = currValue;
		}

		// Store laser position
		if (laser._hitTarget) {
			// consoleText.text += "\n" + laser._endPoint;
			hash["LaserPosition"] = laser._endPoint;
        	localPlayer.SetCustomProperties(hash);
		}

		if (!liveMode || liveModeActualID == -1 || liveModeActualID == playerID) 
			return;

		if (liveMode) {
			Player player = PhotonNetwork.CurrentRoom.GetPlayer(liveModeActualID);
			if (player == localPlayer)
				return;

			mainSlider.value = (float) player.CustomProperties["Rotation"];
			// consoleText.text += player.CustomProperties["Rotation"].ToString();
			Vector3 pointerPosition = (Vector3) player.CustomProperties["LaserPosition"];
			// consoleText.text += pointerPosition.ToString();
			sphere.transform.position = pointerPosition;
			sphere.transform.RotateAround(Vector3.zero, Vector3.up, 90*(liveModeActualID - playerID));
		}
	}

	void SyncWithOtherUser() {
		foreach (Player player in PhotonNetwork.PlayerList) {
			if (this.localPlayer != player) {
				// Get Rotation
				mainSlider.value = (float) player.CustomProperties["Rotation"];
				return;
			}
		}
	}

	// Invoked when the value of the slider changes.
	public void ValueChangeCheck()
	{  
		float currValue = mainSlider.value;
        var degree = (int) (currValue*360.0f);
        degreeText.text = degree.ToString() + "°";
		// Debug.Log (mainSlider.value);
		// float delta = currValue - previousRotation;
        // brain.transform.Rotate(Vector3.up * delta * 360f);
		consoleText.text += degree.ToString() + "°";

        hash["Rotation"] = currValue;
        localPlayer.SetCustomProperties(hash);
	}
    // private void LoadState() {
    //     initValue = PlayerPrefs.GetFloat(storedState, 0.0f);
    // }
}
