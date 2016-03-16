using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;


public abstract class Node
{
    public virtual bool execute(State state)
    {
        throw new NotImplementedException();
    }

}

public abstract class Composite : Node
{
    protected string name;
    public List<Node> children = new List<Node>();

    public Composite(List<Node> childNodes, String name = null)
    {
        children = childNodes;
        this.name = name;
    }

    public Composite(String name)
    {
        this.name = name;
    }

}




//===================Composite Nodes==============================
public class Selector : Composite
{

    public Selector(List<Node> childNodes, String name) : base(childNodes, name) { }
    public Selector(String name) : base(name) { }

    public override bool execute(State state)
    {
        Debug.Log(name);
        foreach (Node child in children)
        {
            bool success = child.execute(state);
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
    public Sequence (List<Node> childNodes, String name) : base(childNodes, name) { }
    public Sequence(String name) : base(name) { }

    public override bool execute(State state)
    {
        Debug.Log(name);
        foreach (Node child in children)
        {
            bool continue_execution = child.execute(state);
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
    string name;

    public Check(string check_function)
    {
        name = check_function;
        checkMethods = new CheckMethods();
        Type checkType = typeof(CheckMethods);
        this.check_function = checkType.GetMethod(check_function);
    }

    public override bool execute(State state)
    {
        Debug.Log(name);
        object[] parameters = new object[1];
        parameters[0] = state;
        return (bool)check_function.Invoke(checkMethods, parameters);
    }
}


public class Action : Node
{
    MethodInfo action_function;
    ActionMethods actionMethods;
    string name;

    public Action(string action_function)
    {
        name = action_function;
        actionMethods = new ActionMethods();
        Type actionType = typeof(ActionMethods);
        this.action_function = actionType.GetMethod(action_function);
    }

    public override bool execute(State state)
    {
        Debug.Log(name);
        object[] parameters = new object[1];
        parameters[0] = state;
        return (bool)action_function.Invoke(actionMethods, parameters);
    }
    
}
