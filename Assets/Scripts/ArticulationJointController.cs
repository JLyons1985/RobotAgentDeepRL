using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationVelocityDirection { None = 0, Positive = 1, Negative = -1 };
public enum RotationDrive { XDrive = 0, YDrive = 1, ZDrive = 2 };

public class ArticulationJointController : MonoBehaviour
{
    public RotationVelocityDirection rotationXState = RotationVelocityDirection.None;
    public RotationVelocityDirection rotationYState = RotationVelocityDirection.None;
    public RotationVelocityDirection rotationZState = RotationVelocityDirection.None;

    public ArticulationReducedSpace jointPosition;
    public ArticulationReducedSpace jointVelocity;

    public float velocityXState = 0.0f;
    public float velocityYState = 0.0f;
    public float velocityZState = 0.0f;
    public float speed = 500.0f;

    private ArticulationBody articulation;


    // LIFE CYCLE

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();

        // Save initial state
        SaveState();
    }

    void SaveState() {
        jointPosition = articulation.jointPosition;
        jointVelocity = articulation.jointVelocity;
    }

    public void RestoreState() {
        articulation.jointPosition = jointPosition;
        articulation.jointVelocity = jointVelocity;
    }

    void FixedUpdate() 
    {
        if (rotationXState != RotationVelocityDirection.None) {
            float rotationChange = (float)rotationXState * speed * Time.fixedDeltaTime;
            float rotationGoal = CurrentPrimaryAxisRotation(0) + rotationChange;
            RotateTo(rotationGoal, RotationDrive.XDrive);
        }

        if (rotationYState != RotationVelocityDirection.None) {
            float rotationChange = (float)rotationYState * speed * Time.fixedDeltaTime;
            float rotationGoal = CurrentPrimaryAxisRotation(1) + rotationChange;
            RotateTo(rotationGoal, RotationDrive.YDrive);
        }

        if (rotationZState != RotationVelocityDirection.None) {
            float rotationChange = (float)rotationZState * speed * Time.fixedDeltaTime;
            float rotationGoal = CurrentPrimaryAxisRotation(0) + rotationChange;
            RotateTo(rotationGoal, RotationDrive.ZDrive);
        }
    }


    // MOVEMENT HELPERS

    float CurrentPrimaryAxisRotation(int jointIndex)
    {
        float currentRotationRads = articulation.jointPosition[jointIndex];
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        return currentRotation;
    }

    void RotateTo(float primaryAxisRotation, RotationDrive driveType)
    {
        ArticulationDrive drive = articulation.xDrive;
        switch (driveType) {
            case RotationDrive.XDrive:
                drive = articulation.xDrive;
                drive.target = primaryAxisRotation;
                articulation.xDrive = drive;
                break;
            case RotationDrive.YDrive:
                drive = articulation.yDrive;
                drive.target = primaryAxisRotation;
                articulation.yDrive = drive;
                break;
            case RotationDrive.ZDrive:
                drive = articulation.zDrive;
                drive.target = primaryAxisRotation;
                articulation.zDrive = drive;
                break;
        }
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }

    public void AddForceAtPosition(Vector3 force, Vector3 worldPosition)
    {
        var lever = worldPosition - articulation.centerOfMass;
        articulation.AddTorque(Vector3.Cross(lever, force));
        articulation.AddForce(force);
    }

    public void ChangeVelocityTo(float axisVelocity, RotationDrive driveType)
    {
        ArticulationDrive drive = articulation.xDrive;
        switch (driveType) {
            case RotationDrive.XDrive:
                drive = articulation.xDrive;
                drive.targetVelocity = axisVelocity * speed;
                articulation.xDrive = drive;
                break;
            case RotationDrive.YDrive:
                drive = articulation.yDrive;
                drive.targetVelocity = axisVelocity * speed;
                articulation.yDrive = drive;
                break;
            case RotationDrive.ZDrive:
                drive = articulation.zDrive;
                drive.targetVelocity = axisVelocity * speed;
                articulation.zDrive = drive;
                break;
        }
        
    }


}
