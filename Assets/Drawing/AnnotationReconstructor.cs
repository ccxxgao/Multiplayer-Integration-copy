using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;

public class AnnotationReconstructor : MonoBehaviour
{
    // UI Elements — User Input
    public Text text;
    public Button deleteAnnotationsButton;
    public Slider slider;

    // Annotation Elements
    public GameObject mesh;
    private Vector3[] vertices;
    private List<Annotation> annotationList;
    private List<GameObject> annotationObjectList;
    private List<GraphicLineRenderer> lines;

 
    // Statuses
    private int currentlyReconstructingIndex = 0; // Which index in the annotationList we are currently constructing
    private bool currentlyDrawing = false; // Are we currently in the process of drawing?
    private int numAnnotations = 0;

    // Network Info
    private int localPlayerID = -1;


    // Start is called before the first frame update
    void Start() {        
        annotationList = new List<Annotation>();
        annotationObjectList = new List<GameObject>();
        lines = new List<GraphicLineRenderer>();

        deleteAnnotationsButton.onClick.AddListener (delegate { this.deleteAllAnnotations(); });
    }

    void Update() {
        if (currentlyDrawing) {
            while (annotationList[currentlyReconstructingIndex].completed == true) {
                currentlyReconstructingIndex += 1;
                // text.text += "\n Currently reconstructing Annotation " + currentlyReconstructingIndex.ToString();
            }
            slider.value = annotationList[currentlyReconstructingIndex].brainRotation;

            var activeAnnotate = annotationList[currentlyReconstructingIndex];

            lines[currentlyReconstructingIndex].AddPoint(activeAnnotate.GetTransformedVertex());
            if (annotationList[currentlyReconstructingIndex].completed
                && currentlyReconstructingIndex==numAnnotations-1) {
                    currentlyDrawing = false;
            }
        }
    }

	public void OnEnable()
	{
		PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
	}

	public void OnDisable()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
	}	

    private void deleteAllAnnotations() {
        text.text += "\nDeleting all annotations";
        annotationList.Clear();
        foreach (GameObject annote in annotationObjectList) {
            Destroy(annote);
        }
        annotationObjectList.Clear();
        lines.Clear();
        
        currentlyReconstructingIndex = 0; // Which index in the annotationList we are currently constructing
        numAnnotations = 0;
    }

    public void OnEvent(EventData photonEvent)
	{
		// print("Event received");
		byte eventCode = photonEvent.Code;

		if (eventCode == Globals.TRANSFERRING_ANNOTATIONS)
		{
            object[] data = (object[])photonEvent.CustomData;
            if (data.Length == 1) {
                text.text += "\n" + (string) data[0];
                return;
            }

            // Parse data into Annotations struct
            int networkPlayerID = Globals.convert(photonEvent.Sender);

            // Rotate brain into sender's position
            // Player player = PhotonNetwork.CurrentRoom.GetPlayer(photonEvent.Sender);

            if (localPlayerID == -1) {
                localPlayerID = Globals.convert(PhotonNetwork.LocalPlayer.ActorNumber);
            }
            numAnnotations += 1;

            text.text += "\nReceived annotation " + numAnnotations.ToString() 
                            + " from Player " + photonEvent.Sender.ToString();

            Annotation annote = new Annotation(data, networkPlayerID, localPlayerID);
            annotationList.Add(annote);
            
            lines.Add(SetupAnnotationObject(annote.color));
            currentlyDrawing = true;
		}
	}

    private GraphicLineRenderer SetupAnnotationObject(Color color) {
        GameObject go = new GameObject();
        if (mesh == null) {
            var meshes = GameObject.FindGameObjectsWithTag("BrainMesh");
            if (meshes.Length > 0) {
                mesh = meshes[0];
                go.transform.parent = mesh.transform;
            }
        } else {
            go.transform.parent = mesh.transform;
        }
        
        go.AddComponent<MeshFilter>();
        var renderer = go.AddComponent<MeshRenderer>();
            renderer.receiveShadows = false;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        // Change line rendering stuff
        GraphicLineRenderer currLine = go.AddComponent<GraphicLineRenderer>();
        currLine.SetMaterial(Globals.createNewMaterial(color));
        currLine.SetWidth(0.01f);

        annotationObjectList.Add(go);

        return currLine;
        // text.text = "2 GOT HERE";
	}
}
