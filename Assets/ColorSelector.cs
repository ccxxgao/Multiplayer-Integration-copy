using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public Color colorSelector;

    public Text text;

    void Start()
    {
        transform.GetComponent<MeshRenderer>().material.color = colorSelector;
    }

    // Update is called once per frame

    // private void OnTriggerEnter(Collider other)
    // {
    //     text.text += colorSelector.ToString() + " TRIGGERED";
    //     // Find the brush in scene
    //     DrawLineManager drawLineManager = (DrawLineManager)FindObjectOfType(typeof(DrawLineManager));

    //     // If there is a brush, set our brush's color with the setBrushColor Method
    //     if (drawLineManager) {
    //         drawLineManager.color = colorSelector;
    //         // other.gameObject.GetComponent<drawLineManager>().setBrushColor(colorSelector);
    //     }
    // }
    // void Update()
    // {
        
    // }
}
