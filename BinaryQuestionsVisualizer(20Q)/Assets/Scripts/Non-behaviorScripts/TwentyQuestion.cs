using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.UI;

public class TwentyQuestion : MonoBehaviour
{
    public enum State { setupNewGameState, readyToStart, playing }

    public State state = State.readyToStart;
    static BTTree tree;
    [SerializeField]private string output;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private GameObject buttonsSection;
    [SerializeField]
    private GameObject inputFieldSection;
    public Text outputText;
    public Text questionCountText;

    public string inputText;

    public GameObject nodePrefab;

    public string _output
    {
        get { return this.output; }
        set
        {
            this.output = value;
            this.outputText.text = value;
        }
    }

    public string _inputText
    {
        get { return this.inputText; }
        set { this.inputText = value; }
    }

    void Start()
    {
        //There's no need to ask for the initial data when it already exists
        if(File.Exists(Application.persistentDataPath + "/serialized.bin"))
        {
            tree = this.gameObject.AddComponent<BTTree>();
            tree.parent = this;
            this._output = "Tree loaded";
        }
        else
            startNewGame();

    }

    static bool playAgain()
    {
        Debug.Log("\nPlay Another Game? ");
        char inputCharacter = Console.ReadLine().ElementAt(0);
        inputCharacter = Char.ToLower(inputCharacter);
        while(inputCharacter != 'y' && inputCharacter != 'n')
        {
            Debug.Log("Incorrect input please enter again: ");
            inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
        }
        if(inputCharacter == 'y')
            return true;
        else
            return false;
    }

    static void startNewGame()
    {
        Debug.LogError("No previous knowledge found!\n" +
            "Initializing a new game.\n");
        Console.WriteLine("Enter a question about an object, person or animal: ");
        string question = Console.ReadLine();
        Console.Write("Enter a possible guess (an object, person or animal) if the response to this question is Yes: ");
        string yesGuess = Console.ReadLine();
        Console.Write("Enter a possible guess (an object, person or animal) if the response to this question is No: ");
        string noGuess = Console.ReadLine();

        tree = new BTTree(question, yesGuess, noGuess);
    }

    public void yesButtonPressed()
    {

    }

    public void noButtonPressed()
    {

    }

    public void confirmTextButtonPressed()
    {
        this._inputText = inputField.text;
    }

}

