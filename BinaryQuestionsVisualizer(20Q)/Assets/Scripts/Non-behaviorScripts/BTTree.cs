using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class BTTree
{
    bool constructorUsed = false;
    [SerializeField]public BTNode rootNode;
    [NonSerialized] public string filePath;
    public TwentyQuestion parent;
    public BTTree(string question, string yesGuess, string noGuess)
    {
        filePath = Application.persistentDataPath;
        rootNode = new BTNode(question, 0, 0);
        rootNode.setYesNode(new BTNode(yesGuess, 0, 0));
        rootNode.setNoNode(new BTNode(noGuess, 0, 0));
        this.constructorUsed = true;
        //Serialize the object on creation
        this.saveQuestionTree();
    }

    public BTTree()
    {
        filePath = Application.persistentDataPath;
        IFormatter formatter = new BinaryFormatter();
        using (FileStream stream = File.OpenRead(filePath + "/serialized.bin"))
        {
            rootNode = (BTNode)formatter.Deserialize(stream);
        }

    }

    public void saveQuestionTree()
    {
        IFormatter formatter = new BinaryFormatter();
        using(FileStream stream = File.Create(filePath + "/serialized.bin"))
        {
            formatter.Serialize(stream, rootNode);
        }
    }

}
