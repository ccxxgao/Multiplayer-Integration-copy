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


public class ReconstructAnnotations : MonoBehaviour
{
    // UI Elements — User Input
    // public Button button;
    public Text text;
    

    // Annotation Elements
    public GameObject mesh;
    // private GraphicsLineRenderer currLine;
    private Vector3[] vertices;
    private List<Annotation> annotationList;
    // private Annotation activeAnnote;
    private List<GraphicsLineRenderer> lines;

 
    // Statuses
    private int currentlyReconstructingIndex = -1; // Which index in the annotationList we are currently constructing
    private bool currentlyDrawing = false; // Are we currently in the process of drawing?
    private int numAnnotations = -1;

    // Network Info
    private int localPlayerID;
    private int networkPlayerID;


    // Start is called before the first frame update
    void Start() {
        // text = button.GetComponentInChildren<Text>();

        localPlayerID = PhotonNetwork.LocalPlayer.ActorNumber % 4;
        
        annotationList = new List<Annotation>();

        lines = new List<GraphicsLineRenderer>();
    }

    void Update() {
        if (currentlyReconstructingIndex == -1){
            // text.text = "here";
            return;
        } else if (currentlyReconstructingIndex > -1 && currentlyDrawing) { // && vertices!=null && reconstructionIndex!=-1){
            // if (currentlyReconstructingIndex > 0) {
            //     while (currentlyReconstructingIndex > 0) {
            //         if (!annotationList[currentlyReconstructingIndex-1].completed) {
            //             currentlyReconstructingIndex -= 1;
            //         }
            //     }
            // }
            var activeAnnotate = annotationList[currentlyReconstructingIndex];
            
            text.text += "\n" + "\n" + currentlyReconstructingIndex.ToString();
            // If currently drawing, update
            // transform point
            // Vector3 transformedPoint = RotatePointAroundPivot(vertices[reconstructionIndex], 
            //                             new Vector3(0, 90*(networkPlayerID - playerID), 0));
            lines[currentlyReconstructingIndex].AddPoint(activeAnnotate.GetTransformedVertex());
            // text.text = vertices[currentlyReconstructingIndex].ToString();
            // reconstructionIndex++;
            if (annotationList[currentlyReconstructingIndex].completed) {
                currentlyReconstructingIndex+=1;
                if (currentlyReconstructingIndex == numAnnotations) {
                    currentlyDrawing = false;
                }
            }
            // if (activeAnnote.iterator == activeAnnote.numVertices) {
            //     // We are done drawing
            //     if (currentlyReconstructingIndex+1 == annotationList.Count) {
            //         currentlyDrawing = false;
            //     } 
            //     // Draw the next annotation
            //     else {
            //         currentlyReconstructingIndex += 1;
            //     }
            //     // mesh.transform.RotateAround(Vector3.zero, Vector3.up, 90*(networkPlayerID - playerID));
            //     // currentlyReconstructingIndex++;
            //     // reconstructionIndex=-1;
            // }
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
            networkPlayerID = photonEvent.Sender % 4;

            // text.text = "S:" + networkPlayerID.ToString() + "R:" + localPlayerID.ToString()
            //             + "R:" + (90*(networkPlayerID-localPlayerID)).ToString();

            text.text += "\n" + annotationList.Count.ToString();

            Annotation annote = new Annotation(data, networkPlayerID, localPlayerID);
            annotationList.Add(annote);
            // text.text = "Color: " + annote.color.ToString();
            
            lines.Add(SetupAnnotationObject(annote.color));
            numAnnotations += 1;
            if (currentlyDrawing == false) {
                currentlyReconstructingIndex += 1;
                // activeAnnote = annote;
            }
            currentlyDrawing = true;
            
            // currentlyReconstructingIndex += 1;
		}
	}

    private GraphicsLineRenderer SetupAnnotationObject(Color color) {
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
        GraphicsLineRenderer currLine = go.AddComponent<GraphicsLineRenderer>();
        currLine.SetMaterial(Globals.createNewMaterial(color));
        currLine.SetWidth(0.01f);

        return currLine;
        // text.text = "2 GOT HERE";
	}
}
