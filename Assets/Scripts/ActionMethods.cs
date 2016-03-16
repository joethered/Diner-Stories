using UnityEngine;
using System.Collections;

public class ActionMethods {

    public bool setPlayerAsWidow(State state)
    {
        state.roles[RoleType.Widowed][0].isPlayer = true;
        state.roles[RoleType.Widowed][0].personalRoles.Add(RoleType.Player);
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.playerCharacter = state.roles[RoleType.Widowed][0];
        return true;
    }
    public bool setPlayerAsDater1(State state)
    {
        state.roles[RoleType.Daters][0].isPlayer = true;
        state.roles[RoleType.Daters][0].personalRoles.Add(RoleType.Player);
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.playerCharacter = state.roles[RoleType.Daters][0];
        return true;
    }
    public bool setPlayerAsServer(State state)
    {
        state.roles[RoleType.Servers][0].isPlayer = true;
        state.roles[RoleType.Servers][0].personalRoles.Add(RoleType.Player);
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.playerCharacter = state.roles[RoleType.Servers][0];
        return true;
    }
    public bool setPlayerAsRobber(State state)
    {
        state.roles[RoleType.Robbers][0].isPlayer = true;
        state.roles[RoleType.Robbers][0].personalRoles.Add(RoleType.Player);
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.playerCharacter = state.roles[RoleType.Robbers][0];
        return true;
    }
    public bool setPlayerAsPatron(State state)
    {
        state.roles[RoleType.Patron][0].isPlayer = true;
        state.roles[RoleType.Patron][0].personalRoles.Add(RoleType.Player);
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.playerCharacter = state.roles[RoleType.Patron][0];
        return true;
    }

    /*
    public bool setExistingAsDater1(State state)
    {
        state.roles[RoleType.Servers][0].isPlayer = true;
        state.roles[RoleType.Servers][0].personalRoles.Add(RoleType.Player);
        return true;
    }
    public bool setExistingAsDater2(State state)
    {
        return false;
    }
    public bool setExistingAsServer(State state)
    {
        return false;
    }
    public bool setExistingAsPatron(State state)
    {
        return false;
    }
    public bool setExistingAsRobber(State state)
    {
        return false;
    }*/

    public bool setRobberAsDater1(State state)
    {
        state.roles[RoleType.Robbers][0].personalRoles.Add(RoleType.Daters);
        if (state.roles[RoleType.Daters].Count < 1)
        {
            state.roles[RoleType.Daters].Add(state.roles[RoleType.Robbers][0]);
        }
        else
        {
            state.roles[RoleType.Daters][0] = state.roles[RoleType.Robbers][0];
        }
        return true;
    }
    public bool setRobberAsDater2(State state)
    {
        state.roles[RoleType.Robbers][0].personalRoles.Add(RoleType.Daters);
        if (state.roles[RoleType.Daters].Count < 2)
        {
            state.roles[RoleType.Daters].Add(state.roles[RoleType.Robbers][0]);
        }
        else
        {
            state.roles[RoleType.Daters][1] = state.roles[RoleType.Robbers][0];
        }
        return true;
    }

    public bool setWidowAsDater1(State state)
    {
        state.roles[RoleType.Widowed][0].personalRoles.Add(RoleType.Daters);
        if (state.roles[RoleType.Daters].Count < 1)
        {
            state.roles[RoleType.Daters].Add(state.roles[RoleType.Widowed][0]);
        }
        else
        {
            state.roles[RoleType.Daters][0] = state.roles[RoleType.Widowed][0];
        }
        return true;
    }
    public bool setWidowAsDater2(State state)
    {
        state.roles[RoleType.Widowed][0].personalRoles.Add(RoleType.Daters);
        if (state.roles[RoleType.Daters].Count < 2)
        {
            state.roles[RoleType.Daters].Add(state.roles[RoleType.Widowed][0]);
        }
        else
        {
            state.roles[RoleType.Daters][1] = state.roles[RoleType.Widowed][0];
        }
        return true;
    }
    public bool setWidowAsRobber(State state)
    {
        state.roles[RoleType.Widowed][0].personalRoles.Add(RoleType.Robbers);
        state.roles[RoleType.Robbers].Add(state.roles[RoleType.Widowed][0]);
        return true;
    }

