using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //public const float Y_ANGLE_MIN = 0;
    //public const float Y_ANGLE_MAX = 0;
    public Transform target;
    //public float cameraSpeed = 0.125f;
    public Vector3 offset = new Vector3(-0.53f, 1.93f, 0.75f);
        
	// Use this for initialization
	void Start () {
        offset = new Vector3(-0.53f, 1.93f, 0.75f);
    }
	
	// Update is called once per frame
	void LateUpdate () {
        /*Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition =  Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
        transform.position = smoothPosition;*/

        transform.position = target.position + offset;
        transform.rotation = target.rotation;


        //transform.LookAt(target);
    }
}
