using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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

    public enum SetupStates { question, objectYes, objectNo }

    public State state = State.readyToStart;

    public SetupStates setupStates = SetupStates.question;

    public string userQuestion, userObjectYes, userObjectNo;

    public State _state
    {
        get { return this.state; }
        set
        {
            this.state = value;
            switch (value)
            {
                case State.setupNewGameState:
                    this.inputFieldSection.SetActive(true);
                    this.buttonsSection.SetActive(false);
                    questionCountText.text = "No data found lets setup the start of the game!";
                    _setupStates = SetupStates.question;
                    break;
                case State.readyToStart:
                    this.inputFieldSection.SetActive(false);
                    this.buttonsSection.SetActive(true);
                    this.questionCountText.text = "Welcome to 20Q!";
                    _output = "Think up any object, person or animal. After such press Yes to start and I will start guessing at what you have in your mind! Press no to leave me :(";
                    //whenver we get here we want to save the tree.
                    tree.saveQuestionTree();
                    StartCoroutine(SpawnTree(1));
                    break;
                case State.playing:
                    ChangeActiveNode(tree.rootNode);
                    this.inputFieldSection.SetActive(false);
                    this.buttonsSection.SetActive(true);
                    break;
                case State.addingNewQuestion:
                    this.inputFieldSection.SetActive(true);
                    this.buttonsSection.SetActive(false);
                    this.questionCountText.text = "Looks like you won, could you help me get smarter?";
                    this._output = "What where you thinking of?";
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
                    this._output = "And what would be your answer to the question: " + this.userQuestion + " given you were thinking of " + this.userObject;
                    break;
                case State.addingNewQuestionObject:
                    this.inputFieldSection.SetActive(true);
                    this.buttonsSection.SetActive(false);
                    this._output = "What would be a question to distuingish a(n) " + currentActiveNode.message + " From a(n)" + this.userObject + "?";
                    break;
                default:
                    Debug.LogWarning("Problems have arised");
                    break;
            }
        }
    }

    public SetupStates _setupStates
    {
        get { return this.setupStates; }
        set
        {
            this.setupStates = value;
            switch (value)
            {
                case SetupStates.question:
                    _output = "Enter a question about an object, person or animal: ";
                    break;

            }
        }
    }

    public GameObject treeHolder;
    public GameObject UICanvas;

    public BTTree tree;
    [SerializeField]
    private string output;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private GameObject buttonsSection;
    [SerializeField]
    private GameObject inputFieldSection;
    public Text outputText;
    public Text questionCountText;
    int questionsAsked;
    public string userObject;

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
        if (File.Exists(Application.persistentDataPath + "/serialized.bin"))
        {
            tree = new BTTree();
            tree.parent = this;
            this._output = "Tree loaded";
            this._state = State.readyToStart;
        }
        else
            startNewGame();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            UICanvas.SetActive(!UICanvas.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            treeHolder.SetActive(!treeHolder.activeSelf);
        }
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
        while (inputCharacter != 'y' && inputCharacter != 'n')
        {
            Debug.Log("Incorrect input please enter again: ");
            inputCharacter = Console.ReadLine().ElementAt(0);
            inputCharacter = Char.ToLower(inputCharacter);
        }
        if (inputCharacter == 'y')
            return true;
        else
            return false;
    }

    void startNewGame()
    {
        this._state = State.setupNewGameState;
        Debug.LogError("No previous knowledge found!\n" +
            "Initializing a new game.\n");
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
                this.currentActiveNode.winsOnThisNode++;
                _state = State.readyToStart;
                break;
            case State.addingNewQuestionAnswer:
                //add new node
                currentActiveNode.setNoNode(new BTNode(currentActiveNode.message, currentActiveNode.traversedTimes, currentActiveNode.winsOnThisNode));
                currentActiveNode.setYesNode(new BTNode(userObject, 0, 0));
                //reset this node
                currentActiveNode.setMessage(this.userQuestion);
                currentActiveNode.ResetTrackingVars();
                _state = State.readyToStart;
                break;

        }
    }

    public void noButtonPressed()
    {
        switch (_state)
        {
            case State.readyToStart:
                Application.Quit();
                break;
            case State.playing:
                ChangeActiveNode(this.currentActiveNode.getNoNode());
                questionsAsked++;
                this.questionCountText.text = "I have but only asked " + questionsAsked + " questions";
                this._output = currentActiveNode.query(questionsAsked);
                break;
            case State.guessing:
                _state = State.addingNewQuestion;
                break;
            case State.addingNewQuestionAnswer:
                //add new node
                currentActiveNode.setYesNode(new BTNode(currentActiveNode.message, currentActiveNode.traversedTimes, currentActiveNode.winsOnThisNode));
                currentActiveNode.setNoNode(new BTNode(userObject, 0, 0));
                //reset this node
                currentActiveNode.setMessage(this.userQuestion);
                currentActiveNode.ResetTrackingVars();
                _state = State.readyToStart;
                break;
        }
    }

    public void confirmTextButtonPressed()
    {
        this._inputText = inputField.text;
        inputField.text = "";
        switch (_state)
        {
            case State.setupNewGameState:
                switch (setupStates)
                {
                    case SetupStates.question:
                        this.userQuestion = this._inputText;
                        this.setupStates = SetupStates.objectYes;
                        this.questionCountText.text = this.userQuestion;
                        this._output = "What would a plausible object be if you answered yes for this question?";
                        break;
                    case SetupStates.objectYes:
                        this.userObjectYes = this._inputText;
                        this.setupStates = SetupStates.objectNo;
                        this._output = "What would a plausible object be if you answered no for this question?";
                        break;
                    case SetupStates.objectNo:
                        this.userObjectNo = this._inputText;
                        tree = new BTTree(this.userQuestion, this.userObjectYes, this.userObjectNo);
                        this._state = State.readyToStart;
                        break;
                }
                break;
            case State.addingNewQuestion:
                this.userObject = this._inputText;
                this._state = State.addingNewQuestionObject;
                break;
            case State.addingNewQuestionObject:
                this.userQuestion = this._inputText;
                this._state = State.addingNewQuestionAnswer;
                break;

        }
    }


    public void ChangeActiveNode(BTNode nodeToChangeToo)
    {
        this.currentActiveNode = nodeToChangeToo;
        this.questionsAsked++;
        this.questionCountText.text = "This is question #: " + questionsAsked;
        this._output = currentActiveNode.query(questionsAsked);
    }

    IEnumerator SpawnTree(int maximumNodesToSpawn, float timeToWait = 0)
    {
        var nodesDeleted = 0;
        foreach (Transform child in treeHolder.transform)
        {
            Destroy(child.gameObject);//destroy all the children first
            nodesDeleted++;
            if (nodesDeleted > maximumNodesToSpawn)
            {
                nodesDeleted = 0;
                if (timeToWait == 0)
                {
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    yield return new WaitForSecondsRealtime(Math.Abs(timeToWait));
                }
            }
        }
        List<BTNode> discoveredNodes = new List<BTNode>
        {
            tree.rootNode
        };
        tree.rootNode.visualNode = Instantiate(nodePrefab, treeHolder.transform);
        tree.rootNode.visualNode.GetComponent<VisualNode>()._myNode = tree.rootNode;//rootnode has to be spawned on its own rules
        while (true)
        {
            for (int i = 0; i < maximumNodesToSpawn; i++)
            {
                if (discoveredNodes.Count == 0)
                {
                    Debug.Log("Coroutine has ended;");
                    yield break;
                }
                BTNode current = discoveredNodes[0];
                var instantiateFor = current.getYesNode();
                if (instantiateFor != null)
                {
                    discoveredNodes.Add(instantiateFor);
                    InstantiateNode(current.visualNode, instantiateFor, true);
                }

                instantiateFor = current.getNoNode();
                if (instantiateFor != null)
                {
                    discoveredNodes.Add(instantiateFor);
                    InstantiateNode(current.visualNode, instantiateFor, false);
                }
                discoveredNodes.Remove(current);

            }
            if (timeToWait == 0)
            {
                yield return new WaitForFixedUpdate();
            }
            else
            {
                yield return new WaitForSecondsRealtime(Math.Abs(timeToWait));
            }
        }
    }

    //all non root-nodes follow this route
    private void InstantiateNode(GameObject parentNode, BTNode childNode, bool left)
    {
        Debug.Log(parentNode.GetComponent<VisualNode>()._myNode.message + " parentNode message / childNode message " + childNode.message);
        var newNode = Instantiate(nodePrefab, treeHolder.transform);
        var parentVisualNodeScript = parentNode.GetComponent<VisualNode>();
        var newNodeVisualNodeScript = newNode.GetComponent<VisualNode>();
        childNode.visualNode = newNode;
        int direction = left ? -1 : 1;//convert bool into 1 or -1 for math purposes
        if (left)
        {
            parentVisualNodeScript.leftVisualNode = newNode.transform;
        }
        else
        {
            parentVisualNodeScript.rightVisualNode = newNode.transform;
        }
        var distanceOffset = parentVisualNodeScript.distanceFromParent / 2;
        distanceOffset.x /= 1.1f;//reduce it a tad more on the x axis to prevent branches hitting eachother
        newNodeVisualNodeScript.distanceFromParent = distanceOffset;
        newNode.transform.localScale = parentVisualNodeScript.transform.localScale / 2.2f;//total scalar reduction is now 2.2 (2 * 1.1)
        var parentLocation = parentVisualNodeScript.transform.position;
        var newNodeLocation = new Vector3(parentLocation.x + distanceOffset.x * direction, parentLocation.y + distanceOffset.y,
            parentLocation.z + distanceOffset.z * direction);
        newNode.transform.position = newNodeLocation;
        newNodeVisualNodeScript.widthMultipliers = parentVisualNodeScript.widthMultipliers / 2.2f;
        newNodeVisualNodeScript._myNode = childNode;//update message last
    }

}

