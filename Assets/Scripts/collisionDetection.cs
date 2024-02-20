using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetection : MonoBehaviour
{

    public static bool cupGrabbedFlag = false;

    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Collision! : ");

        //Debug.Log(col.gameObject.name + " entered to : " + gameObject.name);
        /*
        if (col.gameObject.name == avatarName)
        {
            if (collisionNo == collisionThreshold) //avatr is inside the memebrs' group radius for talking (O-Space)
            {
                //Change the focus of the lateral agent Head Look Controler to the avatar
                agents[lateralAgent1Id].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
            }
        }*/



        if (gameObject.name == "TableCup" && col.gameObject.name == "Controller (right)")
        {
            cupGrabbedFlag = true;


            Debug.Log("COFFFFFFEEEEEEE CUP GRABBBBBBBBBBBBBBED! : ");
        }


        if (gameObject.name == "InvisibleGroupCenterWall" && col.gameObject.name == "Camera (eye)")
        {
            //cupGrabbedFlag = true;


            Debug.Log(col.gameObject.name + " entered to : " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (gameObject.name == "TableCup" && col.gameObject.name == "Controller (right)")
        {
            cupGrabbedFlag = false;


            Debug.Log("COFFFFFFEEEEEEE CUP RELEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEESED! : ");
        }

        if (gameObject.name == "InvisibleGroupCenterWall" && col.gameObject.name == "Camera (eye)")
        {
            //cupGrabbedFlag = true;


            Debug.Log(col.gameObject.name + " exited from : " + gameObject.name);
        }
    }
}