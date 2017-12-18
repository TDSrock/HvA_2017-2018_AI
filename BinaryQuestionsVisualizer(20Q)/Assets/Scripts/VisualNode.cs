using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class VisualNode : MonoBehaviour {
    [SerializeField]
    private Text nodeNameText;
    [SerializeField]
    private Text nodeInfoText;
    [SerializeField]
    private BTNode myNode;

    public BTNode _myNode
    {
        get { return this.myNode; }
        set
        {
            this.myNode = value;
            if(value != null)
                this.UpdateMessage();
        }
    }
    public Vector3 distanceFromParent = new Vector3(10, -10, 0);
    [Header("Binding Line Data")]
    [HideInInspector]
    public Transform leftVisualNode;
    [HideInInspector]
    public Transform rightVisualNode;

    private LineRenderer leftVisualNodeLineRenderer;
    private LineRenderer rightVisualNodeLineRenderer;
    [Header("Color gradients")]
    [SerializeField] private Gradient leftColorGradient;
    [SerializeField] private Gradient rightColorGradient;
    [Header("Width curves")]
    [SerializeField] private AnimationCurve leftWidtCurve;
    [SerializeField] private AnimationCurve rightWidtCurve;
    [Header("Width detail")]
    [Range(2,100)]public int curveVertexes = 50;

    [Header("Width multiplier(affects every vertex)")]
    [Range(1, 500)]public float widthMultipliers;

    // Use this for initialization
    void Start () {
        leftVisualNodeLineRenderer = this.gameObject.GetComponentsInChildren<LineRenderer>()[0];
        rightVisualNodeLineRenderer = this.gameObject.GetComponentsInChildren<LineRenderer>()[1];
    }

    //set the text fields of this instance
    void UpdateMessage ()
    {
        nodeNameText.text = myNode.getMessage();
        StringBuilder infoText = new StringBuilder();//useing a string builder, it's append funtion is O(1) instead of string concating which is O(n)
        if (!myNode.isQuestion())
        {
            infoText.Append("No children nodes I am a leaf node!");
            infoText.AppendLine();
        }
        else
        {
            infoText.Append("I am a question node");
            infoText.AppendLine();
            var node = myNode.getYesNode();
            if (node != null)
            {
                infoText.Append("My yes node is: " + node.getMessage());
            }
            else
            {
                infoText.Append("I do not have a yes node");
            }
            infoText.AppendLine();
            node = myNode.getNoNode();
            if(node != null)
            {
                infoText.Append("My no node is: " + node.getMessage());
            }
            else
            {
                infoText.Append("I do not have a no node");
            }
            infoText.AppendLine();
        }
        infoText.Append("I have been traversed " + myNode.traversedTimes + " times, crazy huh?");
        if(!_myNode.isQuestion())
        {
            infoText.Append("I have won on this " + _myNode.winsOnThisNode + " times");
        }
        nodeInfoText.text = infoText.ToString();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //render the left line
        if(leftVisualNode != null)
        {
            leftVisualNodeLineRenderer.positionCount = this.curveVertexes;
            leftVisualNodeLineRenderer.SetPositions((CalcLineVertexes(this.transform, leftVisualNode, this.curveVertexes)));
            //move these three to start later
            leftVisualNodeLineRenderer.colorGradient = leftColorGradient;
            leftVisualNodeLineRenderer.widthCurve = leftWidtCurve;
            leftVisualNodeLineRenderer.widthMultiplier = widthMultipliers;
        }
        //render the right line
        if(rightVisualNode != null)
        {
            rightVisualNodeLineRenderer.positionCount = this.curveVertexes;
            rightVisualNodeLineRenderer.SetPositions(CalcLineVertexes(this.transform, rightVisualNode, this.curveVertexes));
            //move these three to start later
            rightVisualNodeLineRenderer.colorGradient = rightColorGradient;
            rightVisualNodeLineRenderer.widthCurve = rightWidtCurve;
            rightVisualNodeLineRenderer.widthMultiplier = widthMultipliers;
        }
    }
    
    //calculate the vertex possitions
    private Vector3[] CalcLineVertexes(Transform myself, Transform linePoint, int vertexDetail)
    {
        //TODO: Make this method throw an error of vertexDetail is lower then 2
        Vector3[] r = new Vector3[vertexDetail];
        r[0] = myself.position;//always start at self
        var lineScaling = (linePoint.position - myself.position).normalized;
        for(float i = 1; i < vertexDetail - 1; i++)
        {
            r[(int)i] = Vector3.Lerp(myself.position, linePoint.position, (i / vertexDetail));
        }
        r[vertexDetail - 1] = linePoint.position;//always end at the goal location
        return r;
    }
}
