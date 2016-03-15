using UnityEngine;
using System.Collections;

public class CheckMethods {

    //Current Scene Checks
    public bool isFirstScene(State state)
    {
        if (state.sceneNumber == 1)
        {
            return true;
        }
        return false;
    }
	public bool inSpeedDateScene(State state)
    {
        if (state.currentScene == Scene.SpeedDating)
        {
            return true;
        }
        return false;
    }
    public bool inRobberyScene(State state)
    {
        if (state.currentScene == Scene.Robbery)
        {
            return true;
        }
        return false;
    }
    public bool inFuneralScene(State state)
    {
        if (state.currentScene == Scene.Funeral)
        {
            return true;
        }
        return false;
    }

    //Prev Scene Checks
    public bool hadSpeedDate(State state)
    {
        foreach (Scene scene in state.previousScenes)
        {
            if (scene == Scene.SpeedDating)
            {
                return true;
            }
        }
        return false;
    }
    public bool hadRobbery(State state)
    {
        foreach (Scene scene in state.previousScenes)
        {
            if (scene == Scene.Robbery)
            {
                return true;
            }
        }
        return false;
    }
    public bool hadFuneral(State state)
    {
        foreach (Scene scene in state.previousScenes)
        {
            if (scene == Scene.Funeral)
            {
                return true;
            }
        }
        return false;
    }


}
