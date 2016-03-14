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
        if (state.currentScene == SceneTitles.SpeedDate)
        {
            return true;
        }
        return false;
    }
    public bool inRobberyScene(State state)
    {
        if (state.currentScene == SceneTitles.Robbery)
        {
            return true;
        }
        return false;
    }
    public bool inFuneralScene(State state)
    {
        if (state.currentScene == SceneTitles.Funeral)
        {
            return true;
        }
        return false;
    }

    //Prev Scene Checks
    public bool hadSpeedDate(State state)
    {
        foreach (SceneTitles scene in state.previousScenes)
        {
            if (scene == SceneTitles.SpeedDate)
            {
                return true;
            }
        }
        return false;
    }
    public bool hadRobbery(State state)
    {
        foreach (SceneTitles scene in state.previousScenes)
        {
            if (scene == SceneTitles.Robbery)
            {
                return true;
            }
        }
        return false;
    }
    public bool hadFuneral(State state)
    {
        foreach (SceneTitles scene in state.previousScenes)
        {
            if (scene == SceneTitles.Funeral)
            {
                return true;
            }
        }
        return false;
    }


}
