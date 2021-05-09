using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class DrawLineManager : MonoBehaviour
{
    // UI Elements — User Input
    public Text text;
    public LaserPointer laser;
    public FlexibleColorPicker colorPicker;



    // Annotation Elements
    private GraphicsLineRenderer currLine;
    public GameObject mesh;
    // private List<List<Vector3>> meshList;
    private List<Annotation> annotationList;
    // public Color color;


    // Counters
    private int numClicks = 0;
    private int counter;

    // private Player localPlayer;
    // Testing
    private bool currentlyReconstructing = false;
    private Vector3[] points;
    private int reconstructionIndex = 0;
    private int numVertices;

    void Start() {

		// localPlayer = PhotonNetwork.LocalPlayer;

        annotationList = new List<Annotation>();
        counter = -1;

        // TestSetupAndMeshGeneration();
    }

    //////////////////////////////////////////////////////////////////////////////
    // CONTROLLER SETTINGS FOR DRAWING                                          //
    // Index Trigger --> End current annotation and beginn new annotation       //
    // Hand Trigger  --> Draw on surfaces                                       //
    //////////////////////////////////////////////////////////////////////////////

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger)) {
            // Setup new annotation
            counter += 1;
            // Color c;
            // if (color != null) {
            //     c = color;
            // } else {
            Color color = colorPicker.color;
            // }
            // Color c = colorPicker.color;
            annotationList.Add(new Annotation(color));
            
            // text.text += "\n" + colorPicker.color.ToString();
            
            SetupAnnotationObject(color);

            // Reset numClicks
            numClicks = 0;
        } else if (laser._hitTarget && OVRInput.Get(OVRInput.RawButton.RHandTrigger)) {
            // text.text = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch).ToString();
            currLine.AddPoint(laser._endPoint);
            annotationList[counter].AddVertex(laser._endPoint);
            // text.text += "\n" + annotationList[counter].vertices[numClicks].ToString() 
                        // + annotationList[counter].numVertices.ToString();

            numClicks++;
        } else if (OVRInput.Get(OVRInput.RawButton.LHandTrigger)) {
            text.text += "\n" + "Testing Indie";
            // TestSetupAndMeshGeneration();
        } else if (currentlyReconstructing) {
            currLine.AddPoint(points[reconstructionIndex]);
            text.text += "\n" + points[reconstructionIndex].ToString();
            reconstructionIndex++;
            if (reconstructionIndex == numVertices) {
                currentlyReconstructing = false;
            }
        }
    }

    // Create new GameObject for annotations
    private void SetupAnnotationObject(Color color) {
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
        currLine = go.AddComponent<GraphicsLineRenderer>();
        currLine.SetMaterial(Globals.createNewMaterial(color));
        currLine.SetWidth(0.01f);
    }

    // Test drawing functions independent of physical drawing functionalities
    // private void TestSetupAndMeshGeneration() {
    //     points = 
    //     points = new Vector3[] {new Vector3(0f,0f,0f), 
    //                             new Vector3(0.1f,0.1f,0.1f),
    //                             new Vector3(0.2f,0.2f,0.2f),
    //                             new Vector3(0.3f,0.3f,0.4f),
    //                             new Vector3(0.4f,0.6f,0.2f)};
    //     SetupAnnotationObject(Color.red);
    //     currentlyReconstructing = true;
    //     reconstructionIndex = 0;
    //     numVertices = points.Length;
    //     // for (int i = 0; i < points.Length; i++) {
    //     //     currLine.AddPoint(points[i]);
    //     //     text.text = points[i].ToString();
    //     // }
    // }


    // Handle Network Requests
	public void OnEnable() {
		PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
	}

	public void OnDisable() {
		PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
	}	

    public void OnEvent(EventData photonEvent) {
		// print("Event received");
		byte eventCode = photonEvent.Code;

		if (eventCode == Globals.ANNOTATIONS_REQUEST && counter > -1)
		{
            // text.text = meshList[counter][0].ToString(); // "WANT ANNOTATIONS";
            if (counter == -1) {
                SendAnnotations(-1);
            }
            for (int i = 0; i < counter; i++) {
                SendAnnotations(counter);
            }
		}
	}

    void SendAnnotations(int index) {
        object[] content;
        // If there are no annotations, send message that there are no annotations to update
        if (index == -1) {
            content = new object[] {"No annotation to send"};
        } else {
            Annotation annote = annotationList[index];
            content = annote.ConvertToObject();
            // text.text = content[0].ToString();
            if (content == null) return;
        }
        
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(Globals.TRANSFERRING_ANNOTATIONS, content, raiseEventOptions, SendOptions.SendReliable);
	}
}

public class Annotation {
    public int numVertices;         // Number of vertices in annotation mesh
    public List<Vector3> vertices;  // Ordered vertices in annotation mesh
    public Color color;             // Color of annotation
    public int iterator;
    
    public Vector3[] rVertices; // received vertices;
    public Vector3[] transformedVertices;
    public bool completed;

    // Drawing Constructor
    public Annotation(Color c) {
        numVertices = 0;
        color = c;
        vertices = new List<Vector3>();
    }

    // Reconstruction Constructor
    public Annotation(object[] data, int senderID, int recieverID) {
        numVertices = (int) data[0];
        Vector3 colorRaw = (Vector3) data[1];
        color = new Color ((float)colorRaw[0], (float)colorRaw[1], (float)colorRaw[2], 1f);
        rVertices = new Vector3[numVertices];
        for (int i = 2; i < numVertices+2; i++) {
            rVertices[i-2] = (Vector3) data[i];
        }
        TransformVertices(senderID, recieverID);

        iterator = 0;
        completed = false;
        // return this;
    }

    public void AddVertex(Vector3 vertex) {
        vertices.Add(vertex);
        numVertices++;
    }

    public object[] ConvertToObject() {
        if (numVertices == 0) {
            return null;
        }
        // send numVertices, color, and vertices
        object[] obj = new object[numVertices + 2];
        obj[0] = numVertices;
        obj[1] = new Vector3(color[0], color[1], color[2]);

        for (int i = 2; i < numVertices+2; i++) {
            obj[i] = vertices[i-2];
        };

        return obj;
    }

    public void TransformVertices(int senderID, int recieverID) {
        transformedVertices = new Vector3[numVertices];
        Vector3 rotationAngle = new Vector3(0, 90*(senderID-recieverID), 0);

        for (int i = 0; i < numVertices; i++) {
            Vector3 direction = rVertices[i] - Vector3.zero;             // get point direction relative to pivot
            direction = Quaternion.Euler(rotationAngle) * direction;    // rotate it
            transformedVertices[i] = direction + Vector3.zero;                           // calculate rotated point
        }
    }

    public Vector3 GetTransformedVertex() {
        // if (iterator >= numVertices) return null;
        Vector3 vertex = transformedVertices[iterator];
        iterator++;
        if (iterator == numVertices)
            completed = true;
        return vertex;
    }
}