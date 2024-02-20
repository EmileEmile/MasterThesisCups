using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class EndTrial : MonoBehaviour
{
    //trigger action associated with the right trigger
    public SteamVR_Action_Boolean m_EndTrial = null;
    
    //"flag" to manage right trigger press to end the trial, and enable it only when close to the gorup
    public GameObject check = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the endtrial action is performed by any controller, set the flag object as active
        if(m_EndTrial.GetStateDown(SteamVR_Input_Sources.Any)) {
            check.SetActive(true);
        }
    }
}
