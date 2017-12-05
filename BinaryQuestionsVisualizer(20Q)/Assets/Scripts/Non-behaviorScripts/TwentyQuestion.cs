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
    #region Singleton

    public static TwentyQuestion instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }

        instance = this;
    }

    #endregion

    public enum State { setupNewGameState, readyToStart, playing, addingNewQuestion, addingNewQuestionAnswer, addingNewQuestionObject, guessing }

    public State state = State.readyToStart;

    public State _state
    {
        get { return this.state; }
        set {
            this.state = value;
            switch (value)
            {
                case State.setupNewGameState:
                    this.inputFieldSection.SetActive(true);
                    this.buttonsSection.SetActive(false);
                    break;
                case State.readyToStart:
                    this.inputFieldSection.SetActive(false);
                    this.buttonsSection.SetActive(true);
                    break;
                case State.playing:
                    this.inputFieldSection.SetActive(false);
                    this.buttonsSection.SetActive(true);
                    break;
                case State.addingNewQuestion:
                    this.inputFieldSection.SetActive(true);
                    this.buttonsSection.SetActive(false);
                    this.questionCountText.text = "Looks like you won, could you help me get smarter?";
                    break;
                case State.guessing:
                    this.inputFieldSection.SetActive(false);
                    this.buttonsSection.SetActive(true);
                    this.questionCountText.text = "Hmmm I sense you are thinking about:";
                    break;
                case State.addingNewQuestionAnswer:
                    this.inputFieldSection.SetActive(false);
                    this.buttonsSection.SetActive(true);
                    this.questionCountText.text = "";
                    break;
                case State.addingNewQuestionObject:
                    this.inputFieldSection.SetActive(true);
                    this.buttonsSection.SetActive(false);
                    break;
                default:
                    Debug.LogWarning("Problems have arised");
                    break;
            }
        }
    }

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
    int questionsAsked;
    string userQuestion;
    string userObject;

    public BTNode currentActiveNode;

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
            this._state = State.readyToStart;
        }
        else
            startNewGame();

    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.playing:
                break;
        }
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
        switch (_state)
        {
            case State.readyToStart:
                _state = State.playing;
                currentActiveNode = tree.rootNode;
                this.questionsAsked = 0;
                break;
            case State.playing:
                ChangeActiveNode(this.currentActiveNode.getYesNode());
                break;
            case State.guessing:
                _output = "Haha! I win!!!";
                _state = State.readyToStart;
                break;

        }
    }

    public void noButtonPressed()
    {
        switch (_state)
        {
            case State.readyToStart:
                _state = State.playing;
                currentActiveNode = tree.rootNode;
                questionsAsked = 1;
                this.questionCountText.text = "I have but only asked " + questionsAsked + " questions";
                this._output = currentActiveNode.query(questionsAsked);
                break;
            case State.playing:
                ChangeActiveNode(this.currentActiveNode.getNoNode());
                break;
            case State.guessing:
                _state = State.addingNewQuestion;
                    break;

        }
    }

    public void confirmTextButtonPressed()
    {
        this._inputText = inputField.text;
    }


    public void ChangeActiveNode(BTNode nodeToChangeToo)
    {
        this.currentActiveNode = nodeToChangeToo;
        this.questionsAsked++;
        this.questionCountText.text = "I have but only asked " + questionsAsked + " questions";
        this._output = currentActiveNode.query(questionsAsked);
    }

}

