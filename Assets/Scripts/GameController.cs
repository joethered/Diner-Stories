using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Scenes
{
    SpeedDating, Robbery, Funeral
}

public class GameController : MonoBehaviour {
    private State state = new State();
    //private SceneTree


    public bool randomizedStart = false;
    public bool debug = true;


    //Story Variables
    private bool isDinerClosed = false;
    private bool moreSecurity = false;
    private Dictionary<string, List<CharacterRole>> roles = new Dictionary<string, List<CharacterRole>>();
    private bool wasRobbery = false;
    private CharacterRole playerCharacter;


    private List<string> beats = new List<string>();


    // Use this for initialization
    void Start() {
        Debug.Log("Initialize");
        roles["all"] = new List<CharacterRole>();
        roles["servers"] = new List<CharacterRole>();
        roles["cooks"] = new List<CharacterRole>();
        roles["hosts"] = new List<CharacterRole>();
        roles["managers"] = new List<CharacterRole>();
        // I am forgetting one.  Owners?
        roles["daters"] = new List<CharacterRole>();
        roles["bereaved"] = new List<CharacterRole>();
        roles["widow"] = new List<CharacterRole>();
        roles["eulogists"] = new List<CharacterRole>();
        roles["gaurds"] = new List<CharacterRole>();
        roles["cops"] = new List<CharacterRole>();
        roles["robbers"] = new List<CharacterRole>();
        roles["patron"] = new List<CharacterRole>();
        roles["robbery victim"] = new List<CharacterRole>();
        roles["lovers"] = new List<CharacterRole>();
        roles["friends"] = new List<CharacterRole>();
        roles["enemies"] = new List<CharacterRole>();
        roles["deceased"] = new List<CharacterRole>();
        roles["previousPlayers"] = new List<CharacterRole>();

        beats.Add("introduction");
        beats.Add("datingTrans");
        beats.Add("datingTrans");
        beats.Add("datingTrans");
        beats.Add("isInterested");

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

        if (debug) Debug.Log("Initialization Complete");

    }

