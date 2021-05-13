using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayer : MonoBehaviour
{
//     public string ID;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Animator leftHandAnimator;
    public Animator rightHandAnimator;
    private PhotonView photonView;

    private Transform headRig;
    private Transform leftHandRig;
    private Transform rightHandRig;

    private int brainRotation;

    // private Vector3 positionCamera;
    // public XRRig xrrig;


    // Start is called before the first frame update
    void Start()
    {
        // ID = (PhotonNetwork.LocalPlayer.ActorNumber % 4 + 1).ToString();
        photonView = GetComponent<PhotonView>();
        OVRPlayerController rig = FindObjectOfType<OVRPlayerController>();
        headRig = rig.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
        leftHandRig = rig.transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor/LeftControllerAnchor");
        rightHandRig = rig.transform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor/RightControllerAnchor");

        if (photonView.IsMine){
            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                item.enabled = false;
            }        
        }

    }

    // Update is called once per frame
    void Update()
    {  
        if (photonView.IsMine)
        {
            MapPosition(head, headRig);
            MapPosition(leftHand, leftHandRig);
            MapPosition(rightHand, rightHandRig);

            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.LeftHand), leftHandAnimator);
            UpdateHandAnimation(InputDevices.GetDeviceAtXRNode(XRNode.RightHand), rightHandAnimator);

            // photonView.RPC("InitializeMyPlayerRPC", RpcTarget.All, 2);
        }
    }

    void UpdateHandAnimation(InputDevice targetDevice, Animator handAnimator)
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    void MapPosition(Transform target, Transform rigTransform)
    {
        // InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        // InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
