using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;




public class GameController : MonoBehaviour {
    private State state = new State();
    //private SceneTree

    public bool randomizedStart = false;
    
    public bool debug = true;

    private bool waitingForPlayer = false;

    //Story Variables
    private bool isDinerClosed = false;
    private bool moreSecurity = false;
    private Dictionary<RoleType, List<CharacterRole>> roles = new Dictionary<RoleType, List<CharacterRole>>();
    private bool wasRobbery = false;
    private CharacterRole playerCharacter;
    private CharacterRole otherCharacter;
    private CharacterRole recentSpeaker = null;
    BranchHistory transactionHistory;

    private List<System.Type> beats = new List<System.Type>();
    private List<PlotThread> plotThreads = new List<PlotThread>();

    private ActorMove previousMove = null;
    private ActorMove currentMove;
    private string nextMove;
    private System.Type roleType = typeof(CharacterRole);

    // Use this for initialization
    void Start() {
        Debug.Log("Initialize");

        System.Array roleEnums = System.Enum.GetValues(typeof(RoleType));

        foreach (RoleType r in roleEnums)
        {
            roles[r] = new List<CharacterRole>();
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
        state.currentScene = Scene.Robbery;

        if (debug) Debug.Log("Initialization Complete");

    }

    // Update is called once per frame
    void Update() {

        if (waitingForPlayer)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentMove = new ActorMove();
                currentMove.value = ActionType.witty;
                doNextBeat();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentMove = new ActorMove();
                currentMove.value = ActionType.formal;
                doNextBeat();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentMove = new ActorMove();
                currentMove.value = ActionType.awkward;
                doNextBeat();
            }
            //Debug.Log("he");
        }
        else
        {
            doNextBeat();
            //Debug.Log("hi");
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
                }

                break;


            case 5:
                // TODO Dating Conclusion
                datingConclusion();
                waitingForPlayer = true;
                state.beatIndex++;
                break;