    // Update is called once per frame
    void Update() {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (debug) Debug.Log("------Execute Speed Date------");
            executeSpeedDate();
        }

    }
    /*Example run
    //Initialize other
    //Beat 1
    You (name) sit down with (other).
    An introduction move is made by one agent.
    A response move is made by the other.
    //Beat 2
    //(Dating Transactions: let them move, compliment, small talk, joke)
    They compliment.
    You accept or counter compliment
    //Beat 3
    You small talk.
    They are bored.
    //Beat 4
    You joke.
    They laugh.
    //Beat 5
    Do you want to see this person again?
    Yes.
    But they weren't interested in you.
    */

    public void executeSpeedDate()
    {
        roles["daters"].Clear();
        roles["daters"].Add(addNewCharacter(roles["all"]));
        roles["daters"].Add(addNewCharacter(roles["all"]));

        roles["daters"][0].isPlayer = true;

        datingIntroduction(roles["daters"][0], roles["daters"][1]);
        BranchHistory transactionHistory = new BranchHistory(3);
        transactionHistory.markBranch(datingTransaction(roles["daters"][0], roles["daters"][1], transactionHistory));
        transactionHistory.markBranch(datingTransaction(roles["daters"][0], roles["daters"][1], transactionHistory));
        transactionHistory.markBranch(datingTransaction(roles["daters"][0], roles["daters"][1], transactionHistory));
        datingConclusion(roles["daters"][0], roles["daters"][1]);
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
                    name = GetRandomEnum<MaleNames>();
                    firstName = name.ToString();
                    break;
                case 2:
                    name = GetRandomEnum<FemaleNames>();
                    firstName = name.ToString();
                    break;
                case 3:
                    name = GetRandomEnum<NeutralNames>();
                    firstName = name.ToString();
                    break;
                default:
                    name = GetRandomEnum<NeutralNames>();
                    firstName = name.ToString();
                    break;
            }

            name = GetRandomEnum<LastNames>();
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
            Debug.Log(loopCount);
        } while (!newName && loopCount < 100);

        if (loopCount > 100)
        {
            Debug.Log("Error: Timed out.");
        }

        newCharacter.name = characterName;
        currentRoles.Add(newCharacter);
        return newCharacter;
    }

    public void datingIntroduction(CharacterRole player, CharacterRole other)
    {
        Debug.Log("You, " + player.name + ", sit down with " + other.name + ".");
        ActorMove playerMove = datingIntroMove(player, true);
        Debug.Log(playerMove.output);
        ActorMove otherMove = datingIntroMove(other, false);
        Debug.Log(otherMove.output);
        if (playerMove.value == 5)
        {
            playerMove = datingIntroMove(player, false);
            Debug.Log(playerMove.output);
        }

    }

    public ActorMove datingIntroMove(CharacterRole character, bool initiator)
    {
        ActorMove move = new ActorMove();
        if (initiator)
        {
            move.value = Random.Range(1, 5);
        }
        else
        {
            move.value = Random.Range(1, 4);
        }
        
        switch (move.value)
        {
            case 1:
                if (character.isPlayer)
                    move.output = "You make a very suave introduction of yourself.";
                else
                    move.output = character.name + " made a very suave introduction.";
                break;
            case 2:
                if (character.isPlayer)
                    move.output = "You awkwardly introduce yourself.";
                else
                    move.output = character.name + " intoduced themself, but it was kind of awkward.";
                break;
            case 3:
                if (character.isPlayer)
                    move.output = "You were very formal introducing yourself.";
                else
                    move.output = character.name + " was made a very formal introduction.";
                break;
            case 4:
                if (character.isPlayer)
                    move.output = "You introduced yourself.";
                else
                    move.output = "\"Hi, I'm " + character.name + ",\" says " + character.name + ".";
                break;
            case 5:
                if (character.isPlayer)
                    move.output = "You're not sure how to start.";
                else
                    move.output = "You think " + character.name + " is waiting for you to say something first.";
                break;
        }
        return move;
    }


    public int datingTransaction(CharacterRole player, CharacterRole other, BranchHistory transactionHistory)
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
            if (debug) Debug.Log("------Begin Dating Compliment Transaction-----");
            ActorMove playerMove = datingComplimentMove(player, true, 0);
            Debug.Log(playerMove.output);
            ActorMove otherMove = datingComplimentMove(other, false, playerMove.value);
            Debug.Log(otherMove.output);
            if (playerMove.value == 5)
            {
                playerMove = datingComplimentMove(player, false, otherMove.value);
                Debug.Log(playerMove.output);
            }
        }
        else if (transactionType == 2)
        {
            if (debug) Debug.Log("------Begin Dating Small Talk Transaction-----");
            ActorMove playerMove = datingSmallTalkMove(player, true, 0);
            Debug.Log(playerMove.output);
            ActorMove otherMove = datingSmallTalkMove(other, false, playerMove.value);
            Debug.Log(otherMove.output);
            if (playerMove.value == 5)
            {
                playerMove = datingSmallTalkMove(player, false, playerMove.value);
                Debug.Log(playerMove.output);
            }
        }
        else
        {
            if (debug) Debug.Log("------Begin Dating Joke Transaction-----");
            ActorMove playerMove = datingJokeMove(player, true, 0);
            Debug.Log(playerMove.output);
            ActorMove otherMove = datingJokeMove(other, false, playerMove.value);
            Debug.Log(otherMove.output);
            if (playerMove.value == 5)
            {
                playerMove = datingJokeMove(player, false, playerMove.value);
                Debug.Log(playerMove.output);
            }
        }
        


        return transactionType;
    }

    public ActorMove datingComplimentMove(CharacterRole character, bool initiator, int prompt)
    {
        
        ActorMove move = new ActorMove();
        if (initiator)
        {
            if (prompt == 0 || prompt == 4)
                move.value = Random.Range(1, 5);
            else
                move.value = prompt;
            switch (move.value)
            {
                case 1:
                    if (character.isPlayer)
                        move.output = "You made an obvious compliment.";
                    else
                        move.output = character.name + " made an obvious compliment.";
                    break;
                case 2:
                    if (character.isPlayer)
                        move.output = "You made a really nice compliment.";
                    else
                        move.output = character.name + " made a really nice compliment.";
                    break;
                case 3:
                    if (character.isPlayer)
                        move.output = "You made an inappropriate compliment.";
                    else
                        move.output = character.name + " made an inappropriate compliment.";
                    break;
                case 4:
                    if (character.isPlayer)
                        move.output = "You're not sure what to say next.";
                    else
                        move.output = character.name + " isn't saying anything.  The silence is awkward.";
                    break;
            }
        }
        else
        {
            if (prompt == 0)
                move.value = Random.Range(1, 3);
            else
                move.value = prompt;
            switch (move.value)
            {
                case 1:
                    if (character.isPlayer)
                        move.output = "You don't know how to react to that.";
                    else
                        move.output = character.name + " was not impressed.";
                    break;
                case 2:
                    if (character.isPlayer)
                        move.output = "You're touched to hear that.";
                    else
                        move.output = character.name + " thanks you.";
                    break;
                case 3:
                    if (character.isPlayer)
                        move.output = "You're thinking about leaving.";
                    else
                        move.output = character.name + " is uncomfortable.";
                    break;
            }
        }
        return move;
    }

    public ActorMove datingSmallTalkMove(CharacterRole character, bool initiator, int prompt)
    {
        
        ActorMove move = new ActorMove();
        if (initiator)
        {
            if (prompt == 0 || prompt == 4)
                move.value = Random.Range(1, 5);
            else
                move.value = prompt;
            switch (move.value)
            {
                case 1:
                    if (character.isPlayer)
                        move.output = "You talked about the weather.";
                    else
                        move.output = character.name + " mentioned the weather.";
                    break;
                case 2:
                    if (character.isPlayer)
                        move.output = "You mention the latest news.";
                    else
                        move.output = character.name + " comented on the news of the day.";
                    break;
                case 3:
                    if (character.isPlayer)
                        move.output = "You remark about the decorations in the room.";
                    else
                        move.output = character.name + " notices the decorations in the room.";
                    break;
                case 4:
                    if (character.isPlayer)
                        move.output = "You're not sure what to say next.";
                    else
                        move.output = character.name + " isn't saying anything.  The silence is awkward.";
                    break;
            }
        }
        else
        {
            if (prompt == 0)
                move.value = Random.Range(1, 3);
            else
                move.value = prompt;
            switch (move.value)
            {
                case 1:
                    if (character.isPlayer)
                        move.output = "You agree.";
                    else
                        move.output = character.name + " agrees.";
                    break;
                case 2:
                    if (character.isPlayer)
                        move.output = "You disagree.";
                    else
                        move.output = character.name + " disagrees.";
                    break;
                case 3:
                    if (character.isPlayer)
                        move.output = "You think they are nice, too.";
                    else
                        move.output = character.name + " thinks they're nice, too.";
                    break;
            }
        }
        return move;
    }

    public ActorMove datingJokeMove(CharacterRole character, bool initiator, int prompt)
    {
        
        ActorMove move = new ActorMove();
        if (initiator)
        {
            if (prompt == 0 || prompt == 4)
                move.value = Random.Range(1, 5);
            else
                move.value = prompt;
            switch (move.value)
            {
                case 1:
                    if (character.isPlayer)
                        move.output = "You tell a silly joke.";
                    else
                        move.output = character.name + " tells a silly joke.";
                    break;
                case 2:
                    if (character.isPlayer)
                        move.output = "You make a witty joke.";
                    else
                        move.output = character.name + " tells a briliantly witty joke.";
                    break;
                case 3:
                    if (character.isPlayer)
                        move.output = "You tell a bad joke.";
                    else
                        move.output = character.name + " tells a bad joke.";
                    break;
                case 4:
                    if (character.isPlayer)
                        move.output = "You're not sure what to say next.";
                    else
                        move.output = character.name + " isn't saying anything.  The silence is awkward.";
                    break;
            }
        }
        else
        {
            if (prompt == 0)
                move.value = Random.Range(1, 3);
            else
                move.value = prompt;
            switch (move.value)
            {
                case 1:
                    if (character.isPlayer)
                        move.output = "You think it's dumb, but can't help but to laugh a bit.";
                    else
                        move.output = character.name + " thinks it's stupid, but still laughs.";
                    break;
                case 2:
                    if (character.isPlayer)
                        move.output = "You can't stop laughing.";
                    else
                        move.output = character.name + " can't stop laughing.";
                    break;
                case 3:
                    if (character.isPlayer)
                        move.output = "You are uncomfortable and don't know what to say.";
                    else
                        move.output = "You can't get a read on what " + character.name + " is thinking.";
                    break;
            }
        }
        return move;
    }

    public int datingConclusion(CharacterRole player, CharacterRole other)
    {
        int pChoice = Random.Range(0, 2);
        int oChoice = Random.Range(0, 2);
        Debug.Log(pChoice);
        Debug.Log(oChoice);
        if (pChoice == 1)
        {
            Debug.Log("You want to see " + other.name + " again.");

            if (oChoice == 1)
            {
                Debug.Log("And " + other.name + " wants to see you again.");
            }
            else
            {
                Debug.Log("But " + other.name + " didn't want to see you.");
            }
        }
        else
        {
            Debug.Log("You didn't want to see " + other.name + " again.");
        }

        return pChoice + oChoice;
    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length));
        return V;
    }

}

