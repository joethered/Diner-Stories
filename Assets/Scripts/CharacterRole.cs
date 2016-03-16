using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private int wittyMoves;
    private int formalMoves;
    private int awkwardMoves;
    public bool spokeLast = false;
    public List<RoleType> personalRoles = new List<RoleType>();
    public CharacterRole partner = null;

    public ActorMove move;

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
        else
        {
            personalRoles.Add(RoleType.Player);
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


            foreach (CharacterRole role in roles)
            {
                if (characterName.Equals(role.name))
                {
                    newName = false;
                    break;
                }
            }

            loopCount++;
            //Debug.Log(loopCount);
        } while (!newName && loopCount < 100);

        if (loopCount > 100)
        {
            Debug.Log("Error: Timed out.");
        }

        //name = characterName;
        roles.Add(this);
    }

    //Dating moves
    public void datingIntroMove(ActorMove previousMove, ActorMove currentMove = null)
    {
        move = new ActorMove();
        if (isPlayer)
        {
            if (currentMove != null)
            {
                move = currentMove;
            }
            else
            {
                move.value = chooseInCharacterDecision();
            }

        }
        else
        {
            move.value = chooseInCharacterDecision();
        }

        if (previousMove == null)
        {
            move.phase = SpeechPhase.Call;
            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You make a very witty introduction of yourself.";
                    else
                        move.output = name + " made a very witty introduction.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You were very formal introducing yourself.";
                    else
                        move.output = name + " made a very formal introduction.";

                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You awkwardly introduce yourself.";
                    else
                        move.output = name + " intoduced themself, but it was kind of awkward.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
        else
        {
            move.phase = SpeechPhase.Response;
            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You respond with a very witty introduction of yourself.";
                    else
                        move.output = name + " responds with a very witty introduction.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You responds with very formal introducing yourself.";
                    else
                        move.output = name + " responds with a very formal introduction.";

                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You awkwardly introduce yourself in response.";
                    else
                        move.output = name + " responds with an intoduction of themself, but it was kind of awkward.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }

    }

    public void datingComplimentMove(ActorMove previousMove, ActorMove currentMove = null)
    {

        move = new ActorMove();

        if (previousMove == null)
        {
            move.phase = SpeechPhase.Call;
            if (isPlayer)
            {
                if (currentMove != null)
                {
                    move = currentMove;
                }
                else
                {
                    move.value = chooseInCharacterDecision();
                }

            }
            else
            {
                // TODO make smarter computer choice
                move.value = chooseInCharacterDecision();
            }

            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You made an witty compliment.";
                    else
                        move.output = name + " made an witty compliment.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You made a really nice compliment.";
                    else
                        move.output = name + " made a really nice compliment.";
                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You made an awkward compliment.";
                    else
                        move.output = name + " made an awkward compliment.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
        else
        {
            move.phase = SpeechPhase.Response;
            if (isPlayer)
            {
                if (currentMove != null)
                {
                    move = currentMove;
                }
                else
                {
                    move.value = GetRandomEnum<ActionType>();
                }

            }
            else
            {
                // TODO make smarter computer choice
                move.value = GetRandomEnum<ActionType>();
            }
            switch (previousMove.value)
            {
                case ActionType.witty:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "You give the perfect response.";
                            else
                                move.output = name + " gave a clever response.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You chuckle.";
                            else
                                move.output = name + " chuckles.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "The humor went over your head.";
                            else
                                move.output = name + " didn't get it.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
                case ActionType.formal:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "You make light of it.";
                            else
                                move.output = name + " turns it into a joke.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You're touched to hear that.";
                            else
                                move.output = name + " thanks you.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "You're touched, but don't know how to respond.";
                            else
                                move.output = name + " is at a loss for words.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
                case ActionType.awkward:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "But you play it off.";
                            else
                                move.output = "But " + name + " plays it off.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You respond politely, by are a bit off put.";
                            else
                                move.output = name + " responds politely.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "You say something equally awkward.";
                            else
                                move.output = name + " also says something awkward.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
            }

        }
    }

    public void datingSmallTalkMove(ActorMove previousMove, ActorMove currentMove = null)
    {

        move = new ActorMove();

        if (previousMove == null)
        {
            move.phase = SpeechPhase.Call;
            if (isPlayer)
            {
                if (currentMove != null)
                {
                    move = currentMove;
                }
                else
                {
                    move.value = chooseInCharacterDecision();
                }

            }
            else
            {
                // TODO make smarter computer choice
                move.value = chooseInCharacterDecision();
            }

            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You made a witty observation about current events.";
                    else
                        move.output = name + " made a witty observation about current events.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You made an observation about current events.";
                    else
                        move.output = name + " made an observation about current events.";
                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You try to talk about current events, but you don't know what you're talking about.";
                    else
                        move.output = name + " tried to talk about current events, but makes shallow observations.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
        else
        {
            move.phase = SpeechPhase.Response;
            if (isPlayer)
            {
                if (currentMove != null)
                {
                    move = currentMove;
                }
                else
                {
                    move.value = GetRandomEnum<ActionType>();
                }

            }
            else
            {
                // TODO make smarter computer choice
                move.value = GetRandomEnum<ActionType>();
            }
            switch (previousMove.value)
            {
                case ActionType.witty:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "You counter with an equally witty observation.";
                            else
                                move.output = name + " countered with an equally witty observation.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You chuckle, but talk about the serious side of the topic.";
                            else
                                move.output = name + " chuckles, but then talks about the serious side of the topic.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "You don't know much about this topic.";
                            else
                                move.output = name + " doesn't know much about this topic.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
                case ActionType.formal:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "You make light of it.";
                            else
                                move.output = name + " turns it into a joke.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You make an insightful observation.";
                            else
                                move.output = name + " makes an insightful observation.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "You don't know much about this topic.";
                            else
                                move.output = name + " doesn't know much about this topic.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
                case ActionType.awkward:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "You turn it into a joke to change topics.";
                            else
                                move.output = name + " turns it into a joke to change topics.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You can't believe they said that, but decide not to comment.";
                            else
                                move.output = name + " looks confuse and moves on to the next topic.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "You also make a shallow observation.";
                            else
                                move.output = name + " also also makes a shallow observation.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
            }

        }
    }

    public void datingJokeMove(ActorMove previousMove, ActorMove currentMove = null)
    {

        move = new ActorMove();

        if (previousMove == null)
        {
            move.phase = SpeechPhase.Call;
            if (isPlayer)
            {
                if (currentMove != null)
                {
                    move = currentMove;
                }
                else
                {
                    move.value = chooseInCharacterDecision();
                }

            }
            else
            {
                // TODO make smarter computer choice
                move.value = chooseInCharacterDecision();
            }

            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You tell a hilarious joke.";
                    else
                        move.output = name + " tells a hilarious joke.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You tell a joke.";
                    else
                        move.output = name + " told a joke.";
                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You make a joke, but your delivery is off.";
                    else
                        move.output = name + " tried to tell a joke, but it fell flat.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
        else
        {
            move.phase = SpeechPhase.Response;
            if (isPlayer)
            {
                if (currentMove != null)
                {
                    move = currentMove;
                }
                else
                {
                    move.value = GetRandomEnum<ActionType>();
                }

            }
            else
            {
                // TODO make smarter computer choice
                move.value = GetRandomEnum<ActionType>();
            }
            switch (previousMove.value)
            {
                case ActionType.witty:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "You one up their joke.";
                            else
                                move.output = name + " tops your joke.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You laugh.";
                            else
                                move.output = name + " laughs.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "You don't get it.";
                            else
                                move.output = name + " doesn't get it.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
                case ActionType.formal:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "You one up their joke.";
                            else
                                move.output = name + " tops your joke.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You laugh.";
                            else
                                move.output = name + " laughs.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "You don't get it.";
                            else
                                move.output = name + " doesn't get it.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
                case ActionType.awkward:
                    switch (move.value)
                    {
                        case ActionType.witty:
                            if (isPlayer)
                                move.output = "You manage to pick it up and make it hilarious.";
                            else
                                move.output = name + " spins it into a funnier joke.";
                            wittyMoves++;
                            witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.formal:
                            if (isPlayer)
                                move.output = "You don't know how to respond.";
                            else
                                move.output = name + " is confused.";
                            formalMoves++;
                            formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                        case ActionType.awkward:
                            if (isPlayer)
                                move.output = "You also make an awkward joke.";
                            else
                                move.output = name + " also also makes an awkward joke.";
                            awkwardMoves++;
                            awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                            break;
                    }
                    break;
            }

        }
    }


    //Robbery Moves
    public void robberyOrderFoodMove(ActorMove previousMove, ActorMove currentMove = null)
    {
        move = new ActorMove();
        if (isPlayer)
        {
            if (currentMove != null)
            {
                move = currentMove;
            }
            else
            {
                move.value = chooseInCharacterDecision();
            }

        }
        else
        {
            move.value = chooseInCharacterDecision();
        }

        if (previousMove == null)
        {
            move.phase = SpeechPhase.Call;
            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You make a pun out as you order.";
                    else
                        move.output = name + " made a pun out as their order.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You order the most expensive item on the menu.";
                    else
                        move.output = name + " ordered the most expensive item on the menu.";

                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You make an order, but ask for a lot of substitutions.";
                    else
                        move.output = name + " orders, but asks for a lot of substitutions.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
        else
        {
            move.phase = SpeechPhase.Response;
            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You take the order and spin it into a pun.";
                    else
                        move.output = name + " takes your order and spins it into a pun.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You take the order.";
                    else
                        move.output = name + " takes your order.";

                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You take the order, but probably didn't get it right.";
                    else
                        move.output = name + " takes your order, but probably didn't get it right.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
    }

    public void robberyAnnounceRobberyMove(ActorMove previousMove, ActorMove currentMove = null)
    {
        move = new ActorMove();
        if (isPlayer)
        {
            if (currentMove != null)
            {
                move = currentMove;
            }
            else
            {
                move.value = chooseInCharacterDecision();
            }

        }
        else
        {
            move.value = chooseInCharacterDecision();
        }

        if (previousMove == null)
        {
            move.phase = SpeechPhase.Call;
            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You announce the robbery with a light-hearted tone.";
                    else
                        move.output = name + " announces the robbery with a light-hearted tone.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You assertively announce the robbery.";
                    else
                        move.output = name + " assertively announces the robbery.";

                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You stand to announce the robbery, but first accidentally drop your gun.";
                    else
                        move.output = name + " stands to announce the robbery, but first accidentally drops their gun.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
        else
        {
            move.phase = SpeechPhase.Response;
            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You make light of the situation.";
                    else
                        move.output = name + " makes light of the situation.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You remain calm.";
                    else
                        move.output = name + " remains calm.";

                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You start to panic.";
                    else
                        move.output = name + " starts to panic.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
    }

    public void robberyRobMove(ActorMove previousMove, ActorMove currentMove = null)
    {
        move = new ActorMove();
        if (isPlayer)
        {
            if (currentMove != null)
            {
                move = currentMove;
            }
            else
            {
                move.value = chooseInCharacterDecision();
            }

        }
        else
        {
            move.value = chooseInCharacterDecision();
        }

        if (previousMove == null)
        {
            move.phase = SpeechPhase.Call;
            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "With a bit of charm, you ask for the money in the cash register.";
                    else
                        move.output = name + " asks for the money in the register with a smile.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You demand for the money in the register.";
                    else
                        move.output = name + " demands for the money in the register.";

                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You studder as you ask for the money in the register.";
                    else
                        move.output = name + " studders as they ask for the money in the register.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
        else
        {
            move.phase = SpeechPhase.Response;
            switch (move.value)
            {
                case ActionType.witty:
                    if (isPlayer)
                        move.output = "You hand over the money and make a joke to lighten the mood.";
                    else
                        move.output = name + " hands of the money and makes a joke to lighten the mood.";

                    wittyMoves++;
                    witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.formal:
                    if (isPlayer)
                        move.output = "You calmly hand over the money.";
                    else
                        move.output = name + " calmly hands over the money.";

                    formalMoves++;
                    formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
                case ActionType.awkward:
                    if (isPlayer)
                        move.output = "You try to open the register, but it's stuck.  After few attempts, it opens and you hand over the money.";
                    else
                        move.output = name + " tries to open the register, but it's stuck.  After few attempts, it opens and they hand over the money.";

                    awkwardMoves++;
                    awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                    break;
            }
        }
    }

    public void robberyOpportunityMove(ActorMove previousMove, ActorMove currentMove = null)
    {
        move = new ActorMove();
        if (isPlayer)
        {
            if (currentMove != null)
            {
                move = currentMove;
            }
            else
            {
                move.value = GetRandomEnum<ActionType>();
            }

        }
        else
        {
            move.value = GetRandomEnum<ActionType>();
        }

        move.phase = SpeechPhase.Response;
        switch (move.value)
        {
            case ActionType.witty:
                if (isPlayer)
                    move.output = "You act quickly and escape from the diner.";
                else
                    move.output = name + " quickly escapes from the diner.";

                wittyMoves++;
                witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
                break;
            case ActionType.formal:
                if (isPlayer)
                    move.output = "You assertively tell them to sit back down.";
                else
                    move.output = name + " assertively tell them to sit back down.";

                formalMoves++;
                formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
                break;
            case ActionType.awkward:
                if (isPlayer)
                    move.output = "You panic and accidentally fire your gun";
                else
                    move.output = name + " panics and accidentally fire your gun.";

                awkwardMoves++;
                awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
                break;
        }

    }


    //Funeral Moves
    public void funeralGatheringMove(ActorMove previousMove, ActorMove currentMove = null)
    {
		move = new ActorMove();
		if (isPlayer)
		{
			if (currentMove != null)
			{
				move = currentMove;
			}
			else
			{
				move.value = chooseInCharacterDecision();
			}

		}
		else
		{
			move.value = chooseInCharacterDecision();
		}

		if (previousMove == null)
		{
			move.phase = SpeechPhase.Call;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You make a pun as you meet with the other attendees.";
				else
					move.output = name + " made a pun as they meet with the other attendees.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You greet the other attendees with the most sincerity.";
				else
					move.output = name + " greeted the other attendees with the most sincerity.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "You make an introduction, but end up stumbling as you approach the attendees.";
				else
					move.output = name + " makes an introduction, but ends up stumbling as they approach the attendees.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
		else
		{
			move.phase = SpeechPhase.Response;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You stand quietly as the pun falls flat.";
				else
					move.output = name + " stands quietly as the pun falls flat.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You greet them with the same kindness.";
				else
					move.output = name + " greets you with the same kindness.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "People turn in your direction as the place falls silent.";
				else
					move.output = "People turn in " + name + "'s direction as the place falls silent.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
    }

    public void funeralEulogyMove(ActorMove previousMove, ActorMove currentMove = null)
    {
		move = new ActorMove();
		if (isPlayer)
		{
			if (currentMove != null)
			{
				move = currentMove;
			}
			else
			{
				move.value = chooseInCharacterDecision();
			}

		}
		else
		{
			move.value = chooseInCharacterDecision();
		}

		if (previousMove == null)
		{
			move.phase = SpeechPhase.Call;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You give out an endearing eulogy, but try to bring up everyone's spirits.";
				else
					move.output = name + " gives out an endearing eulogy, but tries to bring up everyone's spirits.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You sympathize with everyone, treasuring the memory of the deceased.";
				else
					move.output = name + " sympathizes with everyone, treasuring the memory of the deceased.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "You pull out the eulogy from your pocket and read it out while stumbling your words.";
				else
					move.output = name + " pulls out the eulogy from their pocket and reads it out while stumbling their words.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
		else
		{
			move.phase = SpeechPhase.Response;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You make the entire room both happy and satisfied with the deceased.";
				else
					move.output = name + " makes the entire room both happy and satisfied with the deceased.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You successfully have the room tearing up to your eulogy.";
				else
					move.output = name + " successfully has the room tearing up to their eulogy.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "You seem to have made the room grow a bit impatient with your eulogy.";
				else
					move.output = name + " seems to have makde the room grow a bit impatient with their eulogy.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
    }

    public void funeralVisitationMove(ActorMove previousMove, ActorMove currentMove = null)
    {
		move = new ActorMove();
		if (isPlayer)
		{
			if (currentMove != null)
			{
				move = currentMove;
			}
			else
			{
				move.value = chooseInCharacterDecision();
			}

		}
		else
		{
			move.value = chooseInCharacterDecision();
		}

		if (previousMove == null)
		{
			move.phase = SpeechPhase.Call;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "As you pass the casket, you lay a single flower into it.";
				else
					move.output = "As " + name + " passes the casket, " + name + " lays a single flower into it.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You simply see the deceased and pay your respects to them.";
				else
					move.output = name + " simply sees deceased and pays their respects to them.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "You accidentally sneeze into the casket of the deceased.";
				else
					move.output = name + " accidentally sneezes into the casket of the deceased.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
		else
		{
			move.phase = SpeechPhase.Response;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You see the room continue to be silent, but everyone seems to be smiling.";
				else
					move.output = name + " sees the room continue to be silent, but everyone seems to be smiling.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You feel calm.";
				else
					move.output = name + " feels calm.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "You feel everyone's eyes directed at you.";
				else
					move.output = name + " feels everyone's eyes directed at you.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
    }

    public void funeralConsolationMove(ActorMove previousMove, ActorMove currentMove = null)
    {
		move = new ActorMove();
		if (isPlayer)
		{
			if (currentMove != null)
			{
				move = currentMove;
			}
			else
			{
				move.value = chooseInCharacterDecision();
			}

		}
		else
		{
			move.value = chooseInCharacterDecision();
		}

		if (previousMove == null)
		{
			move.phase = SpeechPhase.Call;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You console with the widowed, but tell them to look forward to the future.";
				else
					move.output = name + " consoles with the widowed, but tells them to look forward to the future.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You respectfully talk with the widowed, telling them your respects.";
				else
					move.output = name + " respectfully talks with the widowed, telling them their respects.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "You give a quick reply about the deceased, but nothing else.";
				else
					move.output = name + " gives a quick reply about the deceased, but nothing else.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
		else
		{
			move.phase = SpeechPhase.Response;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You give out a smile, and thank them for their kind words.";
				else
					move.output = name + " gives out a smile, and thanks you for your kind words.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You thank them from their kindness";
				else
					move.output = name + " thanks you from your kindness.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "You remain silent.";
				else
					move.output = name + " remains silent.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
    }

    public void funeralReceptionMove(ActorMove previousMove, ActorMove currentMove = null)
    {
		move = new ActorMove();
		if (isPlayer)
		{
			if (currentMove != null)
			{
				move = currentMove;
			}
			else
			{
				move.value = chooseInCharacterDecision();
			}

		}
		else
		{
			move.value = chooseInCharacterDecision();
		}

		if (previousMove == null)
		{
			move.phase = SpeechPhase.Call;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You lift everyone's spirits trying to bring up the mood.";
				else
					move.output = name + " lifts everyone's spirits trying to bring up the mood.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You talk with the other attendees.";
				else
					move.output = name + " talks with th other attendees.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "You try to laugh it off, but your laughter makes things awkward.";
				else
					move.output = name + " tries to laugh it off, but their laughter makes things awkward.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
		else
		{
			move.phase = SpeechPhase.Response;
			switch (move.value)
			{
			case ActionType.witty:
				if (isPlayer)
					move.output = "You and everyone begin to enjoy each other's company after their witty retort.";
				else
					move.output = name + " and everyone begin to enjoy each other's company after your witty retort.";

				wittyMoves++;
				witty = wittyMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.formal:
				if (isPlayer)
					move.output = "You feel glad as everyone respects their attendance.";
				else
					move.output = name + " feels glad as everyone respects your attendence.";

				formalMoves++;
				formal = formalMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			case ActionType.awkward:
				if (isPlayer)
					move.output = "With the room, silent, you decide to remain quiet throughout the event.";
				else
					move.output = "With the room silent, " + " decides to remain quiet throughout the event.";

				awkwardMoves++;
				awkward = awkwardMoves / (wittyMoves + formalMoves + awkwardMoves);
				break;
			}
		}
    }

    public ActionType chooseInCharacterDecision()
    {
        float max = Mathf.Max(witty, formal, awkward);
        if (max.Equals(witty))
        {
            return ActionType.witty;
        }
        else if (max.Equals(formal))
        {
            return ActionType.formal;
        }
        else
        {
            return ActionType.awkward;
        }
    }

    private static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length));
        return V;
    }
}