            case 6:
                state.currentScene = Scene.Robbery;
                state.actNumber++;
                state.beatIndex = 0;
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
            return;
        }
        if (playerCharacter.personalRoles.Contains(RoleType.Robbers))
        {
            //Robber Beats
            switch (state.beatIndex)
            {
                
                case 1:
                    //Do Peace
                    nextMove = "robberyOrderFoodMove";
                    otherCharacter = roles[RoleType.Servers][0];
                    doMove();

                    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                    {
                        state.beatIndex++;
                        nextMove = "";
                        previousMove = null;
                        recentSpeaker = null;
                    }
                    break;

                case 2:
                    //Do Onset
                    nextMove = "robberyAnnounceRobberyMove";
                    otherCharacter = roles[RoleType.Servers][0];
                    doMove();

                    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                    {
                        state.beatIndex++;
                        nextMove = "";
                        previousMove = null;
                        recentSpeaker = null;
                    }
                    break;
                case 3:
                    //Do Robbing
                    nextMove = "robberyRobMove";
                    otherCharacter = roles[RoleType.Servers][0];
                    doMove();

                    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                    {
                        state.beatIndex++;
                        nextMove = "";
                        previousMove = null;
                        recentSpeaker = null;
                    }
                    break;
                case 4:
                    //Do Opportunity
                    nextMove = "robberyOpportunityMove";

                    doMove();

                    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                    {
                        state.beatIndex++;
                        nextMove = "";
                        if (previousMove.value == ActionType.awkward)
                        {
                            roles[RoleType.Deceased].Add(roles[RoleType.Patron][0]);
                            roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                            Debug.Log("You accidentally shot " + roles[RoleType.Patron][0].name + " .");
                        }
                        previousMove = null;
                        recentSpeaker = null;
                        
                    }

                    break;


                case 5:
                    // TODO Outcome
                    robberyConclusion();
                    waitingForPlayer = true;
                    state.beatIndex++;
                    break;

                case 6:
                    state.currentScene = Scene.SpeedDating;
                    state.actNumber++;
                    state.beatIndex = 0;
                    break;


            }
        }
        else if (playerCharacter.personalRoles.Contains(RoleType.Servers))
        {
            //Server Beats
            switch (state.beatIndex)
            {
                case 1:
                    //Do Introductions
                    doMove();

                    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                    {
                        state.beatIndex++;
                        nextMove = "";
                        previousMove = null;
                    }

                    break;

                case 2:
                case 3:
                case 4:
                    //Dating Transactions 3x
                    if (nextMove == "")
                    {
                        chooseDatingTransaction();
                    }

                    doMove();

                    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                    {
                        state.beatIndex++;
                        previousMove = null;
                    }

                    break;


                case 5:
                    // TODO Dating Conclusion



                    break;

            }
        }
        else
        {
            //Patron Beats
            switch (state.beatIndex)
            {
                case 1:
                    //Order Food
                    doMove();

                    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                    {
                        state.beatIndex++;
                        nextMove = "";
                        previousMove = null;
                    }

                    break;

                case 2:
                case 3:
                case 4:
                    //Dating Transactions 3x
                    if (nextMove == "")
                    {
                        chooseDatingTransaction();
                    }

                    doMove();

                    if (previousMove != null && previousMove.phase == SpeechPhase.Response)
                    {
                        state.beatIndex++;
                        previousMove = null;
                    }

                    break;


                case 5:
                    // TODO Dating Conclusion



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
        waitingForPlayer = !waitingForPlayer;
    }


    public CharacterRole addNewCharacter(List<CharacterRole> currentRoles)
    {
        //Make random name and check if it already exists.  If not, add it.
        CharacterRole newCharacter = new CharacterRole();
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
        roles[RoleType.Daters].Add(addNewCharacter(roles[RoleType.All]));
        playerCharacter = roles[RoleType.Daters][0];
        playerCharacter.isPlayer = true;

        roles[RoleType.Daters].Add(addNewCharacter(roles[RoleType.All]));
        otherCharacter = roles[RoleType.Daters][1];


        transactionHistory = new BranchHistory(3);

        previousMove = null;
        nextMove = "datingIntroMove";
        waitingForPlayer = canSpeakFirst(playerCharacter, otherCharacter);

        // OUTPUT
        Debug.Log("  You, " + roles[RoleType.Daters][0].name + ", sit down with " + roles[RoleType.Daters][1].name + ".");
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
            nextMove = "datingComplimentMove";
        }
        else if (transactionType == 2)
        {
            Debug.Log("The Small Talk");
            nextMove = "datingSmallTalkMove";
        }
        else
        {
            Debug.Log("The Joke");
            nextMove = "datingJokeMove";
        }


        transactionHistory.markBranch(transactionType);

        waitingForPlayer = canSpeakFirst(playerCharacter, otherCharacter);

        return transactionType;
    }

    public void datingConclusion()
    {
        int oChoice = Random.Range(0, 2);

        if (currentMove.value == ActionType.witty || currentMove.value == ActionType.formal)
        {
            Debug.Log("You want to see " + otherCharacter.name + " again.");

            if (oChoice == 1)
            {
                Debug.Log("And " + otherCharacter.name + " wants to see you again.");
                plotThreads.Add(PlotThread.what_will_happen_to_the_lovers);
                roles[RoleType.Lovers].Add(playerCharacter);
                playerCharacter.personalRoles.Add(RoleType.Lovers);
                playerCharacter.partner = otherCharacter;

                roles[RoleType.Lovers].Add(otherCharacter);
                otherCharacter.personalRoles.Add(RoleType.Lovers);
                otherCharacter.partner = playerCharacter;
            }
            else
            {
                Debug.Log("But " + otherCharacter.name + " didn't want to see you.");
                plotThreads.Add(PlotThread.what_will_happen_to_the_spurned);
                roles[RoleType.Spurned].Add(playerCharacter);
            }
        }
        else
        {
            Debug.Log("You didn't want to see " + otherCharacter.name + " again.");

            if (oChoice == 1)
            {
                plotThreads.Add(PlotThread.what_will_happen_to_the_spurned);
                roles[RoleType.Spurned].Add(otherCharacter);
            }
            else
            {
                plotThreads.Add(PlotThread.what_was_the_signifigance_of_the_small_talk);
            }
        }

    }


    public void robberySetup()
    {
        Debug.Log("The Robbery");
        roles[RoleType.Robbers].Add(addNewCharacter(roles[RoleType.All]));
        playerCharacter = roles[RoleType.Robbers][0];
        playerCharacter.personalRoles.Add(RoleType.Robbers);
        playerCharacter.isPlayer = true;

        roles[RoleType.Patron].Add(addNewCharacter(roles[RoleType.All]));

        roles[RoleType.Servers].Add(addNewCharacter(roles[RoleType.All]));
        otherCharacter = roles[RoleType.Servers][0];
        roles[RoleType.Servers][0].personalRoles.Add(RoleType.Servers);


        transactionHistory = new BranchHistory(3);

        previousMove = null;
        nextMove = "robberyOrderFoodMove";

        waitingForPlayer = true;

        Debug.Log("You, " + playerCharacter.name + ", are sitting in the diner.");
        Debug.Log(roles[RoleType.Patron][0].name + " is also sitting in the diner elsewhere.");
        Debug.Log(roles[RoleType.Servers][0].name + " is working.");
    }

    public void robberyConclusion()
    {
        if (state.actNumber == 2 && roles[RoleType.Deceased].Count == 0)
        {
            switch (roles[RoleType.Robbers][0].chooseInCharacterDecision())
            {
                case ActionType.witty:
                    if (roles[RoleType.Patron][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Patron][0]);
                        roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        roles[RoleType.Patron][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("On a whim you decide to shoot " + roles[RoleType.Patron][0].name + " .");
                    }
                    else if (roles[RoleType.Servers][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Servers][0]);
                        roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        roles[RoleType.Servers][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("On a whim you decide to shoot " + roles[RoleType.Servers][0].name + " .");
                    }
                    else
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Servers][0]);
                        roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        Debug.Log("On a whim you decide to shoot " + roles[RoleType.Servers][0].name + " .");
                    }

                    break;

                case ActionType.formal:
                    if (roles[RoleType.Patron][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Patron][0]);
                        roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        roles[RoleType.Patron][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("With a stoic look, you shoot " + roles[RoleType.Patron][0].name + " .");
                    }
                    else if (roles[RoleType.Servers][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Servers][0]);
                        roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        roles[RoleType.Servers][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("With a stoic look, you shoot " + roles[RoleType.Servers][0].name + " .");
                    }
                    else
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Servers][0]);
                        roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        Debug.Log("With a stoic look, you shoot " + roles[RoleType.Servers][0].name + " .");
                    }
                    break;

                case ActionType.awkward:
                    if (roles[RoleType.Patron][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Patron][0]);
                        roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        roles[RoleType.Patron][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("You fumble with your gun and accidentally shoot " + roles[RoleType.Patron][0].name + " .");
                    }
                    else if (roles[RoleType.Servers][0].personalRoles.Contains(RoleType.Lovers))
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Servers][0]);
                        roles[RoleType.Servers][0].personalRoles.Add(RoleType.Deceased);
                        roles[RoleType.Servers][0].partner.personalRoles.Add(RoleType.Widowed);
                        Debug.Log("You fumble with your gun and accidentally shoot " + roles[RoleType.Servers][0].name + " .");
                    }
                    else
                    {
                        roles[RoleType.Deceased].Add(roles[RoleType.Patron][0]);
                        roles[RoleType.Patron][0].personalRoles.Add(RoleType.Deceased);
                        Debug.Log("You fumble with your gun and accidentally shoot " + roles[RoleType.Patron][0].name + " .");
                    }
                    break;



            }

            plotThreads.Add(PlotThread.who_died);
            plotThreads.Add(PlotThread.who_is_the_robber);
            plotThreads.Add(PlotThread.will_the_robber_get_justice);

            
        }

        Debug.Log("You leave the diner with haste.");
    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length));
        return V;
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

    public bool isDinerClosed = false;
    public bool moreSecurity = false;
    public bool wasRobbery = false;

    public Dictionary<string, List<CharacterRole>> roles = new Dictionary<string, List<CharacterRole>>();
    
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
        roles = new Dictionary<string, List<CharacterRole>>(original.roles);



    }
}