public class CharacterRole
{
    public string name;
    public Gender gender;
    public Sexuality sexuality;
    public bool isPlayer = false;
    public bool wasPlayer = false;
    public float witty;
    public float formal;
    public float awkward;

    public CharacterRole()
    {

    }

    public CharacterRole(List<CharacterRole> roles, bool player)
    {
        isPlayer = player;
        gender = GetRandomEnum<Gender>();
        sexuality = GetRandomEnum<Sexuality>();
        if (!isPlayer)
        {
            witty = Random.Range(0.0f, 1.0f);
            formal = Random.Range(0.0f, 1.0f);
            awkward = Random.Range(0.0f, 1.0f);
        }


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
                    name = GetRandomEnum<MaleNames>();
                    firstName = name.ToString();
                    break;
                case 2:
                    name = GetRandomEnum<FemaleNames>();
                    firstName = name.ToString();
                    break;
                case 3:
                    name = GetRandomEnum<NeutralNames>();
                    firstName = name.ToString();
                    break;
                default:
                    name = GetRandomEnum<NeutralNames>();
                    firstName = name.ToString();
                    break;
            }

            name = GetRandomEnum<LastNames>();
            lastName = name.ToString();

            characterName = firstName + " " + lastName;


            foreach (CharacterRole role in roles)
            {
                if (characterName.Equals(role.name))
                {
                    newName = false;
                    break;
                }
            }

