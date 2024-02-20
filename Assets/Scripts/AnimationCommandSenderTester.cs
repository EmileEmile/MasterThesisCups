using UnityEngine;

public class AnimationCommandSenderTester : MonoBehaviour {

    public GameObject character;
    public string animationID;
    //static bool playBack1 = false;
    //static bool playBack2 = true;
    //static bool playBack3 = false;

    private GretaCharacterAnimator _charAnimScript;

    void Start()
    {
        _charAnimScript = character.GetComponent<GretaCharacterAnimator>();
    }

    // Update is called once per frame
    void Update ()
    {/*
        //Debug.Log("name of the agent:    "+name); 
        try
        {
            if (playBack2 && name == "Agent2")
            {
                    if (GroupFormation.avatarCollision && GroupFormation.colObject.gameObject.name == "Female")
                    {
                        _charAnimScript.PlayAgentAnimation(animationID);
                        playBack2 = false;
                        Debug.Log("Set the playback of the AGENT 1 to TRUE!");
                        playBack1 = true;
                    }
            }
            if (playBack1 && name == "Agent1")
            {
                Debug.Log("Playing the sound of agent 1!");
                _charAnimScript.PlayAgentAnimation("Examples/DemoEN/Politeness/Politeness Startegy 2");
                playBack1 = false;
                playBack3 = true;
            }

            if (playBack3 && name == "Agent3")
            {
                _charAnimScript.PlayAgentAnimation("Examples/DemoEN/Politeness/Politeness Startegy 1");
                playBack3 = false;
            }

        }
        catch (System.Exception e) {
            Debug.Log(e.ToString());
        }
        */
        /*if (Input.GetKeyUp(KeyCode.T) && animationID != null && animationID.Trim().Length > 0)
            {
                _charAnimScript.PlayAgentAnimation(animationID);
            }*/
    }

    /*
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(gameObject.name + " was trrigered by : " + col.gameObject.name);

        if (col.gameObject.name == "Avatar" && playBack)
        {
            _charAnimScript.PlayAgentAnimation(animationID);
            playBack = false;
        }
    }*/
}