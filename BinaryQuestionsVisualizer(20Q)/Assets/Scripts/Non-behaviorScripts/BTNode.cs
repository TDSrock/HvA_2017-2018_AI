using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class BTNode
{
    public string message;
    BTNode noNode;
    BTNode yesNode;
    [NonSerialized] BTTree parent;
    public int traversedTimes;
    public int winsOnThisNode;
    [NonSerialized] public GameObject visualNode;//ref too the visual node script

    /**
     * Constructor for the nodes: This class holds an String representing 
     * an object if the noNode and yesNode are null and a question if the
     * yesNode and noNode point to a BTNode.
     */
    public BTNode(string nodeMessage, int traversedTimes, int winsOnThisNode)
    {
        message = nodeMessage;
        noNode = null;
        yesNode = null;
        this.traversedTimes = traversedTimes;
        this.winsOnThisNode = winsOnThisNode;
    }

    public string query(int q)
    {
        traversedTimes++;
        if (q > 20)
        {
            TwentyQuestion.instance.state = TwentyQuestion.State.readyToStart;
            return "That was the last question. You win!";
        }
        else if (this.isQuestion())
        {
            return this.message;
        }
        else
        {
            return this.onQueryObject();
        }
    }

    public string onQueryObject()
    {
        TwentyQuestion.instance._state = TwentyQuestion.State.guessing;
        return "Are you thinking of a(n) " + this.message + "?";
    }

    public bool isQuestion()
    {
        if (noNode == null && yesNode == null)
            return false;
        else
            return true;
    }

    //Mutator Methods
    public void setMessage(string nodeMessage)
    {
        message = nodeMessage;
    }

    public string getMessage()
    {
        return message;
    }

    public void setNoNode(BTNode node)
    {
        noNode = node;
    }

    public BTNode getNoNode()
    {
        return noNode;
    }

    public void setYesNode(BTNode node)
    {
        yesNode = node;
    }

    public BTNode getYesNode()
    {
        return yesNode;
    }

    public void ResetTrackingVars()
    {
        traversedTimes = 0;
        winsOnThisNode = 0;
    }
}
