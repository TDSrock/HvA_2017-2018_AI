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
        var input = ' ';
        if(input == 'y')
            Console.Write("I Win!\n");
        else
            updateTree();
    }

    private void updateTree()
    {
        Console.Write("You win! What were you thinking of? ");
        string userObject = Console.ReadLine();
        Console.Write("Please enter a question to distinguish a(n) "
            + this.message + " from " + userObject + ": ");
        string userQuestion = Console.ReadLine();
        Console.Write("If you were thinking of a(n) " + userObject
            + ", what would the answer to that question be (\'yes\' or \'no\')? ");
        char input = getYesOrNo(); //y or n
        if(input == 'y')
        {
            this.noNode = new BTNode(this.message, this.traversedTimes, this.winsOnThisNode);
            this.yesNode = new BTNode(userObject, 0 , 0);
        }
        else
        {
            this.noNode = new BTNode(this.message, this.traversedTimes, this.winsOnThisNode);
            this.yesNode = new BTNode(userObject, 0, 0);
        }
        Console.Write("Thank you! My knowledge has been increased");
        this.setMessage(userQuestion);
    }

    public bool isQuestion()
    {
        if(noNode == null && yesNode == null)
            return false;
        else
            return true;
    }

    /**
     * Asks a user for yes or no and keeps prompting them until the key
     * Y,y,N,or n is entered
     */
    private char getYesOrNo()
    {
        char inputCharacter = ' ';
        /*while(inputCharacter != 'y' && inputCharacter != 'n')
        {
            inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
        }*/
        return inputCharacter;
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
}
