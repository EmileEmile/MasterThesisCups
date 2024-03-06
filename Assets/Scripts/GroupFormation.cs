using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class GroupFormation : MonoBehaviour {
    bool flag, flag2, flag3 = true;

    [Header("Agent Info")]
    // Instantiates prefabs in a circle formation
    public GameObject[] agents;

    public GameObject[] TrialAgents;

    public GameObject[] agentsGaze;

    public GameObject speakerAgentHeadObject; // center of the group or speaking agent
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
    public GameObject trialTableMug;
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
    public bool demo;
    public int experimentConditionsNo;//numebr of all unique conditions of a variable (eg. 4 distnaces between agents)
    public int expCondRepeatNo;//number of repetition of conditions in each experiment (eg. 2, will create 8 trials)    
    public int experimentBlocksNo;//numebr of blocks that we want to have in an experiment (eg. 2, it will create 8 trials) 
    //indicates the row to be fetched in the balanced latin square csv file to create the trials (ex. 0..3)
    //it will be automatically fetched by the ReadRowToFetchForBalancedLatinSquare function from a CSV file
    [Range(0, 3)]
    int latinSquareRowToFetch;


    int trialId = 0;

    List<string> ExpeConditions; //list of all trial of the experiment provided by latin square method (each value is a string that contains several variables' values: table and group distance, gaze,... check latin square help file for details)
    /*
        *
        * Condition	1. Distance between agents (m)	2. Conversation between agents	3. Gaze direction           4. Embodiement	   5. Behavior                  Verbal of the passive agent
        * 1	        0.4 (1)	                            No (1)	                    No Gaze (1)	                Cylinder (1)	   Ignore the user (1)	
        * 2	        0.7 (2)	                            No (1)	                    No Gaze (1)	                Cylinder (1)	   Ignore the user (1)
        * 3	        1 (3)	                            No (1)	                    No Gaze (1)	                Cylinder (1)	   Ignore the user (1)		
        * 4	        1.3 (4)	                            No (1)	                    No Gaze (1)	                Cylinder (1)	   Ignore the user (1)
        * 5	        0.4 (1)	                            Yes (2)	                    Other agent (2)	            Agent (2)	       Ignore the user (1)	
        * 6	        0.7 (2)	                            Yes (2)	                    Other agent (2)	            Agent (2)	       Ignore the user (1)	
        * 7	        1 (3)	                            Yes (2)	                    Other agent (2)	            Agent (2)	       Ignore the user (1)	
        * 8	        1.3 (4)	                            Yes (2)	                    Other agent (2)	            Agent (2)	       Ignore the user (1)	
        * 9	        0.4 (1)	                            Yes (2)	                    User-Other agent-User (3)	Agent (2)	       Acknowledge the user (2)		Hi, feel free to take the cup of coffee.
        * 10	    0.7 (2)	                            Yes (2)	                    User-Other agent-User (3)	Agent (2)	       Acknowledge the user (2)		Hi, feel free to take the cup of coffee.
        * 11	    1 (3)	                            Yes (2)	                    User-Other agent-User (3)	Agent (2)	       Acknowledge the user (2)		Hi, feel free to take the cup of coffee.
        * 12	    1.3 (4)	                            Yes (2)	                    User-Other agent-User (3)	Agent (2)	       Acknowledge the user (2)		Hi, feel free to take the cup of coffee.
        */
    string trialStr;//it includes all the values for one experiment condition (trial) (table and group distance, gaze,... check latin square help file for details).
    int trialAgentsDistanceID = 0;
    int trialConversationID = 0;
    int trialGazeID = 0;
    int trialBehaviorID = 0;
    

    //CSV files parameters
    private StreamWriter SWexpe;
    private StreamWriter SWtrajectory;

    [Header("Hints Window")]
    public GameObject hintsWindow;
    public TMP_Text trialIdTextBox; //the text box to show the trial id
    [Space(20)]

    [Header("User Data Entry")]
    //User profile data entry fields
    public GameObject dataEntry;
    public Button playerId; //the user who is palying with the avatar
    public TMP_Dropdown playerGender;
    public Button playerAge;
    public TMP_Dropdown playerOrigin;
    public TMP_Dropdown playerResidence;
    public TMP_Dropdown playerHandedness;
    public TMP_Dropdown playerEnglishLevel;
    public TMP_Dropdown playerArtificialSysKnow;
    //canvas elements to be active only during the demo
    public GameObject demoSlider = null;
    public GameObject demoSliderLabel = null;

    [Space(20)]


    [Header("Questoinaire Data")]
    //Questionnaire data entry fields
    public GameObject questionnaireObject;
    public Slider ClarityValue;
    public Slider FaceLossValue;
    public Slider PositiveFaceValue;
    public Slider NegativeFaceValue;
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

    // Use this for initialization
    private void Start () {
        myCollider = transform.GetComponent<CapsuleCollider>();

        agents[2].GetComponent<HeadLookController>().targetObject = agentsGaze[2].transform;
        agents[3].GetComponent<HeadLookController>().targetObject = agentsGaze[3].transform;
  


        //look at the participant
        agents[0].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
        agents[1].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
        TrialAgents[0].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
        TrialAgents[1].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;



        endMessageWindow.SetActive(false);
        
        trialHintsCanvas.SetActive(true);

        //display demo slider on data collection canvas only during demo
        if (demo) 
        {
            demoSlider.SetActive(true);
            demoSliderLabel.SetActive(true);
            tableMug.SetActive(false);
        }
        else
        {
            dataEntry.SetActive(false);
            trialTableMug.SetActive(false);
        }
    }

    //Register user profile data into the CSV file (will be called from "Register" button of the UI after receiving all the user profile data)
    public void RegisterUserProfile()
    {
        Debug.Log("button pressed");
        
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
            trialTableMug.SetActive(false);
            StartGame();
        }
        //Debug.Log("");
    }

    private void StartGame()
    {
        ReadRowToFetchForBalancedLatinSquare();
        // Generate trial paths based on Latin square method and the number of experiment conditions 
        //ExpeConditions = GenerateLatinSquare(experimentConditionsNo, experimentBlocksNo);
        // Generate trial paths based on Balanced Latin square method for 12 conditions
        ExpeConditions = CreateBalancedLatinSquareTrials();

        Initialization();
    }

    // Update is called once per frame
    private void Update()
    {
        if (flag)
        {
            //look at the participant
            agents[0].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
            agents[1].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;

            StartCoroutine(DelayedConversation(3.8f));

            agents[0].GetComponent<HeadLookController>().targetObject = agentsGaze[0].transform;
            agents[1].GetComponent<HeadLookController>().targetObject = agentsGaze[1].transform;

            StartCoroutine(StartDiscussion(1, 0, "Examples/DemoEN/CoffeeCup/Fake1Ver2"));

            flag = false;
        }

        if (flag2)
        {
            //look at the participant
            agents[0].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
            agents[1].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;

            StartCoroutine(DelayedConversation(3.8f));

            agents[0].GetComponent<HeadLookController>().targetObject = agentsGaze[0].transform;
            agents[1].GetComponent<HeadLookController>().targetObject = agentsGaze[1].transform;

            StartCoroutine(StartDiscussion(1, 0, "Examples/DemoEN/CoffeeCup/Fake1Ver2"));

            flag2 = false;
        }

        if (flag3)
        {
            //look at the participant
            agents[0].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
            agents[1].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;

            StartCoroutine(DelayedConversation(3.8f));

            agents[0].GetComponent<HeadLookController>().targetObject = agentsGaze[0].transform;
            agents[1].GetComponent<HeadLookController>().targetObject = agentsGaze[1].transform;

            StartCoroutine(StartDiscussion(1, 0, "Examples/DemoEN/CoffeeCup/Fake1Ver2"));

            flag3 = false;
        }

        if (gameStartedFlag)
        {
            //trial end
            if (endTrialOnTriggerPress.activeSelf)
            {
                Debug.Log("Trial over");
                //set the flag to false in order to stop capturing the user trajectory
                Walking.captureTrajectoryFlag = false;

                //show trial questionnaire to the user
                if (!demo)
                {
                    tableMug.SetActive(false); //hide the mug on the table
                    trialHintsCanvas.SetActive(false); // hide trial hints canvas
                    collisionDetection.cupGrabbedFlag = false; //set the cup grabbed flag of the collision script to false
                    questionnaireObject.SetActive(true);

                    //StopCoroutine(StartDiscussion(mainAgentId, 0, "Examples/DemoEN/CoffeeCup/FakeTalk1"));
                    //StopCoroutine(StartDiscussion(lateralAgent1Id, 0, "Examples/DemoEN/CoffeeCup/FakeTalk2"));
                }
                else if (trialId < 1) // give the user a two trials to be familair with the environment only for two trials 
                {
                    //pointer.SetActive(true);
                    //questionnaireObject.SetActive(true);
                    Initialization();
                } 

                //save the current trial values in the CSV file
                //SaveTrialData2File();

                //reload the next trial
                //Initialization();

                endTrialOnTriggerPress.SetActive(false);
            }
        }
    }

    private void Initialization()
    {
        //set questionnaire sliders to their defualt values (4 for a 7 value liker scale)
        ClarityValue.value = 4;
        FaceLossValue.value = 4;
        PositiveFaceValue.value = 4;
        NegativeFaceValue.value = 4;

        //set trial parameters
        //if (trialId < 10)
        if (trialId < ExpeConditions.Count)
        {
            //New trial
            trialStr = ExpeConditions[trialId];
            Debug.Log("TRIAAAAAALLLLLLLL STR = " + trialStr + ",       trial" + trialId.ToString());
            trialId++;


            //display trial number only when not in demo
            if (!demo)
                trialIdTextBox.text = trialId.ToString() + "/" + (expCondRepeatNo * experimentConditionsNo).ToString();
            else
                trialIdTextBox.text = "demo";

            //set trial condition (trialAgentsDistanceID, trialConversationID, trialGazeID, trialEmbodiementID, trialFeedbackID)
            SetTrialCondition();
        }
        else
        {
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


        //start the conversation
        if (trialConversationID == 2)
        {
            //start the conversation and ignore the user
            if (trialGazeID == 2)
            {
                StartAgentsConversation(); 
            }
            //after looking to the user, look back to each other and start the conversation
            //trialGazeID == 3 --> agents first look at the user (attentionID = 3), then to each other during conversation (attentionID = 2), and if user crosses the o-space again to the user (attentionID = 3)
            else if (trialGazeID == 3)
            {
                StartCoroutine(DelayedConversation(3.8f));
            }
        }

        //set the flag to true in order to start capturing the user trajectory
        Walking.captureTrajectoryFlag = true;

    }

    IEnumerator DelayedConversation(float delayTime)
    {
        //acknowledge user presence by talking to the user
        //StartCoroutine(StartDiscussion(mainAgentId, 0, "Examples/DemoEN/CoffeeCup/Acknowledge1"));
        StartCoroutine(StartDiscussion(lateralAgent1Id, 0, "Examples/DemoEN/CoffeeCup/Acknowledge2"));

        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //SetAgentsAttention(2); //attentionID == 2 --> agents look back at each other agter looking to the user
        //StartAgentsConversation();
    }

    private void SetTrialCondition()
    {
        try
        {
            int.TryParse(trialStr[0].ToString(), out trialAgentsDistanceID);
            int.TryParse(trialStr[1].ToString(), out trialConversationID);
            int.TryParse(trialStr[2].ToString(), out trialGazeID); 
            int.TryParse(trialStr[4].ToString(), out trialBehaviorID);
        }
        catch (System.Exception e)
        {
            print("Error:"+ e.Message);
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
            //make female agents invisible
            agents[0].transform.position = new Vector3(4, 0, -7);
            agents[1].transform.position = new Vector3(4, 0, -7);
        }
        else
        {
            Debug.Log("feMale");
            agentsGender = 0;
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

    //attentionID == 2 --> agents look at each other
    //attentionID == 3 --> passive agent looks at user
    private void SetAgentsAttention(int attentionID) //, int agentID1toChangeGaze, int agentID2toChangeGaze)
    {
        if (attentionID == 2) //Change the focus of the agents' heads to the each other
        {
            //Change the focus of secondary agent Head Look Controler to the speaker agent
            speakerAgentHeadObject.transform.position = new Vector3(agents[mainAgentId].transform.position.x, speakerAgentHeadObject.transform.position.y, agents[mainAgentId].transform.position.z);
            agents[lateralAgent1Id].GetComponent<HeadLookController>().targetObject = speakerAgentHeadObject.transform;

            //Change the focus of speaker agent Head Look Controler to the second agent
            addresseeAgentHeadObject.transform.position = new Vector3(agents[lateralAgent1Id].transform.position.x, addresseeAgentHeadObject.transform.position.y, agents[lateralAgent1Id].transform.position.z);
            agents[mainAgentId].GetComponent<HeadLookController>().targetObject = addresseeAgentHeadObject.transform;
        }
        else if (attentionID == 3) //Change the focus of both agents Head Look Controler to the user
        {
            //look at the painting on the wall
            //agents[lateralAgent1Id].GetComponent<HeadLookController>().targetObject = paintingGazeObject.transform;
            //agents[mainAgentId].GetComponent<HeadLookController>().targetObject = paintingGazeObject.transform;

            //look at the participant
            agents[lateralAgent1Id].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
            agents[mainAgentId].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
        }
    }

    private void StartAgentsConversation()
    {
        //start the main conversation    

        if (trialConversationID == 2) // conversation mode: send the fake concersations
        {
            StartCoroutine(StartDiscussion(mainAgentId, 0, "Examples/DemoEN/CoffeeCup/Fake1Ver2"));
            //StartCoroutine(StartDiscussion(mainAgentId, 0, "Examples/DemoEN/CoffeeCup/FakeTalk1"));
            //StartCoroutine(StartDiscussion(lateralAgent1Id, 0, "Examples/DemoEN/CoffeeCup/FakeTalk2"));
        }
        else //no conversation mode: send the Rest gesture  
        {
            StartCoroutine(StartDiscussion(mainAgentId, 0, "Examples/DemoEN/CoffeeCup/Rest"));
            StartCoroutine(StartDiscussion(lateralAgent1Id, 0, "Examples/DemoEN/CoffeeCup/Rest"));
        }
    }

    private IEnumerator StartDiscussion(int agentNo, float sec, string command)
    {
        Debug.Log("politeness strategy to play: "+command);

        yield return new WaitForSeconds(sec);
        
        agents[agentNo].GetComponent<GretaCharacterAnimator>().PlayAgentAnimation(command);
    }

 
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
        
        if (playerId.GetComponentInChildren<TextMeshProUGUI>().text != "") 
        {
            string datetimeSt = System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            //experiment file to save user profile and joinig point and questionnaire data
            SWexpe = new StreamWriter(".//ExpeResults//Expe-" + playerId.GetComponentInChildren<TextMeshProUGUI>().text + "-" + datetimeSt + ".csv");
            //trajectory file to save user trajectory to join the group
            SWtrajectory = new StreamWriter(".//ExpeResults//Traj-" + playerId.GetComponentInChildren<TextMeshProUGUI>().text + "-" + datetimeSt + ".csv");

            //save user profile data into expe file
            SavePlayerProfile2File(datetimeSt, SWexpe);
            //save user profile data into trajectory file
            SavePlayerProfile2File(datetimeSt, SWtrajectory);

            //save agents and group information to the trajectory file
            //SaveGroupData2TrajectoryFile();

            //create a header for the user results 
            SWexpe.WriteLine("TRIAL,CONDITION,SECONDARY AGENT,MAIN AGENT,JOIN 1,FINAL JOIN,FAR,CLARITY,FACE LOSS,POSITIVE FACE,NEGATIVE FACE");
                
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
        sw.WriteLine("PLAYER ID" + "," + playerId.GetComponentInChildren<TextMeshProUGUI>().text);
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
    }

    //save agents and group position and rotation and forward dirction into the trajectory file
    void SaveGroupData2TrajectoryFile()
    {
        SWtrajectory.WriteLine("TRIAL," + trialId);
        SWtrajectory.WriteLine("Condition ID," + trialId.ToString());

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

        /*Debug.Log("politenessVerbalValue: " + politenessVerbalValue.value);
        Debug.Log("politenessGestureValue: " + politenessGestureValue.value);
        Debug.Log("friendlyValue" + friendlyValue.value);
        Debug.Log("similarityValue" + similarityValue.value);
        Debug.Log("changeMindValue" + changeMindValue.value);*/

        //save experiment joinig point and questionnaire data to file
        SWexpe.WriteLine(trialId + "," /*+ conditionID*/ + "," + lateralAg1 + "," + mainAg + "," + "No Join" + "," + "No Join" + "," + "No Join" + "," + ClarityValue.value + "," + FaceLossValue.value + "," + PositiveFaceValue.value + "," + NegativeFaceValue.value);

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
        questionnaireObject.SetActive(false);
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

    public void startButtonClick()
    {
        hintsWindow.SetActive(false);
    }

    /* provide the latin square for a 6-trial experiment. It will open the "Assets\Resources\BalancedLatinSquare.csv" file and read the current row based on the "latinSquareRowToFetch". 
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
                string[] row = data[latinSquareRowToFetch].Split(new char[] { ',' });

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

            //increase the row number to use it in the current experiment and save it to know what row has been used (it is a 9 row matrix so the number should be between 0-8)
            StreamWriter stWrite = new StreamWriter(".//Assets//Resources//RowToRead.csv");
            latinSquareRowToFetch++;
            latinSquareRowToFetch %= 9;
            stWrite.WriteLine(latinSquareRowToFetch);
            stWrite.Close();

            SWexpe.WriteLine("LATIN SQUARE ROW" + "," + latinSquareRowToFetch);
            SWtrajectory.WriteLine("LATIN SQUARE ROW" + "," + latinSquareRowToFetch);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //colObject = col;
        //avatarCollision = true;

        CapsuleCollider myCollider = transform.GetComponent<CapsuleCollider>();
        //myCollider.radius = 10f; // or whatever radius you want.

        Debug.Log(col.gameObject.name + " entered to : " + gameObject.name + " myCollider.radius = " + myCollider.radius.ToString());

        /*
        if (col.gameObject.name == avatarName)
        {
            //avatar is far from the the center of the group
            if (collisionNo < 6 && NoActiveAgents == 3)
            {
                //Start a fake conversation among the agents
                FakeConversationManager();
            }
            else if (collisionNo == 6 && NoActiveAgents == 3) //avatar is inside the group radius for talking (P-Space)
            {
                //if (NoActiveAgents == 3) {
                //Change the focus of Head Look Controler to the avatar
                agents[0 + agentsGender].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
                agents[1 + agentsGender].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
                agents[2 + agentsGender].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
                agents[3 + agentsGender].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;

                //start the main trial and disscussion
                StartCoroutine(StartDiscussion(mainAgentId, 0, "Examples/DemoEN/Politeness/Politeness Startegy " + PoliteStgNo.ToString()));
                StartCoroutine(StartDiscussion(lateralAgent1Id, 4.8f, "Examples/DemoEN/Politeness/Lateral 1"));
                StartCoroutine(StartDiscussion(lateralAgent2Id, 5.6f, "Examples/DemoEN/Politeness/Lateral 2"));
                /*}
                else if (NoActiveAgents == 2)
                {
                    //Change the focus of main agent Head Look Controler to the avatar
                    agents[mainAgentId].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;

                    //start the main trial and disscussion
                    string PSId = PoliteStgNo.ToString();

                    //choose the direction to show
                    if (showLeft)
                    {
                        PSId += "L";
                        showLeft = false;
                    }
                    else
                    {
                        PSId += "R";
                        showLeft = true;
                    }
                    

                    StartCoroutine(StartDiscussion(mainAgentId, 0, "Examples/DemoEN/Politeness/PS" + PSId));
                }
            }
            else if (collisionNo == collisionThreshold && NoActiveAgents == 2) //avatr is inside the memebrs' group radius for talking (O-Space)
            {
                //Change the focus of the lateral agent Head Look Controler to the avatar
                agents[lateralAgent1Id].GetComponent<HeadLookController>().targetObject = avatarHeadObject.transform;
            }
        }*/
    }

    private void OnTriggerExit(Collider col)
    {
        //avatarCollision = false;
        Debug.Log(col.gameObject.name + " exited the : " + gameObject.name);



        /*
        //Start a fake conversation among the agents
        if (col.gameObject.name == avatarName && collisionNo < 6)
            FakeConversationManager();
            */
    }
}