    public bool setNewAsDater1(State state)
    {
        CharacterRole newCharacter = new CharacterRole(state.roles[RoleType.All], false);
        newCharacter.personalRoles.Add(RoleType.Daters);
        if (state.roles[RoleType.Daters].Count < 1)
        {
            state.roles[RoleType.Daters].Add(newCharacter);
        }
        else
        {
            state.roles[RoleType.Daters][0] = newCharacter;
        }
        return true;
    }
    public bool setNewAsDater2(State state)
    {
        CharacterRole newCharacter = new CharacterRole(state.roles[RoleType.All], false);
        newCharacter.personalRoles.Add(RoleType.Daters);
        if (state.roles[RoleType.Daters].Count < 2)
        {
            state.roles[RoleType.Daters].Add(newCharacter);
        }
        else
        {
            state.roles[RoleType.Daters][1] = newCharacter;
        }

        return true;
    }
    public bool setNewAsRobber(State state)
    {
        CharacterRole newCharacter = new CharacterRole(state.roles[RoleType.All], false);
        newCharacter.personalRoles.Add(RoleType.Robbers);
        state.roles[RoleType.Robbers].Add(newCharacter);
        return true;
    }
    public bool setNewAsPatron(State state)
    {
        CharacterRole newCharacter = new CharacterRole(state.roles[RoleType.All], false);
        newCharacter.personalRoles.Add(RoleType.Patron);
        state.roles[RoleType.Patron].Add(newCharacter);
        return true;
    }
    public bool setNewAsServer(State state)
    {
        CharacterRole newCharacter = new CharacterRole(state.roles[RoleType.All], false);
        newCharacter.personalRoles.Add(RoleType.Servers);
        state.roles[RoleType.Servers].Add(newCharacter);
        return true;
    }

    public bool setLover1AsServer(State state)
    {
        state.roles[RoleType.Lovers][0].personalRoles.Add(RoleType.Servers);
        state.roles[RoleType.Servers].Add(state.roles[RoleType.Lovers][0]);
        return true;
    }
    public bool setLover2AsPatron(State state)
    {
        state.roles[RoleType.Lovers][1].personalRoles.Add(RoleType.Servers);
        state.roles[RoleType.Servers].Add(state.roles[RoleType.Lovers][1]);
        return true;
    }

    public bool setSpurnedAsRobber(State state)
    {
        state.roles[RoleType.Spurned][0].personalRoles.Add(RoleType.Robbers);
        state.roles[RoleType.Robbers].Add(state.roles[RoleType.Spurned][0]);
        return true;
    }

    public bool createRandomSpeedDateSetup(State state)
    {
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        state.roles[RoleType.Daters].Add(gc.addNewCharacter(state.roles[RoleType.All]));
        gc.playerCharacter = state.roles[RoleType.Daters][0];
        gc.playerCharacter.isPlayer = true;

        state.roles[RoleType.Daters].Add(gc.addNewCharacter(state.roles[RoleType.All]));
        gc.otherCharacter = state.roles[RoleType.Daters][1];
        return true;
    }
    public bool createRandomRobberySetup(State state)
    {
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        state.roles[RoleType.Robbers].Add(gc.addNewCharacter(state.roles[RoleType.All]));
        gc.playerCharacter = state.roles[RoleType.Robbers][0];
        gc.playerCharacter.isPlayer = true;

        state.roles[RoleType.Patron].Add(gc.addNewCharacter(state.roles[RoleType.All]));
        gc.otherCharacter = state.roles[RoleType.Patron][0];

        state.roles[RoleType.Servers].Add(gc.addNewCharacter(state.roles[RoleType.All]));
        gc.otherCharacter = state.roles[RoleType.Servers][0];
        return true;
    }
    public bool createRandomFuneralSetup(State state)
    {
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        state.roles[RoleType.Eulogists].Add(gc.addNewCharacter(state.roles[RoleType.All]));
        gc.playerCharacter = state.roles[RoleType.Eulogists][0];
        gc.playerCharacter.isPlayer = true;

        state.roles[RoleType.Widowed].Add(gc.addNewCharacter(state.roles[RoleType.All]));
        gc.otherCharacter = state.roles[RoleType.Widowed][0];

        state.roles[RoleType.Bereaved].Add(gc.addNewCharacter(state.roles[RoleType.All]));
        gc.otherCharacter = state.roles[RoleType.Bereaved][0];
        return true;
    }




    public bool createNewNPCRole(State state)
    {
        return false;
    }
    public bool createNewPlayerRole(State state)
    {
        return false;
    }
    public bool assignPlayerRole(State state)
    {
        return false;
    }
}
