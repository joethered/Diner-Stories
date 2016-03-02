using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour {

    public Text downToughtLabel;
    public Text rightThoughtLabel;
    public Text upThoughtLabel;
    public Text leftThoughtLabel;
    public Text output;
    public List<Text> topics = new List<Text>(4);
    public List<Text> variations = new List<Text>();

    //public Dictionary<string, Dictionary<string, Markup>> thoughtTable;
    //public ThoughtDB thoughtDB;
    private ThoughtTree thoughtTree;

    private List<ThoughtTree> topicTrees = new List<ThoughtTree>();

    //private string currentSymbol;

    private Dictionary<string, string> responses;

    private Thought downThought;
    private Thought rightThought;
    private Thought upThought;
    private Thought leftThought;

    private bool canControl;
    float timeStamp;
    bool speaking;

    private Canvas canvas;

    private int topicIndex = 0;
    private int variationIndex = 0;

    bool vAxisInUse = false;

    void Start()
    {
        
        speaking = true;
        canControl = true;
        downThought = null;
        rightThought = null;
        upThought = null;
        leftThought = null;
        //thoughtDB = new ThoughtDB();
        //thoughtTable = new Dictionary<string, Dictionary<string, Markup>>();
        /*thoughtDB.addThought(new Thought("water", "water","I'll start with water.", "order_beverage", ""));
        addThought("coffee", "Just coffee.", "order_beverage", "");
        addThought("soda", "I'll take a Coke.", "order_beverage", "");
        addThought("soda", "I'll take a Sprite.", "order_beverage", "");
        addThought("eggs", "I'll take some eggs scrambled.", "order_meal", "");
        addThought("eggs", "I'll take some eggs over easy.", "order_meal", "");
        addThought("eggs", "I'll take some eggs sunny side up.", "order_meal", "");
        addThought("burger", "I'll have a burger.", "order_meal", "");
        addThought("burger", "I'll have a turkey burger.", "order_meal", "");
        addThought("burger", "I'll have a veggie burger.", "order_meal", "");
        addThought("burgerbacon", "I'll have a burger with bacon", "order_meal", "");
        addThought("burgerbacon", "I'll have a turkey burger with bacon", "order_meal", "");
        addThought("burgerbacon", "I'll have a veggie burger with 'bacon'", "order_meal", "");
        addThought("eggsbacon", "I'll take some eggs scrambled with a side of bacon.", "order_meal", "");
        addThought("eggsbacon", "I'll take some eggs over easy with a side of bacon.", "order_meal", "");
        addThought("eggsbacon", "I'll take some eggs sunny side up with a side of bacon.", "order_meal", "");*/
        responses = new Dictionary<string, string>();
        //Enter root
        List<string> tmpDeriv = new List<string>();
        tmpDeriv.Add("");
        topicTrees.Add(new ThoughtTree("Enter", new Thought ("","",tmpDeriv,"","")));

        //level 1
        tmpDeriv = new List<string>();
        tmpDeriv.Add("Hi.");
        Thought tmpThought = new Thought("greet", "Greet", tmpDeriv, "", "");
        topicTrees[0].addThought(tmpThought);
        responses.Add("greet", "You're late.");

        tmpDeriv = new List<string>();
        tmpDeriv.Add("Sorry I'm late.");
        tmpDeriv.Add("My phone was working.  I didn't know the time.");
        tmpDeriv.Add("Traffic was horrible!");
        tmpThought = new Thought("excuse", "Excuse", tmpDeriv, "", "");
        topicTrees[0].addThought(tmpThought);
        responses.Add("excuse", "Ok, well, you're here.");


        //level 2 greet
        topicTrees[0].moveToChild("greet");
        tmpDeriv = new List<string>();
        tmpDeriv.Add("Hi. Sorry I'm late.");
        tmpThought = new Thought("greetexcuse", "Excuse", tmpDeriv, "", "");
        topicTrees[0].addThought(tmpThought);
        responses.Add("greetexcuse", "It's fine.");
        //excuse
        topicTrees[0].moveToParent();
        topicTrees[0].moveToChild("excuse");
        tmpDeriv = new List<string>();
        tmpDeriv.Add("Sorry I'm late.  My phone was working.  I didn't know the time.");
        tmpThought = new Thought("excuseexcuse", "Excuse", tmpDeriv, "", "");
        topicTrees[0].addThought(tmpThought);
        responses.Add("excuseexcuse", "Ok, fine.");

        //level 3
        topicTrees[0].moveToChild("excuseexcuse");
        tmpDeriv = new List<string>();
        tmpDeriv.Add("Sorry I'm late. my phone was working and traffic was horrible!");
        tmpThought = new Thought("excuseexcuseexcuse", "Excuse", tmpDeriv, "", "");
        topicTrees[0].addThought(tmpThought);
        responses.Add("excuseexcuseexcuse", "Ok, ok, whatever.  Let's just not waste any more time.");

        topicTrees[0].returnToRoot();
        

        //Work root
        tmpDeriv = new List<string>();
        tmpDeriv.Add("");
        topicTrees.Add(new ThoughtTree("Work", new Thought("", "", tmpDeriv, "", "")));

        //level 1
        tmpDeriv = new List<string>();
        tmpDeriv.Add("Heh, I forgot to bring my work");
        tmpThought = new Thought("forgot", "Forgot", tmpDeriv, "", "");
        topicTrees[1].addThought(tmpThought);
        responses.Add("forgot", "Really?");

        tmpDeriv = new List<string>();
        tmpDeriv.Add("My dog ate my work");
        tmpThought = new Thought("dogexcuse", "Dog Excuse", tmpDeriv, "", "");
        topicTrees[1].addThought(tmpThought);
        responses.Add("dogexcuse", "Seriously?");

        topicTrees[1].returnToRoot();
        
        //Food root
        tmpDeriv = new List<string>();
        tmpDeriv.Add("");
        topicTrees.Add(new ThoughtTree("Food", new Thought("", "", tmpDeriv, "", "")));
        
        //level 1
        tmpDeriv = new List<string>();
        tmpDeriv.Add("Have You Eaten?");
        tmpThought = new Thought("hungry", "Hungry", tmpDeriv, "", "");
        topicTrees[2].addThought(tmpThought);
        responses.Add("hungry", "I just want to get this work finished.");
        
        tmpDeriv = new List<string>();
        tmpDeriv.Add("Can I Order Something first?");
        tmpThought = new Thought("order", "Order", tmpDeriv, "", "");
        topicTrees[2].addThought(tmpThought);
        responses.Add("order", "Ok.");

        topicTrees[2].returnToRoot();
        

        topics[0].text = topicTrees[0].name;
        topics[1].text = topicTrees[1].name;
        topics[2].text = topicTrees[2].name;
        topics[3].text = "";


        thoughtTree = topicTrees[0];
        

        updateThoughtCloud();



    }

    void Awake()
    {
        
    }


    // Update is called once per frame
    void Update () {
        if (canControl)
        {
            int length = variations[0].text.Length;
            if (length < 100)
            {
                //Debug.Log(upThought.display_name + downThought.display_name + leftThought.display_name + rightTought.display_name);

                if (Input.GetButtonDown("Down Thought") && downThought != null)
                {
                    loadVariations(downThought, variations);

                    thoughtTree.moveToChild(downThought.symbol);
                    updateThoughtCloud();
                }
                if (Input.GetButtonDown("Right Thought") && rightThought != null)
                {
                    loadVariations(rightThought, variations);

                    thoughtTree.moveToChild(rightThought.symbol);
                    updateThoughtCloud();
                }
                if (Input.GetButtonDown("Up Thought") && upThought != null)
                {
                    loadVariations(upThought, variations);

                    thoughtTree.moveToChild(upThought.symbol);
                    updateThoughtCloud();
                }
                if (Input.GetButtonDown("Left Thought") && leftThought != null)
                {
                    loadVariations(leftThought, variations);

                    thoughtTree.moveToChild(leftThought.symbol);
                    updateThoughtCloud();
                }
            }
            if (Input.GetButtonDown("Back"))
            {

                if (!thoughtTree.isAtRoot())
                {
                    thoughtTree.moveToParent();
                    loadVariations(thoughtTree.getCurNode().getThought(), variations);
                    updateThoughtCloud();
                }

            }

            if (Input.GetButtonDown("Submit"))
            {
                output.text = variations[variationIndex].text;
                foreach (Text variation in variations)
                {
                    variation.text = "";
                }
                
                canControl = false;
                timeStamp = Time.time;
            }

            if (Input.GetButtonDown("Next Topic"))
            {
                //Debug.Log(topicIndex);
                topics[topicIndex].color = new Color(0.2f, 0.2f, 0.2f);
                if (topicIndex < topicTrees.Count-1)
                    thoughtTree = topicTrees[++topicIndex];
                else
                {
                    topicIndex = 0;
                    thoughtTree = topicTrees[topicIndex];
                }
                    
                topics[topicIndex].color = new Color(0.68f, 0.68f, 0.68f);
                //Debug.Log(topicIndex);

                updateThoughtCloud();
            }
            if (Input.GetButtonDown("Prev Topic"))
            {
                topics[topicIndex].color = new Color(0.2f, 0.2f, 0.2f);
                if (topicIndex > 0)
                    thoughtTree = topicTrees[--topicIndex];
                else
                {
                    topicIndex = topicTrees.Count-1;
                    thoughtTree = topicTrees[topicIndex];
                }
                
                topics[topicIndex].color = new Color(0.68f, 0.68f, 0.68f);
                //Debug.Log(topicIndex);

                updateThoughtCloud();
            }

            if (Input.GetAxis("Vertical") < 0 && !vAxisInUse)
            {
                if (thoughtTree.getCurNode().getThought().derivation.Count > 1)
                {
                    Debug.Log(variationIndex);
                    variations[variationIndex].color = new Color(0.2f, 0.2f, 0.2f);
                    if (variationIndex < thoughtTree.getCurNode().getThought().derivation.Count)
                    {
                        ++variationIndex;
                    }
                    else
                    {
                        variationIndex = 0;
                    }
                    Debug.Log(variationIndex);
                    variations[variationIndex].color = new Color(0.68f, 0.68f, 0.68f);
                    vAxisInUse = true;
                }
                    
            }
            Debug.Log(vAxisInUse);
            if (Input.GetAxis("Vertical") < 0 && !vAxisInUse)
            {
                Debug.Log("should up main");
                if (thoughtTree.getCurNode().getThought().derivation.Count > 1)
                {
                    Debug.Log("should up count");
                    Debug.Log(variationIndex);
                    variations[variationIndex].color = new Color(0.2f, 0.2f, 0.2f);
                    Debug.Log("should up");
                    if (variationIndex > 0)
                    {
                        Debug.Log("up");
                        --variationIndex;
                    }
                    else
                    {
                        variationIndex = thoughtTree.getCurNode().getThought().derivation.Count - 1;
                    }
                    Debug.Log(variationIndex);
                    variations[variationIndex].color = new Color(0.68f, 0.68f, 0.68f);
                    vAxisInUse = true;
                }
                
            }
            Debug.Log(Input.GetAxis("Vertical"));
            Debug.Log(Input.GetAxisRaw("Vertical"));
            if (vAxisInUse && Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.2)
            {
                vAxisInUse = false;
                Debug.Log("hey");
            }
        }
        else
        {
            float newTime = Time.time;
            if (speaking && newTime > timeStamp + 1)
            {
                speaking = false;
                output.text = responses[thoughtTree.getCurNode().getThought().symbol];
            }
        }

    }

    /*
    private void addThought(string symbol, string display_name, string derivation, string markup, string sa)
    {
        if (thoughtTable.ContainsKey(symbol))
        {
            if (!thoughtTable[symbol].ContainsKey(derivation))
            {
                Markup tmpMarkup = new Markup(markup, sa);
                thoughtTable[symbol].Add(derivation, tmpMarkup);
            }
        }
        else
        {
            Markup tmpMarkup = new Markup(markup, sa);
            Dictionary<string, Markup> deriv = new Dictionary<string, Markup>();
            deriv.Add(derivation, tmpMarkup);
            thoughtTable.Add(symbol, deriv);
        }

    }

    private void addThought(string symbol, string derivation, string markup, string sa)
    {
        if (thoughtTable.ContainsKey(symbol))
        {
            if (!thoughtTable[symbol].ContainsKey(derivation))
            {
                Markup tmpMarkup = new Markup(markup, sa);
                thoughtTable[symbol].Add(derivation, tmpMarkup);
            }
        }
        else
        {
            Markup tmpMarkup = new Markup(markup, sa);
            Dictionary<string, Markup> deriv = new Dictionary<string, Markup>();
            deriv.Add(derivation, tmpMarkup);
            thoughtTable.Add(symbol, deriv);
        }
    }*/

    private void loadVariations(Thought thought, List<Text> variations)
    {
        for (int i = 0; i < thought.derivation.Count; ++i)
        {
            variations[i].text = thought.derivation[i];
            if (i == 0)
            {
                variations[i].color = new Color(0.68f, 0.68f, 0.68f);
            }
            else
            {
                variations[i].color = new Color(0.2f, 0.2f, 0.2f);
            }
        }
    }

    private void updateThoughtCloud()
    {
        int pos = 0;
        foreach (TreeNode t in thoughtTree.getCurNode().getChildren())
        {
            switch (pos)
            {
                case 0:
                    upThoughtLabel.text = t.getThought().display_name;
                    upThought = t.getThought();
                    break;
                case 1:
                    downToughtLabel.text = t.getThought().display_name;
                    downThought = t.getThought();
                    break;
                case 2:
                    leftThoughtLabel.text = t.getThought().display_name;
                    leftThought = t.getThought();
                    break;
                case 3:
                    rightThoughtLabel.text = t.getThought().display_name;
                    rightThought = t.getThought();
                    break;
            }
            pos++;
        }

        if (pos < 5)
        {
            for(; pos < 5; ++pos)
            {
                switch (pos)
                {
                    case 0:
                        upThoughtLabel.text = "";
                        upThought = null;
                        break;
                    
                    case 1:
                        downToughtLabel.text = "";
                        downThought = null;
                        break;
                    case 2:
                        leftThoughtLabel.text = "";
                        leftThought = null;
                        break;
                    case 3:
                        rightThoughtLabel.text = "";
                        rightThought = null;
                        break;
                    
                }
            }
        }
    }
}


