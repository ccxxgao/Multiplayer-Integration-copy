// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class Draw : MonoBehaviour
// {
//     public LaserPointer laser;

//     public Button button;
//     // Start is called before the first frame update
//     // void Start()
//     // {
        
//     // }

//     // Update is called once per frame
//     void Update()
//     {
//         RaycastHit hit;
//         GameObject pointer = laser.cursorVisual;
//         Vector3 pointerPosition = pointer.transform.position;

//         // if (pointerPosition == Vector3.zero) { // || !Physics.Raycast(laser._startPoint, laser._endPoint, out hit, Mathf.Infinity)) {
//         if (!laser._hitTarget) {
//             button.GetComponentInChildren<Text>().text = "Not hit";
//             return;
//         }
    
//         button.GetComponentInChildren<Text>().text = laser._endPoint.ToString();

//         Ray ray = new Ray(laser._startPoint, laser._endPoint*100);
//         // if (!Physics.Raycast(ray, out hit, 100))
//         //     return;
//         float laserDistance = Vector3.Distance(laser._startPoint, laser._endPoint);
//         if (!Physics.Raycast(ray, out hit, 100f)) {
//             button.GetComponentInChildren<Text>().text = laserDistance.ToString();
//             // laser._startPoint.ToString() + laser._endPoint.ToString() ;
//             return;
//         }

//         // if (Physics.SphereCast(laser._endPoint, 2, Vector3.down, out hit, 10))
//         // {
//         //     // distanceToObstacle = hit.distance;
//         //     button.GetComponentInChildren<Text>().text = hit.distance.ToString();
//         // } else {
//         //     return;
//         // }
//         // else {
//             // button.GetComponentInChildren<Text>().text = "Raycast is working";
//         //     return;
//         // }
//         Renderer rend = hit.transform.GetComponent<Renderer>();
//         MeshCollider meshCollider = hit.collider as MeshCollider;
//         // Debug.DrawLine(transform.position, hit.transform.position, Color.red);

//         if (hit.point != pointerPosition) {
//             // button.GetComponentInChildren<Text>().text = "Raycast != Laser";
//             button.GetComponentInChildren<Text>().text = hit.transform.name;

//             // button.GetComponentInChildren<Text>().text = hit.point.ToString() + "  " + laser._endPoint.ToString();
//             button.GetComponentInChildren<Text>().fontSize = 8;
//             return;
//         }


//         if (rend == null){
//             // button.GetComponentInChildren<Text>().text = "Renderer is null";
//             button.GetComponentInChildren<Text>().text = hit.ToString();
//             return;
//         }
//         // else if ()

//         if (rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null) {
//             button.GetComponentInChildren<Text>().text = "Issue is in here";
//             return;
//         }

//         // button.GetComponentInChildren<Text>().text = "Hit";

//         Texture2D tex = rend.material.mainTexture as Texture2D;
//         Vector2 pixelUV = hit.textureCoord;
//         pixelUV.x *= tex.width;
//         pixelUV.y *= tex.height;

//         tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
//         tex.Apply();
//     }

//     // void Update()
//     // {
//     //     RaycastHit hit;
//     //     float hitDistance;
//     //     Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;
//     //     Debug.DrawRay(transform.position, forward, Color.green);

//     //     // if (pointerPosition == Vector3.zero) { // || !Physics.Raycast(laser._startPoint, laser._endPoint, out hit, Mathf.Infinity)) {
//     //     if (!laser._hitTarget) {
//     //         button.GetComponentInChildren<Text>().text = "Not hit";
//     //         return;
//     //     }
    
//     //     button.GetComponentInChildren<Text>().text = laser._endPoint.ToString();

//     //     Ray ray = new Ray(laser._startPoint, laser._endPoint);
//     //     // if (!Physics.Raycast(ray, out hit, 100))
//     //     //     return;
//     //     if (!Physics.Raycast(ray, Mathf.Infinity)) {
//     //         button.GetComponentInChildren<Text>().text = Vector3.Distance(laser._startPoint, laser._endPoint).ToString();
//     //         // laser._startPoint.ToString() + laser._endPoint.ToString() ;
//     //         return;
//     //     }

//     //     // if (Physics.SphereCast(laser._endPoint, 2, Vector3.down, out hit, 10))
//     //     // {
//     //     //     // distanceToObstacle = hit.distance;
//     //     //     button.GetComponentInChildren<Text>().text = hit.distance.ToString();
//     //     // } else {
//     //     //     return;
//     //     // }
//     //     else {
//     //             button.GetComponentInChildren<Text>().text = "Raycast is working";
//     //             return;
//     //     }
//     //     Renderer rend = hit.transform.GetComponent<Renderer>();
//     //     MeshCollider meshCollider = hit.collider as MeshCollider;

//     //     if (rend == null){
//     //         button.GetComponentInChildren<Text>().text = "Renderer is null";
//     //         return;
//     //     }
//     //     // else if ()

//     //     if (rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null) {
//     //         button.GetComponentInChildren<Text>().text = "Issue is in here";
//     //         return;
//     //     }

//     //     // button.GetComponentInChildren<Text>().text = "Hit";

//     //     Texture2D tex = rend.material.mainTexture as Texture2D;
//     //     Vector2 pixelUV = hit.textureCoord;
//     //     pixelUV.x *= tex.width;
//     //     pixelUV.y *= tex.height;

//     //     tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
//     //     tex.Apply();
//     // }



// }
