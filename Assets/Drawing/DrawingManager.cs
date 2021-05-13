using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using System.Security.Cryptography;

public class DrawingManager : MonoBehaviour
{
    // UI Elements — User Input
    public Text text;
    public LaserPointer laser;
    public FlexibleColorPicker colorPicker;
    public Button syncAllButton;
    public Button deleteAnnotationsButton;
    public Slider slider;



    // Annotation Elements
    private GraphicLineRenderer currLine;
    public GameObject mesh;
    private List<Annotation> annotationList;
    private bool drawingMode = false;
    private List<GameObject> lines;



    // Counters
    private int numClicks = 0;
    private int counter = -1;

    void Start() {

        annotationList = new List<Annotation>();
        lines = new List<GameObject>();
        counter = -1;

        syncAllButton.onClick.AddListener (delegate { this.SendAnnotationsToAll(); });
        deleteAnnotationsButton.onClick.AddListener (delegate { this.deleteAllAnnotations(); });

    }

    //////////////////////////////////////////////////////////////////////////////
    // CONTROLLER SETTINGS FOR DRAWING                                          //
    // Index Trigger --> End current annotation and beginn new annotation       //
    // Hand Trigger  --> Draw on surfaces                                       //
    //////////////////////////////////////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger) && drawingMode==false) {
            // text.text += "You are Player " + PhotonNetwork.LocalPlayer.ActorNumber.ToString();

            // Setup new annotation
            counter += 1;
            drawingMode = true;
            text.text += "\nEntered drawing mode, annotation " + counter.ToString();
            Color color = colorPicker.color;
            annotationList.Add(new Annotation(color, slider.value));
            GameObject annotation = SetupAnnotationObject(color);
            lines.Add(annotation);

            // Reset numClicks
            numClicks = 0;
        } else if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger) && drawingMode==true) {
            drawingMode = false;
            text.text += "\nExited drawing mode";
        } 
        else if (laser._hitTarget && OVRInput.Get(OVRInput.RawButton.RIndexTrigger) && drawingMode==true) {
            currLine.AddPoint(laser._endPoint);
            annotationList[counter].AddVertex(laser._endPoint);
            numClicks++;
        }
    }

    // Create new GameObject for annotations
    private GameObject SetupAnnotationObject(Color color) {
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
        currLine = go.AddComponent<GraphicLineRenderer>();
        currLine.SetMaterial(Globals.createNewMaterial(color));
        currLine.SetWidth(0.01f);

        return go;
    }

    private void deleteAllAnnotations() {
        text.text += "\nDeleting all annotations";
        annotationList.Clear();
        foreach (GameObject annote in lines) {
            Destroy(annote);
        }
        lines.Clear();
        counter = -1;
    }


    // Handle Network Requests
	public void OnEnable() {
		PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
	}
	public void OnDisable() {
		PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
	}	

    public void OnEvent(EventData photonEvent) {
		byte eventCode = photonEvent.Code;

		if (eventCode == Globals.ANNOTATIONS_REQUEST && counter > -1)
		{
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { TargetActors = new int[] {photonEvent.Sender}};
            text.text += "\nPlayer " + photonEvent.Sender + " requests your annotations"; // "WANT ANNOTATIONS";
            if (counter == -1) {
                SendAnnotations(-1, raiseEventOptions);
                return;
            }
            for (int i = 0; i <= counter; i++) {
                text.text += "\nSending annotation " + i.ToString() 
                            + " to Player " + Globals.convert(photonEvent.Sender).ToString();
                SendAnnotations(i, raiseEventOptions);
            }
		}
	}

    void SendAnnotations(int index, RaiseEventOptions raiseEventOptions) {
        object[] content;
        // If there are no annotations, send message that there are no annotations to update
        if (index == -1) {
            content = new object[] {"No annotation to send"};
        } else {
            Annotation annote = annotationList[index];
            content = annote.ConvertToObject();
            if (content == null) return;
        }
        PhotonNetwork.RaiseEvent(Globals.TRANSFERRING_ANNOTATIONS, content, raiseEventOptions, SendOptions.SendReliable);
	}

    void SendAnnotationsToAll() {
        if (counter == -1) {
            text.text += "\nYou have no annotations to send";
            return;
        }

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        for (int i = 0; i <= counter; i++) {
            text.text += "\nSending annotation " + i.ToString() + "to all";
            SendAnnotations(i, raiseEventOptions);
        }
    }
}
