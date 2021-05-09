using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
// using UnityEngine.UI;


public class NetworkDataManager : MonoBehaviourPunCallbacks
{

    public int brainRotation = 2;
    
    public void Update() {
        PhotonView photonView = PhotonView.Get(this);
        // photonView.RPC("InitializeMyPlayerRPC", RpcTarget.All, brainRotation);
    }

    // void Update(PhotonView photonView) {
    //     if (photonView.IsMine)
    //     {
    //         phtonView.RPC("InitializeMyPlayerRPC", PhotonTargets.OthersBuffered, brainRotation);
    //     }
    // }

    // PhotonView.RPC("ChangeValue", PhtonTargets.All, level);

    // private void initializeCurrentUserData()
    // {
    //     Dictionary<string, string> dataToShare = new Dictionary<string, string>();
    //     dataToShare.Add("BrainRotation", 21);
    //     photonView.RPC("InitializeMyPlayerRPC",
    //                     PhotonTargets.OthersBuffered,
    //                     dataToShare as object
    //                     );

    // }

    [PunRPC]
    void InitializeMyPlayerRPC(int brainRot)
    {
        // Dictionary<string, string> data = dataToShare as Dictionary<string, string>;          
        // player.Name = data["BrainRotation"].ToString();
        brainRotation = brainRot;
        Debug.Log("OMFG IT'S HERE");
    }

}