            loopCount++;
            Debug.Log(loopCount);
        } while (!newName && loopCount < 100);

        if (loopCount > 100)
        {
            Debug.Log("Error: Timed out.");
        }

        //name = characterName;
        roles.Add(this);
    }



    private static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length));
        return V;
    }
}

public class ActorMove
{
    public int value;
    public string output;
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
        Debug.Log(choices);
        int choice;
        if (hasDefault)
        {
            choice = Random.Range(0, choices);
            return remaining[choice];
        }
        choice = Random.Range(1, choices);
        Debug.Log(choice);
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
    public int beatIndex = 1;

    public SceneTitles currentScene;

    public List<SceneTitles> previousScenes = new List<SceneTitles>();

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


public enum Gender
{
    male, female, transgenderM, transgenderF
}

public enum Sexuality
{
    hetero, gay, bi
}

public enum FemaleNames
{
    Autumn, Beth, Cindy, Diane, Erin, Fallon, Grace, Hellen, Iris, Jane, Karen, Lola, Maria, Nina, Olive, Pollyana,
    Queen, Rosa, Sarah, Tina, Uma, Vicky, Wendy, Xandra, Yoko, Zoe
}

public enum MaleNames
{
    Ari, Bob, Cristos, Diego, Eddy, Frank, George, Henry, Idris, James, Karl, Legolas, Mario, Nobuo, Oscar, Paul,
    Quinten, Red, Sean, Tiberius, Ulysses, Vincent, Wesley, Xander, Yusuke, Zed
}

public enum NeutralNames
{
    Amari, Blake, Casey, Dakota, Emory, Harper, Justice, Kai, Landry, Milan, Oakley, Phoenix, Quinn, River, Sawyer, Tatum
}

public enum LastNames
{
    Allen, Boreanaz, Chavez, Dimka, Estevez, Franco, Gilbertson, Hange, Ikuta, Jones, Kalama, Lymperopoulos, McCloud,
    Nquyen, Oppenheimer, Poe, Qadir, Rossi, Simpson, Tortelli, Umezaki, Vega, Whitehead, Xi, Yamauchi, Zardof
}

public enum SceneTitles
{
    SpeedDate,
    Robbery,
    Funeral
}
