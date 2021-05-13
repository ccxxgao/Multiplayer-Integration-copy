using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // public GameObject xrrig;
    public GameObject cam;
    private GameObject spawnPoint;
    private GameObject particles;
    private GameObject stand;

    public GameObject brain;
    public GameObject collisionBrain;
    public GameObject menu;
    public GameObject image;

    public GameObject laser;
    
    private Vector3 playerPosition;

    private int PlayerID;

    public GameObject images;

    public Text consoleText;

    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
        menu.GetComponent<Canvas>().enabled = false;
        images.GetComponent<Canvas>().enabled = false;
    }

    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Server...");
        // text.GetComponentInChildren<Text>().text = "Connecting to Server...";
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server.");
        // text.GetComponentInChildren<Text>().text = "CONNECTED TO ROOM";
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions(); 
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        
        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();

        PlayerID = Globals.convert(PhotonNetwork.LocalPlayer.ActorNumber); // % 4) + 1;

        var station = "Station " + PlayerID.ToString();

        Debug.Log("YOUR PLAYER ID: " + PhotonNetwork.LocalPlayer.ActorNumber + ", YOUR STATION: " + station);

        // Initialize user stuff
        spawnPoint = GameObject.Find(station);
        if (spawnPoint == null) {
            Debug.Log("Yikes");
        }
        playerPosition = spawnPoint.transform.position;
        UpdateRigLocation();
        UpdateParticles();
        OrientUI();
        GenerateBrain();
        // initializeCurrentUserData();

        // foreach (Player player in PhotonNetwork.PlayerList) {
        // // for (int i = 1; i < 5; i++){
        //     // int num = player.ActorNumber % 4;
        //     if (Globals.convert.player.ActorNumber != PlayerID) {
        //         // Debug.Log("HERE HERE HERE in MANAGER" + num);
        //         // Debug.Log("HERE" + station);
        //         // UpdateParticleColor(player.ActorNumber, true);
        //     }
        // }
        // CreateWorkStation();

        // foreach (Player player in PhotonNetwork.PlayerList) {
        //     int num = player.ActorNumber % 4;
        //     Debug.Log("HERE" + station);
        //     UpdateParticleColor(num, true);
        // }
    }

    private void UpdateParticleColor(int num, bool status)   // 0 = left, 1 = joined
    {
        var station = "Station " + num.ToString();
        Debug.Log("NETWORK PLAYER ID: " + num + ", NETWORK PLAYER'S STATION: " + station);
        Debug.Log("UPDATING PARTICLE COLOR FOR STATION " + num);

        // Initialize user stuff
        spawnPoint = GameObject.Find(station);
        if (spawnPoint == null) {
            Debug.Log("Yikes");
        }

        var particles = spawnPoint.transform.Find("Particle System").gameObject;
        if (particles == null) {
            Debug.Log("Did not find particle System");
        }
        var ps = particles.GetComponent<ParticleSystem>().main;
        ps.startColor = Color.blue;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room.");
        base.OnPlayerEnteredRoom(newPlayer);
        // this.addPlayerToDropdown(newPlayer.ActorNumber);
    }

    // private void addPlayerToDropdown(int newPlayerID) {
    //     dropdown.options.Add(new Dropdown.OptionData(newPlayerID.ToString()));
    // }

    private void UpdateRigLocation()
    {
        // Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);
        var floor = spawnPoint.transform.Find("Plane").gameObject;
        cam.transform.position = floor.transform.position;
        
        cam.transform.LookAt(Vector3.zero);
        Debug.Log(cam.transform.position);
    }

    private void UpdateParticles()
    {
        particles = spawnPoint.transform.Find("Particle System").gameObject;
        if (particles == null) {
            Debug.Log("Did not find particle System");
        }
        particles.SetActive(false);
    }

    private void OrientUI() {
        var factor = PlayerID - 1;
        menu.transform.RotateAround(Vector3.zero, Vector3.up, 90*factor);
        images.transform.RotateAround(Vector3.zero, Vector3.up, 90*factor);
        menu.GetComponent<Canvas>().enabled = true;
        images.GetComponent<Canvas>().enabled = true;
    }

    private void GenerateBrain() {
        stand = spawnPoint.transform.Find("Stand").gameObject;
        if (stand == null) {
            Debug.Log("Did not find your stand");
        }
        var standPosition = stand.transform.position;
        var transformedPosition = new Vector3(stand.transform.position.x, 
                                              stand.transform.position.y+0.6f, 
                                              stand.transform.position.z);
        
        Vector3 scale = new Vector3(0.0014f, 0.0014f, 0.0014f);
        // brain.SetActive(false);

        var createdBrain = Instantiate(brain, transformedPosition, Quaternion.identity);
        RotationManager rotation_script = createdBrain.GetComponent<RotationManager>();

        Slider slider = (Slider)FindObjectOfType(typeof(Slider));

        rotation_script.slider = slider;
        rotation_script.text = consoleText;
        if (slider == null){
            consoleText.text += "SLIDER COMPONENT NOT GOTTEN F -- FORWARD NOT SET";
        }
        // Debug.Log(rotation_script.sliderForward);
        createdBrain.SetActive(true);
        createdBrain.transform.localScale = scale;
        createdBrain.transform.rotation = Quaternion.Euler(new Vector3(-90f,90*PlayerID,0));

        // SIMPLIFIED MESH
        // collisionBrain.SetActive(false);

        var brain_for_collision = Instantiate(collisionBrain, transformedPosition, Quaternion.identity);
        RotationManager collision_rotation_script = brain_for_collision.GetComponent<RotationManager>();

        collision_rotation_script.slider = slider;
        collision_rotation_script.text = consoleText;

        if (slider == null){
            consoleText.text += "SLIDER COMPONENT NOT GOTTEN F -- FORWARD NOT SET";
        }
        // Debug.Log(rotation_script.slider);
        brain_for_collision.SetActive(true);

        brain_for_collision.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
        brain_for_collision.transform.rotation = Quaternion.Euler(new Vector3(-90f,90*PlayerID,0));

    }

}
