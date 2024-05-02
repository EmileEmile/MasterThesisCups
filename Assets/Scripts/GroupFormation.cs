using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GroupFormation : MonoBehaviour {
    private const int NO_CONVERSATION = 0;
    private const int FRIENDLY_CONVERSATION = 1;
    private const int NEUTRAL_CONVERSATION = 2;
    private const int UNFRIENDLY_CONVERSATION = 3;

    private const int NO_GAZE = 0;
    private const int USER_GAZE = 1;
    private const int A_U_A_GAZE = 3;
    private const int OTHER_AGENT_GAZE = 2;

    private const int IGNORE_VERBAL = 0;
    private const int NEUTRAL_WAIT_VERBAL = 1;
    private const int FRIENDLY_SUBMISSIVE_WAIT_VERBAL = 2;
    private const int FRIENDLY_DOMINANT_WAIT_VERBAL = 3;
    private const int HOSTILE_DOMINANT_WAIT_VERBAL = 4;
    private const int HOSTILE_SUBMISSIVE_WAIT_VERBAL = 5;


    public float NEUTRAL_WAIT_GAZETIME = 5f;
    public float FRIENDLY_SUBMISSIVE_WAIT_GAZETIME = 5f;
    public float FRIENDLY_DOMINANT_WAIT_ACKNOWLEDGE_GAZETIME = 6f;
    public float HOSTILE_SUBMISSIVE_ACKNOWLEDGE_GAZETIME = 6f;
    public float HOSTILE_DOMINANT_ACKNOWLEDGE_GAZETIME = 5f;


    public string ForceScenario = null;

    public bool flag = false;
    public bool flagAcknowledgeFriendly = false;
    public bool flagAcknowledgeUnfriendly = false;

    public float AudioStartVolume = 1.0f;



  [Header("Agent Info")]
    // Instantiates prefabs in a circle formation
    public GameObject[] agents;
    private HeadLookController[] agentHeads;
    public GretaDelayWrapper[] gretaDelayWrappers;

    public GameObject[] TrialAgents;

    //TODO Uncomment if using dialogues
    /* public string FriendlyPrefix;
     public string NeutralPrefix;
     public string UnfriendlyPrefix;

     public List<GretaDelayWrapper.AnimationCommand> FriendlyDialogues1;
     public List<GretaDelayWrapper.AnimationCommand> FriendlyDialogues2;
     public List<GretaDelayWrapper.AnimationCommand> NeutralDialogues1;
     public List<GretaDelayWrapper.AnimationCommand> NeutralDialogues2;
     public List<GretaDelayWrapper.AnimationCommand> UnfriendlyDialogues1;
     public List<GretaDelayWrapper.AnimationCommand> UnfriendlyDialogues2;*/


    GretaDelayWrapper.AnimationCommand RestCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/FD/Rest", 0);
    GretaDelayWrapper.AnimationCommand WaveCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/FD/Wave", 0);
    GretaDelayWrapper.AnimationCommand NeutralWaitPartnerCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/FD/Rest", 0);


    GretaDelayWrapper.AnimationCommand NeutralWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/FD/WaitNeutral", 3);
    GretaDelayWrapper.AnimationCommand FriendlySubmssiveWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/FD/WaitFriendlySubmissive", 3);
    GretaDelayWrapper.AnimationCommand FriendlyDominantWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/FD/WaitFriendlyDominant", 4);
    GretaDelayWrapper.AnimationCommand HostileSubmssiveWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/FD/WaitHostileSubmissive", 3);
    GretaDelayWrapper.AnimationCommand HostileDominantCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/FD/WaitHostileDominant", 3);

    // OLD STRATEGIES
    /*public GretaDelayWrapper.AnimationCommand AcknowledgeFriendlyNoWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/AcknowledgeFriendly", 3);
    public GretaDelayWrapper.AnimationCommand AcknowledgeFriendlyNoWaitPartnerCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/Rest", 0);

    public GretaDelayWrapper.AnimationCommand AcknowledgeNeutralNoWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/AcknowledgeNeutral", 3);
    public GretaDelayWrapper.AnimationCommand AcknowledgeNeutralNoWaitPartnerCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/Rest", 0);

    public GretaDelayWrapper.AnimationCommand AcknowledgeUnfriendlyNoWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/AcknowledgeUnfriendly", 3);
    public GretaDelayWrapper.AnimationCommand AcknowledgeUnfriendlyNoWaitPartnerCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/Rest", 0);

    public GretaDelayWrapper.AnimationCommand AcknowledgeFriendlyWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/AcknowledgeFriendlyWait", 3);
    public GretaDelayWrapper.AnimationCommand AcknowledgeFriendlyWaitPartnerCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/Rest", 0);

    public GretaDelayWrapper.AnimationCommand AcknowledgeNeutralWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/AcknowledgeNeutralWait", 3);
    public GretaDelayWrapper.AnimationCommand AcknowledgeNeutralWaitPartnerCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/Rest", 0);

    public GretaDelayWrapper.AnimationCommand AcknowledgeUnfriendlyWaitCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/AcknowledgeUnfriendlyWait", 3);
    public GretaDelayWrapper.AnimationCommand AcknowledgeUnfriendlyWaitPartnerCommand = new GretaDelayWrapper.AnimationCommand("Examples/EmileProject/CoffeeCup/Rest", 0);*/


    GameObject speakerAgentHeadObject; // center of the group or speaking agent
    public GameObject MaleSpeakerAgentHeadObject;
    public GameObject FemaleSpeakerAgentHeadObject;
    public GameObject avatarHeadObject; // avatar's head cube
    public GameObject addresseeAgentHeadObject; // the adressee agent
    public GameObject paintingGazeObject; // the gaze object for the painting on the wall


    /*distance between agents and the center of the group in front of the coffee table (radiusX)! 
      NOTE 1: If we want to have 0.5 meters distance between two agents, we set 0.25 for this parameter. Then, two agents will stand 0.25 from the ceneter of the group and 0.5 meter apart from each other.
      NOTE 2: about 40 cm of the distacne between agents will be consumed by their half front part of bodies. Therefore, we should add half of this length (20 cm) to each agent's distance from the center of the group.
        AgentsDistance in the VE     = { 0.4f, 0.7f, 1f, 1.3f }
        AgentsDistance in programming = AgentsDistance in the VE / 2  + 0.20
        AgentsDistance in programming= { 0.40f, 0.55f, 0.7f, 0.85f  };
      ;*/
    //float[] AgentsDistance = { 0.4f, 0.55f, 0.7f, 0.85f };

    public GameObject invisibleGroupCenterWallObject;

    //public GameObject coffeeTable;
    public GameObject tableMug;
    public GameObject trialHintsCanvas;

    //bool playBack = true;

    //public static Collider colObject;
    //public static bool avatarCollision;

    int mainAgentId; // ID of the centeral agent for the experiment
    int lateralAgent1Id; // ID of the lateral agent 1

    /* agents 0 .. 1 --> female
     * agents 2 .. 3 --> male
     * agentGender = 2 if the playerGender = male to generate male agents
     * agentGenr = 0 if the playerGender = female to generate female agents
     */
    int agentsGender = 0;

    //experiment parameters
    public bool DisableTeleport = false;
    public bool demo;
    bool trialOngoing = false;
    public int NumberOfDemoRounds = 2;//numebr of demo trials
    public int expCondRepeatNo;//number of repetition of conditions in each experiment (eg. 2, will create 8 trials)    
    //indicates the row to be fetched in the balanced latin square csv file to create the trials (ex. 0..3)
    //it will be automatically fetched by the ReadRowToFetchForBalancedLatinSquare function from a CSV file
    [Range(0, 3)]
    int latinSquareRowToFetch;


    public int trialIdNum = 0;

    public List<string> ExpeConditions; //list of all trial of the experiment provided by latin square method (each value is a string that contains several variables' values: table and group distance, gaze,... check latin square help file for details)
    public string trialStr;//it includes all the values for one experiment condition (trial) (table and group distance, gaze,... check latin square help file for details).

    int trialStaticActionID = 0;
    int trialGazeID = 0;
    int trialBehaviorID = 0;
    int trialVerbalID = 0;


    //CSV files parameters
    private StreamWriter SWexpe;
    private StreamWriter SWtrajectory;

    [Header("Hints Window")]
    public TMP_Text trialIdTextBox; //the text box to show the trial id
    [Space(20)]

    [Header("User Data Entry")]
    //User profile data entry fields
    public GameObject dataEntry;
    public Button playerId; //the user who is palying with the avatar
    private string playerIdValue;
    public TMP_Dropdown playerGender;
    
    public TMP_Text trialIdTextBoxEnd;
    public TMP_Text timeTextBoxEnd;
    public TMP_Text timePspaceEnd;
    //canvas elements to be active only during the demo
    public GameObject demoSlider = null;

    [Space(20)]
    public GameObject demoSliderLabel = null;


    [Header("Questoinaire Data")]
    //Questionnaire data entry fields
    //public Slider SpatialValue;
    //public Slider EngagementValue;
    //public Slider SocialRealismValue;
    //public Slider SocialPresenceActiveValue;

    public Slider ValenceParticipants;
    public Slider ValenceAgents;
    public Slider DominanceValue;
    public Slider FriendlinessOutValue;
    public Slider FriendlinessInValue;
    public int WheelValue;

    public Button NextButton;
    public SegmentButton selectedSegmentButton;

    public GameObject endtrialUI;
    public GameObject questionnaireDemo;
    public GameObject questionnaireObject1;
    public GameObject questionnaireObject2;
    public Button playerComments;
    [Space(20)]

    //User End message field to save their comments
    public GameObject endMessageWindow;


    public bool gameStartedFlag = false;

    //"flag" to manage right trigger press to end the trial, and enable it only when "collisionDetection.cupGrabbedFlag = true". it means when the right controller collided with the coffee cup on the table.
    public GameObject endTrialOnTriggerPress = null;

    //marker on spawn position
    public GameObject spawnMarker = null;

    //private int collisionThreshold;

    private CapsuleCollider myCollider;

    private bool hitPspace = false;

    private float timeForTrial;
    private float timeForPspace;

    //Hack to set body position

    public float BeforeWaveTime = 0.3f;

    private float waveTimer = 0;
    private bool hasWaved = false;

    private string DemoStr = "0300";

    // Use this for initialization
    private void Start () {
        myCollider = transform.GetComponent<CapsuleCollider>();

        agentHeads = new HeadLookController[agents.Length];
        gretaDelayWrappers = new GretaDelayWrapper[agents.Length];

        for (int i = 0; i < agents.Length; i++)
        {
            agentHeads[i] = agents[i].GetComponent<HeadLookController>();
            gretaDelayWrappers[i] = agents[i].GetComponent<GretaDelayWrapper>();
            //look at the participant
            agentHeads[i].targetObject = avatarHeadObject.transform;
        }

        TrialAgents[0].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
        TrialAgents[1].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;

        endMessageWindow.SetActive(false);

        endtrialUI.SetActive(false);
        questionnaireDemo.SetActive(false);
        questionnaireObject1.SetActive(true);
        questionnaireObject2.SetActive(false);

        trialHintsCanvas.SetActive(true);

        AudioListener.volume = 0.0f;

        //AddPrefixesToAllXmlPaths();

        //display demo slider on data collection canvas only during demo
        if (demo) 
        {
            demoSlider.SetActive(true);
            demoSliderLabel.SetActive(true);
            tableMug.SetActive(false);
        }
        else
        {
            dataEntry.SetActive(true);
        }
    }
    /*private void AddPrefixesToAllXmlPaths()
    {
        AddPrefixesToXmlPaths(FriendlyPrefix, FriendlyDialogues1);
        AddPrefixesToXmlPaths(FriendlyPrefix, FriendlyDialogues2);
        AddPrefixesToXmlPaths(NeutralPrefix, NeutralDialogues1);
        AddPrefixesToXmlPaths(NeutralPrefix, NeutralDialogues2);
        AddPrefixesToXmlPaths(UnfriendlyPrefix, UnfriendlyDialogues1);
        AddPrefixesToXmlPaths(UnfriendlyPrefix, UnfriendlyDialogues2);
    }*/

    private void AddPrefixesToXmlPaths(string prefix, List<GretaDelayWrapper.AnimationCommand> dialogues)
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            dialogues[i] = dialogues[i].AddPrefix(prefix);
        }
    }

    //Register user profile data into the CSV file (will be called from "Register" button of the UI after receiving all the user profile data)
    public void RegisterUserProfile()
    {
        Debug.Log("button pressed");

        Debug.Log("playerGender.value: " + playerGender.value);

        playerIdValue = playerId.GetComponentInChildren<TextMeshProUGUI>().text; 

        if (playerGender.value != 0 && playerIdValue.Length > 3)
        {
            //open two CSV files to save the trial results and save the user profile data, also to save the user trajectory data when joining the group
            OpenCSVFiles();

            //start the game
            if (gameStartedFlag)
            {
                dataEntry.SetActive(false);
                //TODO hide pointers
                TrialAgents[0].SetActive(false);
                TrialAgents[1].SetActive(false);
                tableMug.SetActive(true);
                StartGame();
            }
            //Debug.Log("");
        }
    }

    private void StartGame()
    {
        ReadRowToFetchForBalancedLatinSquare();

        Debug.Log("Making list");

        ExpeConditions = new List<string>();

        ExpeConditions.AddRange(CreateBalancedLatinSquareTrials());

        if(NumberOfDemoRounds > 0)
        {
            demo = true;
        }

        Initialization();
    }

    // Update is called once per frame
    private void Update()
    {
        if (flag)
        {
            //look at the participant
            agentHeads[0].targetObject = avatarHeadObject.transform;
            agentHeads[1].targetObject = avatarHeadObject.transform;

            //StartCoroutine(DelayedConversation(3.8f));

            agentHeads[0].targetObject = speakerAgentHeadObject.transform;
            agentHeads[1].targetObject = speakerAgentHeadObject.transform;

            flag = false;
        }

        if (flagAcknowledgeFriendly)
        {
            StartCoroutine(AcknowledgeUser(FRIENDLY_SUBMISSIVE_WAIT_GAZETIME, FriendlySubmssiveWaitCommand, NeutralWaitPartnerCommand));
            flagAcknowledgeFriendly = false;
        }

        if (flagAcknowledgeUnfriendly)
        {
            StartCoroutine(AcknowledgeUser(HOSTILE_DOMINANT_ACKNOWLEDGE_GAZETIME, HostileDominantCommand, NeutralWaitPartnerCommand));
            flagAcknowledgeUnfriendly = false;
        }

        if (gameStartedFlag && endTrialOnTriggerPress.activeSelf)
        {
            EndTrial();
            endTrialOnTriggerPress.SetActive(false);
        }

        timeForTrial += Time.deltaTime;

        if (!hitPspace)
        {
            timeForPspace += Time.deltaTime;
        }

        if (!hasWaved)
        {
            waveTimer += Time.deltaTime;
            
            if (waveTimer >= BeforeWaveTime)
            {
                hasWaved = true;

                foreach (GretaDelayWrapper wrapper in gretaDelayWrappers)
                {
                    wrapper.InteruptGretaAnimation(WaveCommand);
                }
            }
        }
    }

    private void EndTrial()
    {
        Debug.Log("Trial over");
        //set the flag to false in order to stop capturing the user trajectory
        Walking.captureTrajectoryFlag = false;
        trialOngoing = false;

        AudioListener.volume = 0.0f;

        gretaDelayWrappers[mainAgentId].ClearAnimationQueue();
        gretaDelayWrappers[lateralAgent1Id].ClearAnimationQueue();

        //show trial questionnaire to the user

        tableMug.SetActive(false); //hide the mug on the table
        trialHintsCanvas.SetActive(false); // hide trial hints canvas
                                            //pointer.SetActive(true);
        trialIdTextBoxEnd.text = trialStr;
        timeTextBoxEnd.text = timeForTrial.ToString("0.00");
        timePspaceEnd.text = timeForPspace.ToString("0.00");

        if (demo)
        {
            // give the user some trials to be familair with the environment only for two trials 
            if (trialIdNum >= NumberOfDemoRounds)
            {
                demo = false;
                trialIdNum = 0;
            }

            questionnaireDemo.SetActive(true);
            questionnaireObject1.SetActive(false);
        }

        endtrialUI.SetActive(true);
    }

    private void Initialization()
    {

        ValenceParticipants.value = 4;
        ValenceAgents.value = 4;
        DominanceValue.value = 4;
        FriendlinessOutValue.value = 4;
        FriendlinessInValue.value = 4;

        WheelValue = -1;
        UnselectSegment();

        NextButton.interactable = false;

        timeForTrial = 0;
        timeForPspace = 0;

        hitPspace = false;

        AudioListener.volume = AudioStartVolume;

        //set trial parameters
        //if (trialId < 10)
        if (trialIdNum < ExpeConditions.Count)
        {
            if (demo)
            {
                trialStr = DemoStr;
            }
            else if (ForceScenario == null || ForceScenario.Length == 0)
            {
                //New trial
                trialStr = ExpeConditions[trialIdNum];
            }
            else
            {
                trialStr = ForceScenario;
            }

            trialIdNum++;

            Debug.Log("TRIAAAAAALLLLLLLL STR = " + trialStr + ",       trial" + trialIdNum.ToString());


            //display trial number only when not in demo
            if (!demo)
                trialIdTextBox.text = trialIdNum.ToString() + "/" + (ExpeConditions.Count).ToString();
            else
                trialIdTextBox.text = "demo";


            Debug.Log("Set trial text");
            //set trial condition (trialAgentsDistanceID, trialConversationID, trialGazeID, trialEmbodiementID, trialFeedbackID)
            SetTrialCondition();
        }
        else
        {
            Debug.Log("game over ");
            //Game Over!
            gameStartedFlag = false;
            endMessageWindow.SetActive(true);
            CloseCSVFiles();
            return;
        }

        //set the agent's roles, position, orientation        
        AgentsGroupConfiguration();

        /* Set the Agents attention 
            * trialGazeID == 2 --> agents look at each other (attentionID = 2)
            * trialGazeID == 3 --> agents first look at the user (attentionID = 3), then to each other during conversation (attentionID = 2), and if user crosses the o-space again to the user (attentionID = 3)
            */
        SetAgentsAttention(trialGazeID);


        if (trialGazeID == A_U_A_GAZE)
        {
            Debug.Log("HERE" + trialGazeID);

            Debug.Log("trial behaviour" + trialBehaviorID);
            switch (trialBehaviorID)
            {
                case NEUTRAL_WAIT_VERBAL:
                    Debug.Log("trial behaviourA1");
                    StartCoroutine(AcknowledgeUser(NEUTRAL_WAIT_GAZETIME, NeutralWaitCommand, NeutralWaitPartnerCommand));
                    break;

                case FRIENDLY_SUBMISSIVE_WAIT_VERBAL:
                    Debug.Log("trial behaviourA2");
                    StartCoroutine(AcknowledgeUser(FRIENDLY_SUBMISSIVE_WAIT_GAZETIME, FriendlySubmssiveWaitCommand, NeutralWaitPartnerCommand));
                    break;

                case FRIENDLY_DOMINANT_WAIT_VERBAL:
                    Debug.Log("trial behaviourA3");
                    StartCoroutine(AcknowledgeUser(FRIENDLY_DOMINANT_WAIT_ACKNOWLEDGE_GAZETIME, FriendlyDominantWaitCommand, NeutralWaitPartnerCommand));
                    break;

                case HOSTILE_SUBMISSIVE_WAIT_VERBAL:
                    Debug.Log("trial behaviourA4");
                    StartCoroutine(AcknowledgeUser(HOSTILE_SUBMISSIVE_ACKNOWLEDGE_GAZETIME, HostileSubmssiveWaitCommand, NeutralWaitPartnerCommand));
                    break;

                case HOSTILE_DOMINANT_WAIT_VERBAL:
                    Debug.Log("trial behaviourA5");
                    StartCoroutine(AcknowledgeUser(HOSTILE_DOMINANT_ACKNOWLEDGE_GAZETIME, HostileDominantCommand, NeutralWaitPartnerCommand));
                    break;


                default:
                    StartCoroutine(AcknowledgeUser(FRIENDLY_SUBMISSIVE_WAIT_GAZETIME));
                    break;

            }
        }
        //start the conversation and ignore the user
        else
        {
            StartAgentsStaticAction(); 
        }
        //after looking to the user, look back to each other and start the conversation
        //trialGazeID == 3 --> agents first look at the user (attentionID = 3), then to each other during conversation (attentionID = 2), and if user crosses the o-space again to the user (attentionID = 3)
  
       
        //set the flag to true in order to start capturing the user trajectory
        Walking.captureTrajectoryFlag = true;
        trialOngoing = true;
    }

    IEnumerator DelayedConversation(float delayTime)
    {
        //acknowledge user presence by talking to the user
        //gretaDelayWrappers[mainAgentId].AddGretaAnimationWithMinDelay(0, "Examples/EmileProject/CoffeeCup/Acknowledge1");
        //gretaDelayWrappers[lateralAgent1Id].AddGretaAnimationWithMinDelay(0, "Examples/EmileProject/CoffeeCup/Acknowledge2");

        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //SetAgentsAttention(2); //attentionID == 2 --> agents look back at each other agter looking to the user
        //StartAgentsConversation();
    }

    private void SetTrialCondition()
    {
        try
        {
            Debug.Log("parsing: " + trialStr);

            int.TryParse(trialStr[0].ToString(), out trialStaticActionID);
            Debug.Log("parsing: " + trialStaticActionID);


            int.TryParse(trialStr[1].ToString(), out trialGazeID);
            Debug.Log("parsing: " + trialGazeID);
            int.TryParse(trialStr[2].ToString(), out trialBehaviorID);
            Debug.Log("parsing: " + trialBehaviorID);
            int.TryParse(trialStr[3].ToString(), out trialVerbalID);
            Debug.Log("parsing: " + trialVerbalID);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error parse:"+ e.Message);
        }
    }

    private void AssignAgentRoles()
    {
        /* with Greata it was not possible to deactivate the agent, 
         * since in the begining of each trial 
         * I was receving the previous dialogue from the recently activated agent! 
         * so instead I changed the X and Z axis of the extra agent to 4, -7 in order to make it inivisble!
        */

        /* agents 0 .. 3 --> female
         * agents 4 .. 7 --> male
         * agentGender = 4 if the playerGender = male to generate male agents
         * agentGenr = 0 if the playerGender = female to generate female agents
         */
        if (playerGender.options[playerGender.value].text == "Male")
        {
            Debug.Log("Male");
            agentsGender = 2;
            speakerAgentHeadObject = MaleSpeakerAgentHeadObject;
            //make female agents invisible
            agents[0].transform.position = new Vector3(4, 0, -7);
            agents[1].transform.position = new Vector3(4, 0, -7);
        }
        else
        {
            Debug.Log("feMale");
            agentsGender = 0;
            speakerAgentHeadObject = FemaleSpeakerAgentHeadObject;
            //make male agents invisible
            agents[2].transform.position = new Vector3(4, 0, -7);
            agents[3].transform.position = new Vector3(4, 0, -7);
        }

        mainAgentId = Random.Range(0, 2);

        lateralAgent1Id = 1 - mainAgentId;

        mainAgentId += agentsGender; //in order to apply to male or female agents
        lateralAgent1Id += agentsGender; //in order to apply to male or female agents

        //activate agents 
        //agents[mainAgentId].SetActive(true);
        //agents[lateralAgent1Id].SetActive(true);

        //Debug.Log("lateral 1: " + lateralAgent1Id.ToString() + ", Lateral 2: " + lateralAgent2Id.ToString());
    }

    //configure group of agents parameters in a face-to-face formation for group of two agents
    private void AgentsGroupConfiguration()
    {
        AssignAgentRoles();
        //hide one extra agent
        /* with Greata it was not possible to deactivate the agent, 
            * since in the begining of each trial 
            * I was receving the previous dialogue from the recently activated agent! 
            * so instead I changed the X and Z axis of the extra agent to 4, -7 in order to make it inivisble!
        */

        // set the position and orientation of 2 avtive agents
        /*for (int i = 0; i < 2; i++)
        {
            float x;

            //set the distance between agents based on the trialStr[0] from the Latin Square martrix
            x = Mathf.Cos((180.0f + angle[i]) * Mathf.PI / 180.0f) * AgentsDistance[trialAgentsDistanceID - 1];
            Vector3 pos = transform.position + new Vector3(x, 0, 0);

            //Debug.Log("pos - i: "+pos);

            if (i == 0)
            {
                agents[lateralAgent1Id].transform.position = pos;
                agents[lateralAgent1Id].transform.LookAt(transform.position);
                //Debug.Log("lateral agent: " + lateralAgent1Id);
            }
            else if (i == 1)
            {
                agents[mainAgentId].transform.position = pos;
                agents[mainAgentId].transform.LookAt(transform.position);
                //Debug.Log("main agent: " + mainAgentId);
            }
        } */


        // set the length of the "invisible group center wall object"
        //invisible group center wall object = AgentsDistance from the center of the group * 2 + 0.2 (size of the other half of the agents body)
        /*float ScaleX = AgentsDistance[trialAgentsDistanceID - 1] * 2 + 0.2f;
        invisibleGroupCenterWallObject.transform.localScale = new Vector3(ScaleX, 8, 0.3f);
        */

        //enable the group collider to be able to detect participant's distance less than 1.5 meters (radius of the capsule collider) from the group center

        myCollider.enabled = true;
        //set the group P-Space radius
        /* Since the AgentsDistance in programming = AgentsDistance in the VE / 2  + 0.20 
         * thus we need to do the opposite to find the agents distance in the VE and set as the P-Space radius
         * therefore: p-Space radius  = (AgentsDistance  in programming - 0.20 ) * 2
         */
        //float pSpaceRad = (AgentsDistance[trialAgentsDistanceID - 1] - 0.2f ) * 2;
        //myCollider.radius = pSpaceRad; 

    }

    private void SetAgentsAttention(int attentionID)
    {
        if (attentionID == OTHER_AGENT_GAZE) //Change the focus of the agents' heads to the each other
        {
            //Change the focus of secondary agent Head Look Controler to the speaker agenT
            agents[lateralAgent1Id].GetComponent<HeadLookController>().targetObject = speakerAgentHeadObject.transform;

            //Change the focus of speaker agent Head Look Controler to the second agent
            agents[mainAgentId].GetComponent<HeadLookController>().targetObject = speakerAgentHeadObject.transform;
        }
        else if (attentionID == USER_GAZE) //Change the focus of both agents Head Look Controler to the user
        {

            //look at the participant
            agents[lateralAgent1Id].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
            agents[mainAgentId].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
        }
        else if (attentionID == NO_GAZE)
        {
            //look at the painting on the wall
            agents[lateralAgent1Id].GetComponent<HeadLookController>().targetObject = paintingGazeObject.transform;
            agents[mainAgentId].GetComponent<HeadLookController>().targetObject = paintingGazeObject.transform;
        }
    }

    private void StartAgentsStaticAction(float delay = 0)
    {
        //start the static action between the agents   

        Debug.Log("Trial Conversation Id = " + trialStaticActionID);
        
        if (trialStaticActionID == NO_CONVERSATION)//no conversation mode: send the Rest gesture
        {
            gretaDelayWrappers[mainAgentId].AddGretaAnimationWithMinDelay(delay, RestCommand);
            gretaDelayWrappers[lateralAgent1Id].AddGretaAnimationWithMinDelay(delay, RestCommand);
        }
    }

    /*private IEnumerator StartGretaAnimation(int agentNo, float sec, string command)
    {
        Debug.Log("politeness strategy to play: "+command);

        yield return new WaitForSeconds(sec);
        

        agents[agentNo].GetComponent<GretaCharacterAnimator>().PlayAgentAnimation(command);
    }*/

 
    // Latin Square n x n
    private List<int> GenerateLatinSquare(int expeCondNo, int rows)
    {
        List<int> result = new List<int>();
        
        // Loop to print rows 
        for (int i = 1, j = 1; j <= expeCondNo && i <= rows; j++, i++)
        {
            // The  first loop will generate the first part of each row and the second loop will generate the rest of the number for that row.

            //first loop example for n = 5
            /* 1, 2, 3, 4, 5
               2, 3, 4, 5
               3, 4, 5
               3, 4, 5
               4, 5
               5
               then the second loop will start from 1 and generate the rest of numbers for each line  
            */
            
            // first loop
            int k = j;
            for (; k <= expeCondNo; k++)
            {
                result.Add(k);
                //Debug.Log(k.ToString()+" , ");
            }
            // second loop
            k = 1;
            for (; k <= j-1; k++)
            { 
                result.Add(k);
                //Debug.Log(k.ToString()+" , ");
            }
        }

        //Debug.Log("i1 = " + result[0].ToString()+ " , i2 =" + result[1].ToString() + " , i3 =" + result[2].ToString() + " , i4 =" + result[3].ToString() + " , i5 =" + result[4].ToString() + " , i6 =" + result[5].ToString());
        return result;
    }

    //open a CSV file for the Experiment
    private void OpenCSVFiles()
    {
        Debug.Log("playerGender.value: " + playerGender.value);
        
        if (playerIdValue != "") 
        {
            string datetimeSt = System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            //experiment file to save user profile and joinig point and questionnaire data
            SWexpe = new StreamWriter(".//ExpeResults//Expe-" + playerIdValue + "-" + datetimeSt + ".csv");
            //trajectory file to save user trajectory to join the group
            SWtrajectory = new StreamWriter(".//ExpeResults//Traj-" + playerIdValue + "-" + datetimeSt + ".csv");

            //save user profile data into expe file
            SavePlayerProfile2File(datetimeSt, SWexpe);
            //save user profile data into trajectory file
            SavePlayerProfile2File(datetimeSt, SWtrajectory);

            //save agents and group information to the trajectory file
            //SaveGroupData2TrajectoryFile();

            Debug.Log("FriendlinessInValue: " + FriendlinessInValue.value);
            Debug.Log("FriendlinessOutValue: " + FriendlinessOutValue.value);
            Debug.Log("DominaceValue: " + DominanceValue.value);
            Debug.Log("ValenceAgentsValue: " + ValenceAgents.value);
            Debug.Log("ValenceParticipantsValue: " + ValenceParticipants.value);
            Debug.Log("WheelValue: " + WheelValue);
            //create a header for the user results 
            SWexpe.WriteLine("TRIAL,PLAYERID,GENDER,CONDITION,SECONDARY AGENT,MAIN AGENT,TIME TRIAL,TIME P,ValenceParticipantsValue,ValenceAgentsValue,DominaceValue,FriendlinessInValue,FriendlinessOutValue,CircumplexValue");

            gameStartedFlag = true;
        }
    }

    //close the CSV files of the Experiment and trajectory
    private void CloseCSVFiles()
    {
        string endDatetimeSt = System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");

        // close the CSV file
        try
        {
            SWexpe.WriteLine("EXPERIMENT END" + "," + endDatetimeSt);
            SWtrajectory.WriteLine("EXPERIMENT END" + "," + endDatetimeSt);

            if (SWexpe.BaseStream != null)
                SWexpe.Close();

            if (SWtrajectory.BaseStream != null)
                SWtrajectory.Close();
        }
        catch (System.Exception e)
        {
            Debug.Log("No experiment file has created! " + e.ToString());
        }
    }

    //save player profile data into the CSV file 
    private void SavePlayerProfile2File(string dateTime, StreamWriter sw)
    {
        /*Debug.Log(playerGender.options[playerGender.value].text);
            Debug.Log(playerAge.text);
            Debug.Log(playerOrigin.options[playerOrigin.value].text);
            Debug.Log(playerResidence.options[playerResidence.value].text);
            Debug.Log(playerHandedness.options[playerHandedness.value].text);*/

        //save user profile data
        sw.WriteLine("PLAYER ID" + "," + playerIdValue);
        //sw.WriteLine("PLAYER GENDER" + "," + playerGender.options[playerGender.value].text);
        sw.WriteLine("PLAYER GENDER" + "," + playerGender.value);

        /*
        sw.WriteLine("PLAYER AGE" + "," + playerAge.GetComponentInChildren<TextMeshProUGUI>().text);
        sw.WriteLine("PLAYER NATIONALITY" + "," + playerOrigin.value);
        sw.WriteLine("PLAYER RESIDENCE" + "," + playerResidence.value);
        sw.WriteLine("PLAYER HANDEDNESS" + "," + playerHandedness.value);
        sw.WriteLine("PLAYER ENGLISH" + "," + playerEnglishLevel.value);
        sw.WriteLine("PLAYER INTELLIGENT SYSTEMS FAMILIARITY" + "," + playerArtificialSysKnow.value);
        */
        sw.WriteLine("EXPERIMENT START" + "," + dateTime);
        sw.WriteLine("-");
        sw.WriteLine("TRIAL DATA");
        sw.WriteLine("-");

        Debug.Log("Profile SAVEDD!");
    }

    //save agents and group position and rotation and forward dirction into the trajectory file
    void SaveGroupData2TrajectoryFile()
    {
        SWtrajectory.WriteLine("TRIAL," + trialIdNum);
        SWtrajectory.WriteLine("Condition ID," + trialIdNum.ToString());

        SWtrajectory.WriteLine("LATERAL(POS X)," + agents[lateralAgent1Id].transform.localPosition.x);
        SWtrajectory.WriteLine("LATERAL(POS Y),0");
        SWtrajectory.WriteLine("LATERAL(POS Z)," + agents[lateralAgent1Id].transform.localPosition.z);
        SWtrajectory.WriteLine("LATERAL(ROT X),0");
        SWtrajectory.WriteLine("LATERAL(ROT Y)," + agents[lateralAgent1Id].transform.localRotation.y);
        SWtrajectory.WriteLine("LATERAL(ROT Z),0");
        SWtrajectory.WriteLine("LATERAL(FORWARD X)," + agents[lateralAgent1Id].transform.forward.x);
        SWtrajectory.WriteLine("LATERAL(FORWARD Y)," + agents[lateralAgent1Id].transform.forward.y);
        SWtrajectory.WriteLine("LATERAL(FORWARD Z)," + agents[lateralAgent1Id].transform.forward.z);

        SWtrajectory.WriteLine("MAIN(POS X)," + agents[mainAgentId].transform.localPosition.x);
        SWtrajectory.WriteLine("MAIN(POS Y),0");
        SWtrajectory.WriteLine("MAIN(POS Z)," + agents[mainAgentId].transform.localPosition.z);
        SWtrajectory.WriteLine("MAIN(ROT X),0");
        SWtrajectory.WriteLine("MAIN(ROT Y)," + agents[mainAgentId].transform.localRotation.y);
        SWtrajectory.WriteLine("MAIN(ROT Z),0");
        SWtrajectory.WriteLine("MAIN(FORWARD X)," + agents[mainAgentId].transform.forward.x);
        SWtrajectory.WriteLine("MAIN(FORWARD Y)," + agents[mainAgentId].transform.forward.y);
        SWtrajectory.WriteLine("MAIN(FORWARD Z)," + agents[mainAgentId].transform.forward.z);

        SWtrajectory.WriteLine("GROUP(POS X)," + transform.position.x);
        SWtrajectory.WriteLine("GROUP(POS Y),0");
        SWtrajectory.WriteLine("GROUP(POS Z)," + transform.position.z);

        SWtrajectory.WriteLine("-");
        SWtrajectory.WriteLine("TRAJECTORY DATA");
        SWtrajectory.WriteLine("-");
        
        //create a header for the user trajectory file
        SWtrajectory.WriteLine("PLAYER Pos X,PLAYER Pos Y,PLAYER Pos Z,PLAYER Rot X,PLAYER Rot Y,PLAYER Rot Z,PLAYER Forward X,PLAYER Forward Y,PLAYER Forward Z");
    }

    //Show circumplex question
    public void ShowNext()
    {
        questionnaireDemo.SetActive(false);
        questionnaireObject1.SetActive(false);
        questionnaireObject2.SetActive(true);
    }

    //save each trial values into the CSV files (joining points + questionnaire + trajectory) called from submit button on the questionnaire UI form
    public void DemoSubmit()
    {
        //reinitializing all the lists related to the trajectory
        Walking.userPosX.Clear();
        Walking.userPosY.Clear();
        Walking.userPosZ.Clear();
        Walking.userRotX.Clear();
        Walking.userRotY.Clear();
        Walking.userRotZ.Clear();
        Walking.userForwardX.Clear();
        Walking.userForwardY.Clear();
        Walking.userForwardZ.Clear();

        //Disactivate the questionnaire form and the left controller pointer and show the coffee cup on table again
        endtrialUI.SetActive(false);
        questionnaireDemo.SetActive(false);
        questionnaireObject1.SetActive(true);
        questionnaireObject2.SetActive(false);
        tableMug.SetActive(true); //show the mug on the table
        trialHintsCanvas.SetActive(true); // show trial hints canvas
                                          //if (trialId != 18) pointer.SetActive(false);

        //reload the next trial
        Initialization();
    }


    //save each trial values into the CSV files (joining points + questionnaire + trajectory) called from submit button on the questionnaire UI form
    public void SaveTrialData2File()
    {
        /*
        if (demo)
        {
            //reload the next trial
            Initialization();
        }
        */
        //Agent ID should be increased by 1 since the agent ID starts from 0
        int lateralAg1 = lateralAgent1Id + 1;
        int mainAg = mainAgentId + 1;

        Debug.Log("WheelValue: " + WheelValue);
        Debug.Log("FriendlinessOutValue: " + FriendlinessOutValue.value);
        Debug.Log("FriendlinessInValue: " + FriendlinessInValue.value);
        Debug.Log("DominaceValue: " + DominanceValue.value);
        Debug.Log("ValenceAgentsValue: " + ValenceAgents.value);
        Debug.Log("ValenceParticipantsValue: " + ValenceParticipants.value);


        timeForPspace = hitPspace ? timeForPspace : -1;

        //SWexpe.WriteLine("TRIAL,PLAYERID,GENDER,CONDITION,SECONDARY AGENT,MAIN AGENT,TIME TRIAL,TIME P,ValenceParticipantsValue,ValenceAgentsValue,DominaceValue,FriendlinessInValue,FriendlinessOutValue,CircumplexValue"); 
        //save experiment joinig point and questionnaire data to file
        SWexpe.WriteLine(trialIdNum + "," + playerIdValue + "," + playerGender.value + "," + trialStr + "," + lateralAg1 + "," + mainAg + "," + timeForTrial + "," + timeForPspace +","+ ValenceParticipants.value + "," + ValenceAgents.value + "," + DominanceValue.value + "," + FriendlinessOutValue.value + "," + FriendlinessInValue.value + "," + WheelValue);

        //save agents and group information into the trajectory file
        SaveGroupData2TrajectoryFile();
        //save trajectory data to file
        for (int i = 0; i < Walking.userPosX.Count; i++)
            SWtrajectory.WriteLine(Walking.userPosX[i] + "," + Walking.userPosY[i] + "," + Walking.userPosZ[i] + "," + Walking.userRotX[i] + "," + Walking.userRotY[i] + "," + Walking.userRotZ[i] + "," + Walking.userForwardX[i] + "," + Walking.userForwardY[i] + "," + Walking.userForwardZ[i]);

        //reinitializing all the lists related to the trajectory
        Walking.userPosX.Clear();
        Walking.userPosY.Clear();
        Walking.userPosZ.Clear();
        Walking.userRotX.Clear();
        Walking.userRotY.Clear();
        Walking.userRotZ.Clear();
        Walking.userForwardX.Clear();
        Walking.userForwardY.Clear();
        Walking.userForwardZ.Clear();

        //Disactivate the questionnaire form and the left controller pointer and show the coffee cup on table again
        endtrialUI.SetActive(false);
        questionnaireObject1.SetActive(true);
        questionnaireObject2.SetActive(false);
        tableMug.SetActive(true); //show the mug on the table
        trialHintsCanvas.SetActive(true); // show trial hints canvas
        //if (trialId != 18) pointer.SetActive(false);
            
        //reload the next trial
        Initialization();
    }

    void OnApplicationQuit()
    {
        // close the CSV file
        Application.Quit();
    }

    /* Read the list of conditions then shuffel them */
    private List<string> CreateRandomisedListOfConditions()
    {
        List<string> result = null;

        try
        {
            Debug.Log("beat1 ");
            TextAsset conditionData = Resources.Load<TextAsset>("Conditions");
            Debug.Log("beat2");

            //the interesting row to be repeated
            string[] conditionArray = conditionData.text.TrimEnd('\r', '\n').Split(new char[] { ',' });

            Debug.Log("beat3");

            Debug.Log("Condition Data: " + conditionData.text);

            result = FisherShuffle(new List<string>(conditionArray));

            Debug.Log($"[{string.Join(",", result)}]");

        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        return result;
    }

    public static List<string> FisherShuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }


    /* provide the latin square for a 7-trial experiment. It will open the "Assets\Resources\BalancedLatinSquare.csv" file and read the current row based on the "latinSquareRowToFetch". 
    * Then it will repeat the fetched row based on the "expCondRepeatNo" and create the trials of the experiment. 
    * */
    private List<string> CreateBalancedLatinSquareTrials()
    {
        List<string> result = new List<string>();

        try
        {
            TextAsset balancedLatinSquareData = Resources.Load<TextAsset>("BalancedLatinSquare");

            string[] data = balancedLatinSquareData.text.Split(new char[] { '\n' });

            //repeat one row from the latin square to "expCondRepeatNo" times to create the trials
            for (int i = 0; i < expCondRepeatNo; i++)
            {
                //the interesting row to be repeated
                string[] row = data[latinSquareRowToFetch].TrimEnd('\r', '\n').Split(new char[] { ',' });

                //Debug.Log("rowdata: " + data[i].ToString());

                //adding the values of the interesting row to the result list
                for (int j = 0; j < row.Length; j++)
                {
                    string value = row[j];                    
                    result.Add(value);

                    //Debug.Log("value " + (i*j).ToString() + " : " + value.ToString());
                }
            }

        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        return result;
    }


    // called from the submit button from the endmessage canvas in order to save the user comments and terminate the game
    public void GameOver()
    {
        /*
        //save the user comment into the experiment CSV file 
        SWexpe.WriteLine("-");
        SWexpe.WriteLine("PLAYER COMMENTS" + "," + playerComments.GetComponentInChildren<TextMeshProUGUI>().text);
        SWexpe.WriteLine("-");

        //close the CSV files
        CloseCSVFiles();
        */

        //Debug.Log("GAMEOVER CALLED");
        Application.Quit();
    }

    //read the row number that should be fetched from the balanced latin square file
    private void ReadRowToFetchForBalancedLatinSquare()
    {
        if (demo)
            latinSquareRowToFetch = 0;
        else
        {
            //read the previous row that has been fetched
            StreamReader stRead = new StreamReader(".//Assets//Resources//RowToRead.csv");
            string line = stRead.ReadLine();
            int.TryParse(line, out latinSquareRowToFetch);
            Debug.Log("latin Square Row To Fetch: " + latinSquareRowToFetch);
            stRead.Close();

            SWexpe.WriteLine("LATIN SQUARE ROW" + "," + latinSquareRowToFetch);
            SWtrajectory.WriteLine("LATIN SQUARE ROW" + "," + latinSquareRowToFetch);

            //increase the row number to use it in the current experiment and save it to know what row has been used (it is a 7 row matrix so the number should be between 0-6)
            StreamWriter stWrite = new StreamWriter(".//Assets//Resources//RowToRead.csv");
            latinSquareRowToFetch++;
            latinSquareRowToFetch %= 7;
            stWrite.WriteLine(latinSquareRowToFetch);
            stWrite.Close();


        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //avatarCollision = true;
        //myCollider.radius = 10f; // or whatever radius you want.

        Debug.Log("hi! " + col.gameObject.name + " entered to : " + gameObject.tag + " myCollider.radius = " + myCollider.radius.ToString());

        if ( trialOngoing && col.gameObject.tag == "UserHead")
        {

        }
    }

    private void OnTriggerExit(Collider col)
    {
        //avatarCollision = false;
        Debug.Log(col.gameObject.name + " exited the : " + gameObject.name);
    }

    private IEnumerator AcknowledgeUser(float gazeTime, GretaDelayWrapper.AnimationCommand activeCommand, GretaDelayWrapper.AnimationCommand partnerCommand)
    {
        //look at the participant
        agentHeads[lateralAgent1Id].targetObject = avatarHeadObject.transform;
        agentHeads[mainAgentId].targetObject = avatarHeadObject.transform;

        gretaDelayWrappers[mainAgentId].InteruptGretaAnimation(activeCommand);
        gretaDelayWrappers[lateralAgent1Id].InteruptGretaAnimation(partnerCommand);

        yield return new WaitForSeconds(gazeTime);

        gretaDelayWrappers[mainAgentId].AddGretaAnimationWithMinDelay(gazeTime, partnerCommand);
        gretaDelayWrappers[lateralAgent1Id].AddGretaAnimationWithMinDelay(gazeTime, partnerCommand);

        agentHeads[lateralAgent1Id].targetObject = speakerAgentHeadObject.transform;
        agentHeads[mainAgentId].targetObject = speakerAgentHeadObject.transform;
    }

    private IEnumerator AcknowledgeUser(float gazeTime)
    {
        //look at the participant
        agentHeads[lateralAgent1Id].targetObject = avatarHeadObject.transform;
        agentHeads[mainAgentId].targetObject = avatarHeadObject.transform;

        yield return new WaitForSeconds(gazeTime);

        agentHeads[lateralAgent1Id].targetObject = speakerAgentHeadObject.transform;
        agentHeads[mainAgentId].targetObject = speakerAgentHeadObject.transform;
    }

    public void EnteredPspace()
    {
        hitPspace = true;
    }

    public void UnselectSegment()
    {
        if (selectedSegmentButton != null)
        {
            selectedSegmentButton.thisButton.interactable = true;
            selectedSegmentButton = null;
        }
    }
}
