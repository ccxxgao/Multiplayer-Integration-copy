using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public static class Globals
{
    // public const Int32 BUFFER_SIZE = 512; // Unmodifiable
    public const byte ANNOTATIONS_REQUEST = 1;

    public const byte TEST_TEXT = 2;

    public const byte TRANSFERRING_ANNOTATIONS = 3;

    // public static localPlayer.

    public static Material createNewMaterial(Color color) {
        Shader shader = Shader.Find("Standard");
        Material mat = new Material(shader);
        mat.SetInt("_SmoothnessTextureChannel", 1);
        mat.SetColor("_Color", color);
        return mat;
    }

    // public static String FILE_NAME = "Output.txt"; // Modifiable
    // public static readonly String CODE_PREFIX = "US-"; // Unmodifiable
}