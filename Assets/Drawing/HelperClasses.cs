using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public static class Globals
{
    // public const Int32 BUFFER_SIZE = 512; // Unmodifiable
    public const byte ANNOTATIONS_REQUEST = 1;

    // public const byte TEST_TEXT = 2;

    public const byte TRANSFERRING_ANNOTATIONS = 2;

    public const float E = 0.001f;

    // public static localPlayer.

    public static Material createNewMaterial(Color color) {
        Shader shader = Shader.Find("Standard");
        Material mat = new Material(shader);
        mat.SetInt("_SmoothnessTextureChannel", 1);
        mat.SetColor("_Color", color);
        return mat;
    }

    public static int convert(int actorNumber) {
        if (actorNumber % 4 == 0)
            return 4;
        else
            return (actorNumber % 4);
    }

    // public static String FILE_NAME = "Output.txt"; // Modifiable
    // public static readonly String CODE_PREFIX = "US-"; // Unmodifiable
}


public class Annotation {
    public static int numHeaderElements = 3;
    public int numVertices;         // Number of vertices in annotation mesh
    public List<Vector3> vertices;  // Ordered vertices in annotation mesh
    public Color color;             // Color of annotation
    public int iterator;
    public float brainRotation;       // Rotation of the brain at the time the annotation was made (reference: 0f)
    
    public Vector3[] rVertices; // received vertices;
    public Vector3[] transformedVertices;
    public bool completed = false;

    // Drawing Constructor
    public Annotation(Color c, float rotation) {
        numVertices = 0;
        color = c;
        vertices = new List<Vector3>();
        brainRotation = rotation;
    }

    // Reconstruction Constructor
    public Annotation(object[] data, int senderID, int recieverID) {
        numVertices = (int) data[0];
        Vector3 colorRaw = (Vector3) data[1];
        color = new Color ((float)colorRaw[0], (float)colorRaw[1], (float)colorRaw[2], 1f);

        brainRotation = (float) data[2];

        rVertices = new Vector3[numVertices];
        for (int i = numHeaderElements; i < numVertices+numHeaderElements; i++) {
            rVertices[i-numHeaderElements] = (Vector3) data[i];
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
        object[] obj = new object[numVertices + numHeaderElements];
        obj[0] = numVertices;
        obj[1] = new Vector3(color[0], color[1], color[2]);
        obj[2] = brainRotation;

        for (int i = numHeaderElements; i < numVertices+numHeaderElements; i++) {
            obj[i] = vertices[i-numHeaderElements];
        };

        return obj;
    }

    public void TransformVertices(int senderID, int recieverID) {
        transformedVertices = new Vector3[numVertices];
        Vector3 rotationAngle = new Vector3(0, 90*(recieverID-senderID), 0);

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