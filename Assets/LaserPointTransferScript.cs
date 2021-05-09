// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using Photon.Realtime;

// public class LaserPointTransferScript : MonoBehaviour
// {

//     // public Vector3 position;

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
//         if (stream.IsWriting == true) {
//             stream.SendNext(this.transform.position);
//         } else {
//             position = (Vector3) stream.ReceiveNext();
//         }
//     }

//     void OnPhoton
// }
