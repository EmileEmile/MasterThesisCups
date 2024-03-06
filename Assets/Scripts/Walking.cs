using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour {
    private Vector3 moveDirection = Vector3.zero;

    //Trajectory variables to save the path user follows to join the group
    public static List<float> userPosX = new List<float>();
    public static List<float> userPosY = new List<float>(); // it will be 0
    public static List<float> userPosZ = new List<float>();
    public static List<float> userRotX = new List<float>();
    public static List<float> userRotY = new List<float>();
    public static List<float> userRotZ = new List<float>();
    public static List<float> userForwardX = new List<float>();
    public static List<float> userForwardY = new List<float>();
    public static List<float> userForwardZ = new List<float>();
    
    /*
    float prevUserPosX = -9999;
    float prevUserPosZ = -9999;
    float prevUserRotY = -9999;
    */
    //int transNewItemIndex = 0; //last item index in the trajectory list
    public static bool captureTrajectoryFlag = false; //start capturing trajectory only when the trial starts (by initilializing a trial in the GroupFormation script) and stop it when the trial stops (by pressing N in the GroupFormation script)

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //save the user trajectory information if it should capture the user trajcotry
        if (captureTrajectoryFlag)
            SavePlayerTrajectory();
    }

    //add the user position and rotation to the array
    void SavePlayerTrajectory()
    {
        //add the new transform values only if the current transform differs from the previous one
        //it is possible that user stop at some moments. so I remove the following condition and its related variables
        //if (prevUserPosX != transform.position.x || prevUserPosZ != transform.position.z || prevUserRotY != transform.rotation.y)
        //{
        userPosX.Add(transform.position.x);
        userPosY.Add(0);
        userPosZ.Add(transform.position.z);

        userRotX.Add(0);
        userRotY.Add(transform.rotation.y);
        userRotZ.Add(0);

        userForwardX.Add(transform.forward.x);
        userForwardY.Add(transform.forward.y);
        userForwardZ.Add(transform.forward.z);

        //Debug.Log("SavePlayerTrajectory" + userPosX[userPosX.Count - 1]);
        /*
            //save the current added value
            prevUserPosX = transform.position.x;
            prevUserPosZ = transform.position.z;
            prevUserRotY = transform.rotation.y;*/
        //}

        /*
        if (transNewItemIndex > 0) //It is the second or greater elements in the array to be added
        {
            //add the new transform values only if the current transform differs from the previous one
            if (userPosX[transNewItemIndex - 1] != transform.position.x || userPosZ[transNewItemIndex - 1] != transform.position.z || userRotY[transNewItemIndex - 1] != transform.rotation.y)
            {
                userPosX.Add(transform.position.x);
                userPosY.Add(0);
                userPosZ.Add(transform.position.z);

                userRotX.Add(0);
                userRotY.Add(transform.rotation.y);
                userRotZ.Add(0);

                userForwardX.Add(transform.forward.x);
                userForwardY.Add(transform.forward.y);
                userForwardZ.Add(transform.forward.z);

                transNewItemIndex++;
             
                
            }
        }
        else if (transNewItemIndex == 0) // It is the first element in the array to be added
        {
            userPosX.Add(transform.position.x);
            userPosY.Add(0);
            userPosZ.Add(transform.position.z);

            userRotX.Add(0);
            userRotY.Add(transform.rotation.y);
            userRotZ.Add(0);

            userForwardX.Add(transform.forward.x);
            userForwardY.Add(transform.forward.y);
            userForwardZ.Add(transform.forward.z);

            transNewItemIndex++;
        }*/
    }
}
