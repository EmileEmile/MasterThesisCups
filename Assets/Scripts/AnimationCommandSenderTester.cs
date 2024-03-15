using UnityEngine;

public class AnimationCommandSenderTester : MonoBehaviour {

    public GameObject character;
    public string animationID;
    public bool playBack1 = false;
    public bool acknowledgeFriendly = false;
    public bool acknowledgeUnfriendly = false;

    private GretaCharacterAnimator _charAnimScript;

    void Start()
    {
        _charAnimScript = character.GetComponent<GretaCharacterAnimator>();
    }

    // Update is called once per frame
    void Update ()
    {
        //Debug.Log("name of the agent:    "+name); 
        try
        {
            if (playBack1)
            {
                _charAnimScript.PlayAgentAnimation(animationID);

                playBack1 = false;
            }
            if (acknowledgeFriendly)
            {
                Debug.Log("Playing the sound of agent 1!");
                _charAnimScript.PlayAgentAnimation("Examples/EmileProject/CoffeeCup/AcknowledgeFriendly");
                acknowledgeFriendly = false;
            }

            if (acknowledgeUnfriendly)
            {
                _charAnimScript.PlayAgentAnimation("Examples/EmileProject/CoffeeCup/AcknowledgeUnfriendly");
                acknowledgeUnfriendly = false;
            }

        }
        catch (System.Exception e) {
            Debug.Log(e.ToString());
        }
        
        /*if (Input.GetKeyUp(KeyCode.T) && animationID != null && animationID.Trim().Length > 0)
            {
                _charAnimScript.PlayAgentAnimation(animationID);
            }*/
    }

    
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(gameObject.name + " was trrigered by : " + col.gameObject.name);

        if (col.gameObject.name == "Avatar" && playBack1)
        {
            _charAnimScript.PlayAgentAnimation(animationID);
            playBack1 = false;
        }
    }
}