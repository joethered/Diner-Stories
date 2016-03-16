using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public State state = new State();
    //private SceneTree

    public bool randomizedStart = false;
    
    public bool debug = true;

    private bool waitingForPlayer = false;
    private bool waitingToProceed = false;

    //Story Variables
    private bool isDinerClosed = false;
    private bool moreSecurity = false;
    private bool wasRobbery = false;
    public CharacterRole playerCharacter;
    public CharacterRole otherCharacter;
    public CharacterRole recentSpeaker = null;
    BranchHistory transactionHistory;

    private List<System.Type> beats = new List<System.Type>();
    

    private ActorMove previousMove = null;
    private ActorMove currentMove;
    private string nextMove;
    private System.Type roleType = typeof(CharacterRole);

    private ActionType? input = null;

    private bool enabled = false;

    // Use this for initialization
    void Start() {
        Debug.Log("Initialize");

        System.Array roleEnums = System.Enum.GetValues(typeof(RoleType));

        foreach (RoleType r in roleEnums)
        {
            state.roles[r] = new List<CharacterRole>();
        }

        if (randomizedStart)
        {
            // Random Robbery Precondition
            System.Random gen = new System.Random();
            int prob = gen.Next(100);
            if (prob < 5)
            {
                wasRobbery = false;
                isDinerClosed = true;
                moreSecurity = false;

            }
        }

        state.actNumber = 1;
        state.beatIndex = 0;

        if (debug) Debug.Log("Initialization Complete");
        
    }

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (enabled)
        {
            if (!waitingToProceed)
            {
                if (waitingForPlayer)
                {

                    if (Input.GetKeyDown(KeyCode.UpArrow) || input == ActionType.witty)
                    {
                        input = null;
                        currentMove = new ActorMove();
                        currentMove.value = ActionType.witty;
                        doNextBeat();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow) || input == ActionType.formal)
                    {
                        input = null;
                        currentMove = new ActorMove();
                        currentMove.value = ActionType.formal;
                        doNextBeat();
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) || input == ActionType.awkward)
                    {
                        input = null;
                        currentMove = new ActorMove();
                        currentMove.value = ActionType.awkward;
                        doNextBeat();
                    }
                    //Debug.Log("he");
                }
                else
                {
                    doNextBeat();
                    if (previousMove == null)
                    {
                        waitingForPlayer = true;
                    }
                    //Debug.Log("hi");
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    waitingToProceed = false;
                    GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = true;
                    GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = true;
                    GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = true;
                }
            }
           
        }
        

    }

    public void doNextBeat()
    {
        
        switch (state.currentScene)
        {
            case Scene.SpeedDating:
                executeSpeedDateBeat();
                break;
            case Scene.Robbery:
                executeRobberyBeat();
                
                break;
			case Scene.Funeral:
				executeFuneralBeat ();
                break;
        }
        
    }

    public void executeSpeedDateBeat()
    {
        switch (state.beatIndex)
        {
            case 0:
                //Setup SpeedDate
                datingSetup();
                state.beatIndex++;
                waitingToProceed = true;
                GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                break;
            case 1:
                //Do Introductions
                doMove();

                if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                {
                    state.beatIndex++;
                    nextMove = "";
                    previousMove = null;
                    recentSpeaker = null;
                    waitingToProceed = true;
                    GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                }


                break;

            case 2:
            case 3:
            case 4:
                //Dating Transactions 3x
                if(nextMove == "")
                {
                    chooseDatingTransaction();
                }

                doMove();


                if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                {

                    state.beatIndex++;
                    nextMove = "";
                    previousMove = null;
                    recentSpeaker = null;
                    waitingToProceed = true;
                    GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                }

                break;


            case 5:
                // TODO Dating Conclusion
                datingConclusion();
                waitingForPlayer = true;
                state.beatIndex++;
                waitingToProceed = true;
                GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                break;

			case 6:
				//SceneManager.LoadScene ("Acts");
                state.actNumber++;
                state.beatIndex = 0;
                flipEnabled();
                waitingForPlayer = false;
                state.roles[RoleType.PreviousPlayers].Add(playerCharacter);
                state.roles[RoleType.Player].Clear();
                playerCharacter = null;
                Application.LoadLevel("Acts");
                break;


        }
        
    }

    public void executeRobberyBeat()
    {
        if (state.beatIndex == 0)
        {
            //Setup Robbery
            robberySetup();
            state.beatIndex++;
            waitingToProceed = true;
            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
            if (playerCharacter.personalRoles.Contains(RoleType.Robbers))
            {
                waitingForPlayer = true;
            }
            else
            {
                waitingForPlayer = false;
            }
            
            return;
        }
        //Robber Beats
        switch (state.beatIndex)
        {

            case 1:
                //Do Peace
                nextMove = "robberyOrderFoodMove";

                otherCharacter = state.roles[RoleType.Servers][0];
                doMove();

                if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                {
                    state.beatIndex++;
                    waitingToProceed = true;
                    GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                    nextMove = "";
                    previousMove = null;
                    recentSpeaker = null;
                    if (playerCharacter.personalRoles.Contains(RoleType.Robbers))
                    {
                        waitingForPlayer = true;
                    }
                    else
                    {
                        waitingForPlayer = false;
                    }
                }
                Text Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
                Beat.text = "The Onset";


                break;

            case 2:
                //Do Onset
                nextMove = "robberyAnnounceRobberyMove";
                otherCharacter = state.roles[RoleType.Servers][0];
                doMove();

                if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                {
                    state.beatIndex++;
                    waitingToProceed = true;
                    GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                    nextMove = "";
                    previousMove = null;
                    recentSpeaker = null;
                    if (playerCharacter.personalRoles.Contains(RoleType.Robbers))
                    {
                        waitingForPlayer = true;
                    }
                    else
                    {
                        waitingForPlayer = false;
                    }
                }
                Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
                Beat.text = "The Act";
                break;
            case 3:
                //Do Robbing
                nextMove = "robberyRobMove";
                otherCharacter = state.roles[RoleType.Servers][0];
                doMove();

                if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                {
                    state.beatIndex++;
                    waitingToProceed = true;
                    GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                    nextMove = "";
                    previousMove = null;
                    recentSpeaker = null;
                    if (playerCharacter.personalRoles.Contains(RoleType.Robbers))
                    {
                        waitingForPlayer = true;
                    }
                    else
                    {
                        waitingForPlayer = false;
                    }
                }
                Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
                Beat.text = "The Opportunity";
                break;
            case 4:
                //Do Opportunity
                nextMove = "robberyOpportunityMove";

                doMove();

                if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                {
                    state.beatIndex++;
                    waitingToProceed = true;
                    GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                    nextMove = "";
                    if (previousMove.value == ActionType.awkward)
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Patron][0]);
                        state.roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        Debug.Log("You accidentally shot " + state.roles[RoleType.Patron][0].name + " .");
                    }
                    previousMove = null;
                    recentSpeaker = null;
                    if (playerCharacter.personalRoles.Contains(RoleType.Robbers))
                    {
                        waitingForPlayer = true;
                    }
                    else
                    {
                        waitingForPlayer = false;
                    }

                }
                Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
                Beat.text = "The Outcome";
                break;


            case 5:
                // TODO Outcome
                robberyConclusion();
                waitingForPlayer = true;
                state.beatIndex++;
                waitingToProceed = true;
                GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                break;

            case 6:
                //SceneManager.LoadScene ("Acts");
                state.actNumber++;
                state.beatIndex = 0;
                flipEnabled();
                waitingForPlayer = false;
                state.roles[RoleType.PreviousPlayers].Add(playerCharacter);
                state.roles[RoleType.Player].Clear();
                playerCharacter = null;
                Application.LoadLevel("Acts");
                break;
        }


 

    }

	public void executeFuneralBeat(){
		if (state.beatIndex == 0)
		{
			//Setup Funeral
			funeralSetup();
			state.beatIndex++;
            waitingToProceed = true;
            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
            return;
		}
		if (playerCharacter.personalRoles.Contains(RoleType.Widowed))
		{
			//Widowed Beats
			switch (state.beatIndex)
			{

			    case 1:
				    //Do Peace
				    nextMove = "funeralGatheringMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                        GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                        nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 2:
				    nextMove = "funeralEulogyMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                        GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                        nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 3:
				    //Do Visitations
				    /*nextMove = "funeralVisitaionMove";

				    doMove ();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response) {
					    state.beatIndex++;
					    nextMove = "";
					    if (previousMove.value == ActionType.formal) {
						    Debug.Log ("You show your last sentiments to the deceased.");
					    } else if (previousMove.value == ActionType.awkward) {
						    Debug.Log ("You can hardly look at the deceased paying your respects, but scared to see them one more time.");
					    } else {
						    Debug.Log ("You make a sincere glance at the deceased, and leave without another word.");
					    }
					    previousMove = null;
					    recentSpeaker = null;

				    }*/
				    nextMove = "funeralVisitationMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                        GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                        nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 4:
				    //Do Consolation

				    nextMove = "funeralConsolationMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                        GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                        nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;


			    case 5:
				    // TODO Dating Conclusion
				    nextMove = "funeralReceptionMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                        GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                        nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 6:
				    // TODO Outcome
				    funeralConclusion();
				    waitingForPlayer = true;
				    state.beatIndex++;
                    waitingToProceed = true;
                    GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                    GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                    break;

			    case 7:
				    //SceneManager.LoadScene ("Acts");
				    state.actNumber++;
				    state.beatIndex = 0;
                    flipEnabled();
                    waitingForPlayer = false;
                    state.roles[RoleType.PreviousPlayers].Add(playerCharacter);
                    state.roles[RoleType.Player].Clear();
                    playerCharacter = null;
                    Application.LoadLevel("Acts");
				    break;


			}
		}
		else if (playerCharacter.personalRoles.Contains(RoleType.Eulogists))
		{
			//Eulogist Beats
			switch (state.beatIndex)
			{
			    case 1:
				    //Do Introductions
				    nextMove = "funeralGatheringMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
				    }

				    break;

			    case 2:
				    nextMove = "funeralEulogyMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 3:
				    nextMove = "funeralVisitationMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 4:
				    nextMove = "funeralConsolationMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;


			    case 5:
				    // TODO Dating Conclusion
				    nextMove = "funeralReceptionMove";
				    otherCharacter = state.roles[RoleType.Bereaved][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 6:
				    // TODO Dating Conclusion
				    funeralConclusion();
				    waitingForPlayer = true;
				    state.beatIndex++;
                    waitingToProceed = true;
                        GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                        break;

			    case 7:
                    //SceneManager.LoadScene ("Acts");
                    state.actNumber++;
                    state.beatIndex = 0;
                    flipEnabled();
                    waitingForPlayer = false;
                    state.roles[RoleType.PreviousPlayers].Add(playerCharacter);
                    state.roles[RoleType.Player].Clear();
                    playerCharacter = null;
                    Application.LoadLevel("Acts");
                    break;

			}
		}
		else
		{
			//Bereaved Beats
			switch (state.beatIndex)
			{
			    case 1:
				    //Order Food
				    nextMove = "funeralGatheringMove";
				    otherCharacter = state.roles[RoleType.Eulogists][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
				    }

				    break;

			    case 2:
				    nextMove = "funeralEulogistMove";

				    doMove ();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response) {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    if (previousMove.value == ActionType.awkward) {
						    Debug.Log ("You give out a loud sneeze breaking the pace of the ceremony and the attention.");
					    } else {
						    Debug.Log ("You give a sincere clap for the eulogist.");
					    }
					    previousMove = null;
					    recentSpeaker = null;

				    }
				    break;

			    case 3:
				    nextMove = "funeralVisitationMove";
				    otherCharacter = state.roles[RoleType.Eulogists][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 4:
				    nextMove = "funeralConsolationMove";
				    otherCharacter = state.roles[RoleType.Eulogists][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;


			    case 5:
				    // TODO Dating Conclusion
				    nextMove = "funeralReceptionMove";
				    otherCharacter = state.roles[RoleType.Eulogists][0];
				    doMove();

				    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
				    {
					    state.beatIndex++;
                        waitingToProceed = true;
                            GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                            GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                            nextMove = "";
					    previousMove = null;
					    recentSpeaker = null;
				    }
				    break;

			    case 6:
				    // TODO Dating Conclusion
				    funeralConclusion();
				    waitingForPlayer = true;
				    state.beatIndex++;
                    waitingToProceed = true;
                        GameObject.FindGameObjectWithTag("FormalButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("WittyButton").GetComponent<Button>().interactable = false;
                        GameObject.FindGameObjectWithTag("AwkwardButton").GetComponent<Button>().interactable = false;
                        break;

			    case 7:
                    //SceneManager.LoadScene ("Acts");
                    state.actNumber++;
                    state.beatIndex = 0;
                    flipEnabled();
                    waitingForPlayer = false;
                    state.roles[RoleType.PreviousPlayers].Add(playerCharacter);
                    state.roles[RoleType.Player].Clear();
                    playerCharacter = null;
                    Application.LoadLevel("Acts");
                    break;
			
			}
		}
	}

    public void doMove()
    {
        if (recentSpeaker != null)
        {
            previousMove = recentSpeaker.move;
        }

         

        object[] parameters = new object[2];
        parameters[0] = previousMove;
        parameters[1] = currentMove;

        if (waitingForPlayer)
        {
            //Debug.Log(nextMove);
            MethodInfo methodInfo = roleType.GetMethod(nextMove);
            methodInfo.Invoke(playerCharacter, parameters);
            recentSpeaker = playerCharacter;
            previousMove = playerCharacter.move;
        }
        else
        {
            //Debug.Log(nextMove);
            MethodInfo methodInfo = roleType.GetMethod(nextMove);
            methodInfo.Invoke(otherCharacter, parameters);
            recentSpeaker = otherCharacter;
            previousMove = otherCharacter.move;
        }

        

        // OUTPUT
        Debug.Log("  " + previousMove.output);
        if (previousMove.phase == SpeechPhase.Call)
        {
            Text initiator = GameObject.FindGameObjectWithTag("Initiator").GetComponent<Text>();
            initiator.text = previousMove.output;
            Text responder = GameObject.FindGameObjectWithTag("Responder").GetComponent<Text>();
            responder.text = "";
        }
        else
        {
            Text responder = GameObject.FindGameObjectWithTag("Responder").GetComponent<Text>();
            responder.text = previousMove.output;
        }
        
        waitingForPlayer = !waitingForPlayer;
    }


    public CharacterRole addNewCharacter(List<CharacterRole> currentRoles)
    {
        //Make random name and check if it already exists.  If not, add it.
        CharacterRole newCharacter = new CharacterRole(currentRoles, false);
        bool newName = true;
        string characterName;
        int loopCount = 0;
        do
        {
            int firstNamePool = Random.Range(1, 3);
            string firstName;
            string lastName;


            System.Enum name;
            switch (firstNamePool)
            {
                case 1:
                    name = GetRandomEnum<MaleName>();
                    firstName = name.ToString();
                    break;
                case 2:
                    name = GetRandomEnum<FemaleName>();
                    firstName = name.ToString();
                    break;
                case 3:
                    name = GetRandomEnum<NeutralName>();
                    firstName = name.ToString();
                    break;
                default:
                    name = GetRandomEnum<NeutralName>();
                    firstName = name.ToString();
                    break;
            }

            name = GetRandomEnum<LastName>();
            lastName = name.ToString();

            characterName = firstName + " " + lastName;


            foreach (CharacterRole role in currentRoles)
            {
                if (characterName.Equals(role.name))
                {
                    newName = false;
                    break;
                }
            }

            loopCount++;
        } while (!newName && loopCount < 100);

        if (loopCount > 100)
        {
            Debug.Log("Error: Timed out.");
        }

        newCharacter.name = characterName;
        currentRoles.Add(newCharacter);
        return newCharacter;
    }

    public bool canSpeakFirst(CharacterRole character1, CharacterRole character2)
    {
        if (character1.awkward <= character2.awkward)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void datingSetup()
    {
        Debug.Log("The Speed Date");
        Text Title = GameObject.FindGameObjectWithTag("SceneTitle").GetComponent<Text>();
        Title.text = "The Speed Date";

        SceneInitializer initializer = new SceneInitializer();

        Selector setupTree = initializer.setupSpeedDateTree();
        setupTree.execute(state);

        /*state.roles[RoleType.Daters].Add(addNewCharacter(state.roles[RoleType.All]));
        playerCharacter = state.roles[RoleType.Daters][0];
        playerCharacter.isPlayer = true;

        state.roles[RoleType.Daters].Add(addNewCharacter(state.roles[RoleType.All]));
        otherCharacter = state.roles[RoleType.Daters][1];*/


        transactionHistory = new BranchHistory(3);

        previousMove = null;
        nextMove = "datingIntroMove";
        waitingForPlayer = canSpeakFirst(playerCharacter, otherCharacter);

        // OUTPUT
        Debug.Log("  You, " + state.roles[RoleType.Daters][0].name + ", sit down with " + state.roles[RoleType.Daters][1].name + ".");
        Text initiator = GameObject.FindGameObjectWithTag("Initiator").GetComponent<Text>();
        initiator.text = "  You, " + state.roles[RoleType.Daters][0].name + ", sit down with " + state.roles[RoleType.Daters][1].name + ".";
        Text Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
        Beat.text = "Introductions";
        Debug.Log("Introductions");
    }

    public int chooseDatingTransaction()
    {

        int transactionType;

        if (transactionHistory.allBranchesVisited)
        {
            Debug.Log("ERROR: All branches visited");
            return 0;
        }

        transactionType = transactionHistory.getRandomUnused();


        if (transactionType == 1)
        {
            Debug.Log("The Compliment");
            Text Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
            Beat.text = "The Compliment";
            nextMove = "datingComplimentMove";
        }
        else if (transactionType == 2)
        {
            Debug.Log("The Small Talk");
            Text Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
            Beat.text = "The Small Talk";
            nextMove = "datingSmallTalkMove";
        }
        else
        {
            Debug.Log("The Joke");
            Text Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
            Beat.text = "The Joke";
            nextMove = "datingJokeMove";
        }


        transactionHistory.markBranch(transactionType);

        waitingForPlayer = canSpeakFirst(playerCharacter, otherCharacter);

        return transactionType;
    }

    public void datingConclusion()
    {
        int oChoice = Random.Range(0, 2);

        Text Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
        Beat.text = "The Choice";

        if (currentMove.value == ActionType.witty || currentMove.value == ActionType.formal)
        {
            Debug.Log("You want to see " + otherCharacter.name + " again.");
            Text initiator = GameObject.FindGameObjectWithTag("Initiator").GetComponent<Text>();
            initiator.text = "You want to see " + otherCharacter.name + " again.";

            if (oChoice == 1)
            {
                Debug.Log("And " + otherCharacter.name + " wants to see you again.");
                Text responder = GameObject.FindGameObjectWithTag("Responder").GetComponent<Text>();
                responder.text = "And " + otherCharacter.name + " wants to see you again.";
                state.plotThreads.Add(PlotThread.what_will_happen_to_the_lovers);
                state.roles[RoleType.Lovers].Add(playerCharacter);
                playerCharacter.personalRoles.Add(RoleType.Lovers);
                playerCharacter.partner = otherCharacter;

                state.roles[RoleType.Lovers].Add(otherCharacter);
                otherCharacter.personalRoles.Add(RoleType.Lovers);
                otherCharacter.partner = playerCharacter;
            }
            else
            {
                Debug.Log("But " + otherCharacter.name + " didn't want to see you.");
                Text responder = GameObject.FindGameObjectWithTag("Responder").GetComponent<Text>();
                responder.text = "But " + otherCharacter.name + " didn't want to see you.";
                state.plotThreads.Add(PlotThread.what_will_happen_to_the_spurned);
                state.roles[RoleType.Spurned].Add(playerCharacter);
            }
        }
        else
        {
            Debug.Log("You didn't want to see " + otherCharacter.name + " again.");
            Text initiator = GameObject.FindGameObjectWithTag("Initiator").GetComponent<Text>();
            initiator.text = "You didn't want to see " + otherCharacter.name + " again.";
            Text responder = GameObject.FindGameObjectWithTag("Responder").GetComponent<Text>();
            responder.text = "";


            if (oChoice == 1)
            {
                state.plotThreads.Add(PlotThread.what_will_happen_to_the_spurned);
                state.roles[RoleType.Spurned].Add(otherCharacter);
            }
            else
            {
                state.plotThreads.Add(PlotThread.what_was_the_signifigance_of_the_small_talk);
            }
        }

    }


    public void robberySetup()
    {
        Debug.Log("The Robbery");
        Text Title = GameObject.FindGameObjectWithTag("SceneTitle").GetComponent<Text>();
        Title.text = "The Robbery";

        SceneInitializer initializer = new SceneInitializer();

        Selector setupTree = initializer.setupRobberyTree();
        setupTree.execute(state);

        /*state.roles[RoleType.Robbers].Add(addNewCharacter(state.roles[RoleType.All]));
        playerCharacter = state.roles[RoleType.Robbers][0];
        playerCharacter.personalRoles.Add(RoleType.Robbers);
        playerCharacter.isPlayer = true;

        state.roles[RoleType.Patron].Add(addNewCharacter(state.roles[RoleType.All]));

        state.roles[RoleType.Servers].Add(addNewCharacter(state.roles[RoleType.All]));
        otherCharacter = state.roles[RoleType.Servers][0];
        state.roles[RoleType.Servers][0].personalRoles.Add(RoleType.Servers);
        */


        previousMove = null;
        nextMove = "robberyOrderFoodMove";

        waitingForPlayer = true;
        Text Beat = GameObject.FindGameObjectWithTag("Beat").GetComponent<Text>();
        Beat.text = "The Ordinary Day";

        Debug.Log("You, " + playerCharacter.name + ", are sitting in the diner.");
        Debug.Log(state.roles[RoleType.Patron][0].name + " is also sitting in the diner elsewhere.");
        Debug.Log(state.roles[RoleType.Servers][0].name + " is working.");

        Text initiator = GameObject.FindGameObjectWithTag("Initiator").GetComponent<Text>();
        initiator.text = "You, " + playerCharacter.name + ", are sitting in the diner.  " + state.roles[RoleType.Patron][0].name + " is also sitting in the diner elsewhere.  " +
                            state.roles[RoleType.Servers][0].name + " is working.";

    }

    public void robberyConclusion()
    {
        if (state.actNumber == 2 && state.roles[RoleType.Deceased].Count == 0)
        {
            switch (state.roles[RoleType.Robbers][0].chooseInCharacterDecision())
            {
                case ActionType.witty:
                    if (state.roles[RoleType.Patron][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Patron][0]);
                        state.roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        state.roles[RoleType.Patron][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("On a whim you decide to shoot " + state.roles[RoleType.Patron][0].name + " .");
                    }
                    else if (state.roles[RoleType.Servers][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Servers][0]);
                        state.roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        state.roles[RoleType.Servers][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("On a whim you decide to shoot " + state.roles[RoleType.Servers][0].name + " .");
                    }
                    else
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Servers][0]);
                        state.roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        Debug.Log("On a whim you decide to shoot " + state.roles[RoleType.Servers][0].name + " .");
                    }

                    break;

                case ActionType.formal:
                    if (state.roles[RoleType.Patron][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Patron][0]);
                        state.roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        state.roles[RoleType.Patron][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("With a stoic look, you shoot " + state.roles[RoleType.Patron][0].name + " .");
                    }
                    else if (state.roles[RoleType.Servers][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Servers][0]);
                        state.roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        state.roles[RoleType.Servers][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("With a stoic look, you shoot " + state.roles[RoleType.Servers][0].name + " .");
                    }
                    else
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Servers][0]);
                        state.roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        Debug.Log("With a stoic look, you shoot " + state.roles[RoleType.Servers][0].name + " .");
                    }
                    break;

                case ActionType.awkward:
                    if (state.roles[RoleType.Patron][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Patron][0]);
                        state.roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        state.roles[RoleType.Patron][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("You fumble with your gun and accidentally shoot " + state.roles[RoleType.Patron][0].name + " .");
                    }
                    else if (state.roles[RoleType.Servers][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Servers][0]);
                        state.roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        state.roles[RoleType.Servers][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("You fumble with your gun and accidentally shoot " + state.roles[RoleType.Servers][0].name + " .");
                    }
                    else
                    {
                        state.roles[RoleType.Deceased].Add(state.roles[RoleType.Patron][0]);
                        state.roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        Debug.Log("You fumble with your gun and accidentally shoot " + state.roles[RoleType.Patron][0].name + " .");
                    }
                    break;



            }

            state.plotThreads.Add(PlotThread.aftermath_of_death);
            state.plotThreads.Add(PlotThread.what_happened_to_the_robber);

            
        }

        Debug.Log("You leave the diner with haste.");
    }

	public void funeralSetup(){
		
		Debug.Log("The Funeral");
        Text Title = GameObject.FindGameObjectWithTag("SceneTitle").GetComponent<Text>();
        Title.text = "The Funeral";
        state.roles[RoleType.Widowed].Add(addNewCharacter(state.roles[RoleType.All]));
		playerCharacter = state.roles[RoleType.Widowed][0];
		playerCharacter.personalRoles.Add(RoleType.Widowed);
		playerCharacter.isPlayer = true;

        state.roles[RoleType.Eulogists].Add(addNewCharacter(state.roles[RoleType.All]));

        state.roles[RoleType.Bereaved].Add(addNewCharacter(state.roles[RoleType.All]));
		otherCharacter = state.roles[RoleType.Bereaved][0];
        state.roles[RoleType.Bereaved][0].personalRoles.Add(RoleType.Bereaved);


		transactionHistory = new BranchHistory(3);

		previousMove = null;
		nextMove = "funeralGatheringMove";

		waitingForPlayer = true;

		Debug.Log("You, " + playerCharacter.name + ", are arriving at the funeral.");
		Debug.Log(state.roles[RoleType.Eulogists][0].name + " seems to be both anxious and sad.");
		Debug.Log(state.roles[RoleType.Bereaved][0].name + " is waiting for the ceremony.");
	}

	public void funeralConclusion()
    {

	}

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length));
        return V;
    }

    public void clickWitty()
    {
        input = ActionType.witty;
    }
    public void clickFormal()
    {
        input = ActionType.formal;
    }
    public void clickAwkward()
    {
        input = ActionType.awkward;
    }

    public void flipEnabled()
    {
        enabled = !enabled;
        Text initiator = GameObject.FindGameObjectWithTag("Initiator").GetComponent<Text>();
        initiator.text = "";
        Text responder = GameObject.FindGameObjectWithTag("Responder").GetComponent<Text>();
        responder.text = "";
    }
}



