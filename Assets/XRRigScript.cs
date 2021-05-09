using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRRigScript : MonoBehaviour
{
    private Vector3 positionCamera;
    // public XRRig xrrig;

    // Start is called before the first frame update
    // void Start()
    // {
    //     camera = GetComponent<XRRig>();
    // }

    // Update is called once per frame
    void Update()
    {
        GoToTarget();
    }

    public void GoToTarget()
    {
        positionCamera = new Vector3 (1,2,3);
        this.transform.position = positionCamera;
    }

}
