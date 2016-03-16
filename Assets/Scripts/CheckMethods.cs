using UnityEngine;
using System.Collections;

public class CheckMethods {


    public bool isThirdScene(State state)
    {
        if (state.actNumber == 3)
        {
            return true;
        }
        return false;
    }
    public bool isRobberPlotThreadActive(State state)
    {
        if (state.plotThreads.Contains(PlotThread.what_happened_to_the_robber))
        {
            return true;
        }
        return false;
    }
    public bool isWidowedPlotThreadActive(State state)
    {
        if (state.plotThreads.Contains(PlotThread.what_happened_to_the_widow))
        {
            return true;
        }
        return false;
    }
    public bool wasRobberPlayer(State state)
    {
        if (state.roles[RoleType.Robbers][0].personalRoles.Contains(RoleType.PreviousPlayers))
        {
            return true;
        }
        return false;
    }
    public bool wasWidowPlayer(State state)
    {
        if (state.roles[RoleType.Widowed][0].personalRoles.Contains(RoleType.PreviousPlayers))
        {
            return true;
        }
        return false;
    }
    public bool isSecondScene(State state)
    {
        if (state.actNumber == 2)
        {
            return true;
        }
        return false;
    }
    public bool isLoversThreadActive(State state)
    {
        if (state.plotThreads.Contains(PlotThread.what_will_happen_to_the_lovers))
        {
            return true;
        }
        return false;
    }
    public bool isSpurnedThreadActive(State state)
    {
        if (state.plotThreads.Contains(PlotThread.what_will_happen_to_the_spurned))
        {
            return true;
        }
        return false;
    }










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
 