public class ActorMove
{
    public ActionType value;
    public string output;
    public SpeechPhase phase;
    public string response;
    public int respondToIndex;
    private ActorMove previousMove;

}

public class BranchHistory
{
    //This class is for storing the history of a branching node to determine if all branches have
    //been explored. This also allows for there to be branches that are ignored by this check.
    //To have an ignored branch, it needs a key outside the range of 1 and numOfBranches.
    private List<int> remaining = new List<int>();
    private List<int> used = new List<int>();
    public bool allBranchesVisited = false;

    public BranchHistory(int numOfBranches)
    {
        for (int i = 1; i <= numOfBranches; ++i)
        {
            remaining.Add(i);
        }
    }

    public void markBranch(int i)
    {
        if (remaining.Contains(i))
        {
            remaining.Remove(i);
            used.Add(i);
            if (remaining.Count == 0)
            {
                allBranchesVisited = true;
            }
        }
    }

    public int getRandomUnused(bool hasDefault = false)
    {
        if (allBranchesVisited)
        {
            Debug.Log("ERROR: all branches visited");
            return 0;
        }
        int choices = remaining.Count;
        //Debug.Log(choices);
        int choice;
        if (hasDefault)
        {
            choice = Random.Range(0, choices);
            return remaining[choice];
        }
        choice = Random.Range(1, choices);
        //Debug.Log(choice);
        return remaining[choice-1];
    }

    public bool hasVisited(int branch)
    {
        return used.Contains(branch);
    }

}

public class State
{
    public int sceneNumber = 1;
    public int actNumber = 1;
    public int beatIndex = 0;

    public Scene currentScene;

    public List<Scene> previousScenes = new List<Scene>();
    public List<PlotThread> plotThreads = new List<PlotThread>();

    public bool isDinerClosed = false;
    public bool moreSecurity = false;
    public bool wasRobbery = false;

    public Dictionary<RoleType, List<CharacterRole>> roles = new Dictionary<RoleType, List<CharacterRole>>();
    
    public CharacterRole playerCharacter;

    public State()
    {

    }

    public State(State original)
    {


        sceneNumber = original.sceneNumber;
        beatIndex = original.beatIndex;
        isDinerClosed = original.isDinerClosed;
        moreSecurity = original.moreSecurity;
        wasRobbery = original.wasRobbery;
        roles = new Dictionary<RoleType, List<CharacterRole>>(original.roles);



    }
}




