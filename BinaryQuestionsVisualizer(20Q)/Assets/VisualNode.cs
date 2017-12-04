using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualNode : MonoBehaviour {

    [SerializeField]
    private Text nodeNameText;
    [SerializeField]
    private Text nodeInfoText;
    public BTNode myNode;

    public Transform leftVisualNode;
    public Transform rightVisualNode;

    private LineRenderer leftVisualNodeLineRenderer;
    private LineRenderer rightVisualNodeLineRenderer;

    [SerializeField] private Gradient leftColorGradient;
    [SerializeField] private Gradient rightColorGradient;

    [SerializeField] private AnimationCurve leftWidtCurve;
    [SerializeField] private AnimationCurve rightWidtCurve;

    [Range(1, 200)][SerializeField] private float widthMultipliers;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if(leftVisualNode != null)
        {
            var vectorArray = new Vector3[2];
            vectorArray[0] = transform.position;
            vectorArray[1] = leftVisualNode.position;
            leftVisualNodeLineRenderer.SetPositions(vectorArray);
            //move these three to start later
            leftVisualNodeLineRenderer.colorGradient = leftColorGradient;
            leftVisualNodeLineRenderer.widthCurve = leftWidtCurve;
            leftVisualNodeLineRenderer.widthMultiplier = widthMultipliers;
        }
        if(rightVisualNode)
        {
            var vectorArray = new Vector3[2];
            vectorArray[0] = transform.position;
            vectorArray[1] = rightVisualNode.position;
            rightVisualNodeLineRenderer.SetPositions(vectorArray);
            //move these three to start later
            rightVisualNodeLineRenderer.colorGradient = rightColorGradient;
            rightVisualNodeLineRenderer.widthCurve = rightWidtCurve;
            rightVisualNodeLineRenderer.widthMultiplier = widthMultipliers;
        }
    }
}
