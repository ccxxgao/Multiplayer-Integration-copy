using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

// public class User {
//     public float userID;
// }


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

    // public Button text;
    
    private Vector3 playerPosition;

    private int PlayerID;

    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
        menu.GetComponent<Canvas>().enabled = false;
        // menu.SetActive(false);
        // xrrig = GameObject.Find("XR Rig");
        // spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
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

        PlayerID = (PhotonNetwork.LocalPlayer.ActorNumber % 4);

        var station = "Station " + PlayerID.ToString();

        Debug.Log("YOUR PLAYER ID: " + PlayerID + ", YOUR STATION: " + station);

        // Initialize user stuff
        spawnPoint = GameObject.Find(station);
        if (spawnPoint == null) {
            Debug.Log("Yikes");
        }
        playerPosition = spawnPoint.transform.position;
        UpdateRigLocation();
        UpdateParticles();
        generateBrain();
        generateMenu();
        // initializeCurrentUserData();

        foreach (Player player in PhotonNetwork.PlayerList) {
        // for (int i = 1; i < 5; i++){
            int num = player.ActorNumber % 4;
            if (num != PlayerID) {
                Debug.Log("HERE HERE HERE in MANAGER" + num);
                // Debug.Log("HERE" + station);
                // UpdateParticleColor(num, true);
            }
        }
        // CreateWorkStation();

        // foreach (Player player in PhotonNetwork.PlayerList) {
        //     int num = player.ActorNumber % 4;
        //     Debug.Log("HERE" + station);
        //     UpdateParticleColor(num, true);
        // }

            // Debug.Log(player.ActorNumber);
    }

    // private void UpdateParticleColor(int num, bool status)   // 0 = left, 1 = joined
    // {
    //     var station = "Station " + num.ToString();
    //     Debug.Log("NETWORK PLAYER ID: " + num + ", NETWORK PLAYER'S STATION: " + station);
    //     Debug.Log("UPDATING PARTICLE COLOR FOR STATION " + num);

    //     // Initialize user stuff
    //     spawnPoint = GameObject.Find(station);
    //     if (spawnPoint == null) {
    //         Debug.Log("Yikes");
    //     }

    //     var particles = spawnPoint.transform.Find("Particle System").gameObject;
    //     if (particles == null) {
    //         Debug.Log("Did not find particle System");
    //     }
    //     var ps = particles.GetComponent<ParticleSystem>().main;
    //     ps.startColor = Color.blue;
    // }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room.");
        base.OnPlayerEnteredRoom(newPlayer);
    }

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

    private void generateBrain()
    {
        // GENERATE EDITING INTERFACE
        // var controls = Instantiate(menu);
        var factor = PlayerID - 1;
        // // // controls.transform.rotation = rotation;
        menu.transform.RotateAround(new Vector3(0,0,0), Vector3.up, 90*factor);
        menu.GetComponent<Canvas>().enabled = true;
        // menu.renderer.enabled = true;
        // menu.SetActive(true);
        // LaserPointer pointer = (LaserPointer)FindObjectOfType(typeof(LaserPointer));
        // OVRRaycaster pointer_script = (OVRRaycaster) menu.GetComponent<OVRRaycaster>();  
        // pointer_script.pointer = laser;

        // var images = Instantiate(image);
        // images.transform.RotateAround(new Vector3(0,0,0), Vector3.up, 90*factor);

        // GENERATE BRAIN
        stand = spawnPoint.transform.Find("Stand").gameObject;
        if (stand == null) {
            Debug.Log("Did not find Stand");
        }
        var standPosition = stand.transform.position;
        var transformedPosition = new Vector3(stand.transform.position.x, 
                                              stand.transform.position.y+0.6f, 
                                              stand.transform.position.z);
        
        Vector3 scale = new Vector3(0.0014f, 0.0014f, 0.0014f);
        brain.SetActive(false);
        // BrainRotation rotation_script = 
        // rotation_script.sliderForward = controls.GetComponent<Slider>();
        // rotation_script.setSlider(controls.GetComponent<Slider>());

        var createdBrain = Instantiate(brain, transformedPosition, Quaternion.identity);
        BrainRotation rotation_script = createdBrain.GetComponent<BrainRotation>();
        // if (rotation_script == null){
        //     Debug.Log("ROTATION SCRIPT NOT FOUND");
        // }
        // Debug.Log(rotation_script.testVar);
        Slider slider = (Slider)FindObjectOfType(typeof(Slider));

        rotation_script.sliderForward = slider;
        if (slider == null){
            Debug.Log("SLIDER COMPONENT NOT GOTTEN F -- FORWARD NOT SET");
        }
        Debug.Log(rotation_script.sliderForward);
        // rotation_script.setSlider(controls.GetComponent<Slider>());
        // createdBrain.GetComponent<BrainRotation>().sliderForward = controls.GetComponent<Slider>();
        
        // var rotationScript = GameObject.Find("Brain Rotation");
        createdBrain.SetActive(true);

        createdBrain.transform.localScale = scale;
        createdBrain.transform.rotation = Quaternion.Euler(new Vector3(-90f,90*PlayerID,0));


        // SIMPLIFIED MESH
        collisionBrain.SetActive(false);

        var brain_for_collision = Instantiate(collisionBrain, transformedPosition, Quaternion.identity);
        rotation_script = brain_for_collision.GetComponent<BrainRotation>();

        rotation_script.sliderForward = slider;
        if (slider == null){
            Debug.Log("SLIDER COMPONENT NOT GOTTEN F -- FORWARD NOT SET");
        }
        Debug.Log(rotation_script.sliderForward);
        brain_for_collision.SetActive(true);

        brain_for_collision.transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
        brain_for_collision.transform.rotation = Quaternion.Euler(new Vector3(-90f,90*PlayerID,0));

    }

    // private v

    private void generateMenu()
    {
        // var rotation = Quaternion.Euler(new Vector3(0,90f,0));
        // var controls = Instantiate(menu);
        var factor = PlayerID - 1;
        // // controls.transform.rotation = rotation;
        // controls.transform.RotateAround(new Vector3(0,0,0), Vector3.up, 90*factor);

        var images = Instantiate(image);
        images.transform.RotateAround(new Vector3(0,0,0), Vector3.up, 90*factor);
        // images.transform.rotation = rotation;
    }

    // private void CreateWorkStation()
    // {
    //     // InputTracking.GetLocalPosition(VRNode.Head);
    //     cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //     cube.transform.localScale = new Vector3 (0.25f, 0.7f, 0.25f);

    //     // Get the non-zero horizontal coordinate (z or x) and move closer to center
    //     if (playerPosition.x != 0.0f) {
    //         var x = (Mathf.Abs(playerPosition.x) - 0.3f)*Mathf.Sign(playerPosition.x);
    //         cube.transform.position = new Vector3(x,0.35f,0);
    //     } else {
    //         var z = (Mathf.Abs(playerPosition.z) - 0.3f)*Mathf.Sign(playerPosition.z);
    //         cube.transform.position = new Vector3(0,0.35f,z);
    //     }
    // }

    // private void initializeCurrentUserData()
    // {
    //     Dictionary<string, string> dataToShare = new Dictionary<string, string>();
    //     dataToShare.Add("BrainRotation", 21);
    //     photonView.RPC("InitializeMyPlayerRPC",
    //                     PhotonTargets.OthersBuffered,
    //                     dataToShare as object
    //                     );

    //     [PunRPC]

    //     void InitializeMyPlayerRPC(object dataToShare)
    //     {
    //         Dictionary<string, string> data = dataToShare as Dictionary<string, string>;          
    //         player.Name = data["BrainRotation"].ToString();
    //     }
    // }
}
