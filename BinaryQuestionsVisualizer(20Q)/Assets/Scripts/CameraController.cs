using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    GameObject viewingNode;
    public Vector3 initialOffset;
    public Vector3 maxOffset;
    public Vector3 offset;
    public Vector3 scrollSpeed;
    public Vector3 offsetScale;
	// Use this for initialization
	void Start () {
        viewingNode = TwentyQuestion.instance.tree.rootNode.visualNode;
	}
	
	// Update is called once per frame
	void Update () {
        offset += Input.mouseScrollDelta.x * scrollSpeed;
        offset = Clamp(offset, maxOffset, -maxOffset);
    }

    public Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
    {
        value.x = Mathf.Clamp(value.x, min.x, max.x);
        value.y = Mathf.Clamp(value.y, min.y, max.y);
        value.z = Mathf.Clamp(value.z, min.z, max.z);
        return value;
    }
}
