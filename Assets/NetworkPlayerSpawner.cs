using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;

    // private Vector3[] spawnPoints = new Vector3[4];
    
    // private GameObject spawnPoint;
    private int networkPlayerID;

    public override void OnJoinedRoom()
    {   
        base.OnJoinedRoom();
        
        // initializeSpawnPoint();
        networkPlayerID = (int)(PhotonNetwork.LocalPlayer.ActorNumber % 4);
        // Initialize user stuff
        // UpdateParticleColor(true);
        // var spawnPoint = GameObject.Find(station);
        // if (spawnPoint == null) {
        //     Debug.Log("Yikes");
        // }
        foreach (Player player in PhotonNetwork.PlayerList) {
        // for (int i = 1; i < 5; i++){
            int num = player.ActorNumber % 4;
            if (num != networkPlayerID) {
                Debug.Log("HERE HERE HERE" + num);
                // Debug.Log("HERE" + station);
                UpdateParticleColor(num, true);
            }
        }

        Debug.Log("Network Player ID: " + networkPlayerID);
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);    
        // UpdateParticleColor(networkPlayerID, true);
    }

    private void UpdateParticleColor(int num, bool status)   // 0 = left, 1 = joined
    {
        var station = "Station " + num.ToString();
        Debug.Log("NETWORK PLAYER ID: " + num + ", NETWORK PLAYER'S STATION: " + station);
        Debug.Log("UPDATING PARTICLE COLOR FOR STATION " + num);

        // Initialize user stuff
        GameObject spawnPoint = GameObject.Find(station);
        if (spawnPoint == null) {
            Debug.Log("Yikes");
        }

        var particles = spawnPoint.transform.Find("Particle System").gameObject;
        if (particles == null) {
            Debug.Log("Did not find particle System");
        }
        var ps = particles.GetComponent<ParticleSystem>().main;
        ps.startColor = Color.green;
    }


    // private void UpdateParticleColor(bool status)   // 0 = left, 1 = joined
    // {
    //     var station = "Station " + networkPlayerID.ToString();
    //     Debug.Log("NETWORK PLAYER ID: " + networkPlayerID + ", NETWORK PLAYER'S STATION: " + station);

    //     // Initialize user stuff
    //     spawnPoint = GameObject.Find(station);
    //     if (spawnPoint == null) {
    //         Debug.Log("Yikes");
    //     }

    //     var particles = spawnPoint.transform.Find("Particle System").gameObject;
    //     if (particles == null) {
    //         Debug.Log("Did not find particle System");
    //     }
    //     var ps = particles.GetComponent<ParticleSystem>().main;
    //     ps.startColor = Color.blue;

    //     // return spawnPoint;
    // }

    public override void OnLeftRoom()
    {
        Debug.Log("A player left the room.");
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
