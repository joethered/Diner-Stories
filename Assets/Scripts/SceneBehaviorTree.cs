using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class SceneBehaviorTree {

	
}

public abstract class Node
{
    public virtual bool excecute(State state)
    {
        throw new NotImplementedException();
    }

}

public abstract class Composite : Node
{
    protected List<Node> children = new List<Node>();

    public Composite(List<Node> childNodes)
    {
        children = childNodes;
    }

}




//===================Composite Nodes==============================
public class Selector : Composite
{

    public Selector(List<Node> childNodes) : base(childNodes) { }


    public bool execute(State state)
    {
        foreach (Node child in children)
        {
            bool success = child.excecute(state);
            if (success)
            {
                return true;
            }
        }

        return false;
        
    }
}

public class Sequence : Composite
{
    public Sequence (List<Node> childNodes) : base(childNodes) { }
    public bool execute(State state)
    {
        foreach (Node child in children)
        {
            bool continue_execution = child.excecute(state);
            if (!continue_execution)
            {
                return false;
            }
        }
        return true;
    }
}


//=====================Leaf Nodes======================================
public class Check : Node
{
    MethodInfo check_function;
    CheckMethods checkMethods;

    public Check(string check_function)
    {
        checkMethods = new CheckMethods();
        Type checkType = typeof(CheckMethods);
        this.check_function = checkType.GetMethod(check_function);
    }

    public bool execute(State state)
    {
        object[] parameters = new object[1];
        parameters[0] = state;
        return (bool)check_function.Invoke(checkMethods, parameters);
    }
}


public class Action : Node
{
    MethodInfo action_function;
    ActionMethods actionMethods;

    public Action(string action_function)
    {
        actionMethods = new ActionMethods();
        Type actionType = typeof(ActionMethods);
        this.action_function = actionType.GetMethod(action_function);
    }

    public bool execute(State state)
    {
        object[] parameters = new object[1];
        parameters[0] = state;
        return (bool)action_function.Invoke(actionMethods, parameters);
    }
    
}
