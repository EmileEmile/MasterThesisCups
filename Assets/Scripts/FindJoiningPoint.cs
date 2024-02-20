using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindJoiningPoint : MonoBehaviour {

    public static string joiningPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        //colObject = col;
        //avatarCollision = true;
        //Debug.Log(col.gameObject.name + " entered to : " + gameObject.name);

        joiningPoint = gameObject.name;
    }
}
