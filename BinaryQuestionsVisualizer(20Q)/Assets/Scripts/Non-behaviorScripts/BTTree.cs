using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class BTTree : MonoBehaviour
{
    bool constructorUsed = false;
    [SerializeField]public BTNode rootNode;
    public TwentyQuestion parent;
    public BTTree(string question, string yesGuess, string noGuess)
    {
        rootNode = new BTNode(question, 0, 0);
        rootNode.setYesNode(new BTNode(yesGuess, 0, 0));
        rootNode.setNoNode(new BTNode(noGuess, 0, 0));
        this.constructorUsed = true;
        //Serialize the object on creation
        this.saveQuestionTree();
    }

    public BTTree()
    {

    }

    public void Start()
    {
            IFormatter formatter = new BinaryFormatter();
            using(FileStream stream = File.OpenRead(Application.persistentDataPath + "/serialized.bin"))
            {
                //rootNode = (BTNode)formatter.Deserialize(stream);
            }

    }

    public void query()
    {
        rootNode.query(1);

        //We're at the end of the game now, so we'll save the tree in case the user added new data
        //this.saveQuestionTree();
    }

    public void saveQuestionTree()
    {
        IFormatter formatter = new BinaryFormatter();
        using(FileStream stream = File.Create(Application.persistentDataPath + "/serialized.bin"))
        {
            formatter.Serialize(stream, rootNode);
        }
    }

}
