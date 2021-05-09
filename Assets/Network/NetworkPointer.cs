// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI; // Required when Using UI elements.
// using Photon.Pun;
// using Photon.Realtime;

// public class NetworkPointer : MonoBehaviour
// {

//     private Toggle toggle;

//     public LaserPointer laser;

//     private Player localPlayer;
//     // Start is called before the first frame update
//     private GameObject sphere;
//     void Start()
//     {
//         this.toggle = (Toggle)FindObjectOfType(typeof(Toggle));
//         this.toggle.isOn = false;
//         localPlayer = PhotonNetwork.LocalPlayer;
//         sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//         sphere.transform.position = new Vector3(0,0,0);
//         sphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (laser._hitTarget) {
//             localPlayer.CustomProperties["LaserPosition"] = laser._endPoint;
//         }
//         if (!toggle.isOn) return;

//         foreach (Player player in PhotonNetwork.PlayerList) {
// 			if (this.localPlayer != player) {
// 				sphere.transform.position = (Vector3) player.CustomProperties["LaserPosition"];
// 				return;
// 			}
// 		}
//         // if (laser._hitTarget) {
//         //     localPlayer.CustomProperties["LaserPosition"] = laser._endPoint;
//         // }
//     }
// }
