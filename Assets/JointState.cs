using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointState : MonoBehaviour
{
    public ArticulationBody ab;

    public ArticulationReducedSpace jointPosition;
    public ArticulationReducedSpace jointVelocity;

    // Start is called before the first frame update
    void Start()
    {
        ab = GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveAllJointStates()
    {
        foreach (var state in FindObjectsOfType<JointState>())
        {
            state.SaveJointState();
        }
    }

    public void RestoreAllJointStates()
    {
        foreach (var state in FindObjectsOfType<JointState>())
        {
            state.RestoreJointState();
        }
    }

    public void SaveJointState()
    {
        Debug.Log($"saving joint state of {name}", gameObject);
        if (ab.jointType == ArticulationJointType.RevoluteJoint) {
            jointPosition = ab.jointPosition;
            jointVelocity = ab.jointVelocity;
        }
    }

    public void RestoreJointState()
    {
        Debug.Log($"loading joint state of {name}", gameObject);
        ab.jointPosition = jointPosition;
        ab.jointVelocity = jointVelocity;
    }

    public void Unparent()
    {
        transform.SetParent(null, true);
    }

    public void UnparentWithReenable()
    {
        transform.SetParent(null, true);
        ReenableAll();
    }

    void ReenableAll()
    {
        foreach (var ab in FindObjectsOfType<ArticulationBody>())
        {
            if (ab.isRoot)
            {
                Debug.Log($"Reenabling {ab.name}", ab);
                ab.enabled = false;
                ab.enabled = true;
                ab.WakeUp();
            }
        }
    }
}