/*
public class ThoughtDB
{
    Dictionary<string, List<Thought>> table;

    public ThoughtDB()
    {
        
    }

    public void addThought(Thought thought)
    {
        if (table.ContainsKey(thought.symbol)){                         //if the symbol already exists
            List<Thought> symbolThoughts = table[thought.symbol];
            foreach (Thought t in symbolThoughts)
            {
                if (t.derivation == thought.derivation)                 //if the derivation already exists under this symbol
                {
                    return;                                             //don't add anything
                }
            }
            table[thought.symbol].Add(thought);
        }
        else
        {
            table.Add(thought.symbol, new List<Thought>());
            table[thought.symbol].Add(thought);
        }
        
    }

}*/


/*
public class Markup
{
    string markup;
    string sa;

    public Markup(string m, string s)
    {
        markup = m;
        sa = s;
    }
}*/

public class ThoughtTree
{
    TreeNode root;
    TreeNode currentNode;
    public string name;
    bool atRoot;

    
    public ThoughtTree(string t, Thought thought)
    {
        name = t;
        root = new TreeNode(thought);
        currentNode = root;
        atRoot = true;
    }

    public void addThought(Thought t)
    {
        currentNode.addChild(t, currentNode);
    }

    public void moveToChild(string t)
    {
        atRoot = false;
        //Debug.Log("moveto start" + currentNode.getThought().symbol);
        foreach (TreeNode child in currentNode.getChildren()){
            //Debug.Log("in");
            if (child.getThought().symbol.Equals(t))
            {
                //Debug.Log("inner");
                currentNode = child;
                return;
            }
            //Debug.Log(child);
        }
        //Debug.Log("'" + t + "'" + " not a child of node");
    }

    public void returnToRoot()
    {
        
        currentNode = root;
        atRoot = true;
        
    }

    public void moveToParent()
    {
        currentNode = currentNode.getParent();
    }

    public TreeNode getCurNode()
    {
        return currentNode;
    }
    public bool isAtRoot()
    {
        return atRoot;
    }
}

public class Thought
{
    public string symbol;
    public string display_name;
    public List<string> derivation;
    public string markup;
    public string sa;

    public Thought(string symbol, string display_name, List<string> derivation, string markup, string sa)
    {
        this.symbol = symbol;
        this.display_name = display_name;
        this.derivation = derivation;
        this.markup = markup;
        this.sa = sa;
    }



}

public class TreeNode
{
    Thought thought;
    TreeNode parent;
    List<TreeNode> children;

    public TreeNode(Thought t)
    {
        thought = t;
        parent = null;
        children = new List<TreeNode>();
    }
    public void addChild(Thought t, TreeNode parent)
    {
        TreeNode tmp = new TreeNode(t);
        tmp.parent = parent;
        children.Add(tmp);
    }

    public List<TreeNode> getChildren()
    {
        return children;
    }

    public TreeNode getParent()
    {
        return parent;
    }

    public Thought getThought()
    {
        return thought;
    }
}
