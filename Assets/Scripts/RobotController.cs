using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public enum RobotPartType {
    Hips,
    Thigh,
    Calf,
    Foot,
    Toe,
    Abdomen,
    Thorax,
    Neck,
    Head,
    Clavicle,
    Upperarm,
    Lowerarm,
    Hand,
    Finger,
}

[ExecuteInEditMode]
public class RobotController : MonoBehaviour
{
    [System.Serializable]
    public struct Joint {
        public RobotPartType partType;
        public string partName;
        public GameObject robotPart;
        public GameObject proximal_flexor_tendon;
        public GameObject distal_flexor_tendon;

        public ArticulationBody GetArticulationBody() {
            return robotPart.GetComponent<ArticulationBody>();
        }

        public ArticulationJointController GetJointController() {
            return robotPart.GetComponent<ArticulationJointController>();
        }
    }
    
    [SerializeField] public List<Joint> joints;

    [Header("Parameters")]
    [SerializeField] float mass;

    #region Part Masses Variables
    private float hips_f = 0.1077f;
    private float thighs_f = 0.1416f;
    private float calves_f = 0.0433f;
    private float feet_f = 0.0127f;
    private float toes_f = 0.001f;
    private float abdomen_f = 0.00515f;
    private float thorax_f = 0.1585f;
    private float clavicles_f = 0.00001f;
    private float upperarms_f = 0.0271f;
    private float lowerarms_f = 0.0162f;
    private float hands_f = 0.004875f;
    private float fingers_f = 0.000108f;
    private float neck_f = 0.0094f;
    private float head_f = 0.06f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetMassForAllParts();
    }

    // Update is called once per frame
    void Update()
    {
        GetCenterOfGravity();
    }

    public void ResetJoints() {
        foreach (Joint joint in joints) {
            joint.GetJointController().RestoreState();
        }
    }

    // Gets the center of gravity for the whole object
    public Vector3 GetCenterOfGravity() {
        float allMass = 0;
        Vector3 allCentroid = new Vector3();
        foreach (Joint joint in joints){
            allMass += joint.GetArticulationBody().mass;
            allCentroid += joint.GetArticulationBody().worldCenterOfMass * joint.GetArticulationBody().mass;
        }
        allCentroid /= allMass;
        Vector3 localAllCentroid = transform.InverseTransformVector(allCentroid);
        Debug.DrawLine(allCentroid, new Vector3(allCentroid.x, 0, allCentroid.z), Color.red, 0.1f);
        ArticulationBody hips = joints.Find(joint => joint.partType == RobotPartType.Hips).GetArticulationBody();
        Debug.DrawLine(hips.worldCenterOfMass, new Vector3(hips.worldCenterOfMass.x, 0, hips.worldCenterOfMass.z), Color.yellow, 0.1f);
        return localAllCentroid;
    }

    // Rotate a specific joint
    public void RotateJoint(Joint joint, RotationVelocityDirection direction, RotationDrive driveType) {
        UpdateRotationState(direction, driveType, joint.robotPart);
    }

    // Set velocity for specific joint
    public void SetVelocityOnJoint(Joint joint, float velocity, RotationDrive driveType) {
        joint.GetJointController().ChangeVelocityTo(velocity, driveType);
    }

    public void AddForceAtPosition(Joint joint, Vector3 force, Vector3 worldPosition)
    {
        joint.GetJointController().AddForceAtPosition(force, worldPosition);
    }

    public void StopAllJointRotations()
    {
        foreach (Joint joint in joints)
        {
            GameObject robotPart = joint.robotPart;
            UpdateRotationState(RotationVelocityDirection.None, RotationDrive.XDrive, robotPart);
            UpdateRotationState(RotationVelocityDirection.None, RotationDrive.YDrive, robotPart);
            UpdateRotationState(RotationVelocityDirection.None, RotationDrive.ZDrive, robotPart);
        }
    }

    public void StopAllJointVelocities()
    {
        foreach (Joint joint in joints)
        {
            GameObject robotPart = joint.robotPart;
            joint.GetJointController().ChangeVelocityTo(0, RotationDrive.XDrive);
            joint.GetJointController().ChangeVelocityTo(0, RotationDrive.YDrive);
            joint.GetJointController().ChangeVelocityTo(0, RotationDrive.ZDrive);
        }
    }


    private void SetMassForAllParts() {
        // Set the mass for all the bodies
        foreach (Joint joint in joints){
            switch (joint.partType) {
                case RobotPartType.Hips:
                    joint.GetArticulationBody().mass = mass * hips_f;
                    break;
                case RobotPartType.Thigh:
                    joint.GetArticulationBody().mass = mass * thighs_f;
                    break;
                case RobotPartType.Calf:
                    joint.GetArticulationBody().mass = mass * calves_f;
                    break;
                case RobotPartType.Foot:
                    joint.GetArticulationBody().mass = mass * feet_f;
                    break;
                case RobotPartType.Toe:
                    joint.GetArticulationBody().mass = mass * toes_f;
                    break;
                case RobotPartType.Abdomen:
                    joint.GetArticulationBody().mass = mass * abdomen_f;
                    break;
                case RobotPartType.Thorax:
                    joint.GetArticulationBody().mass = mass * thorax_f;
                    break;
                case RobotPartType.Neck:
                    joint.GetArticulationBody().mass = mass * neck_f;
                    break;
                case RobotPartType.Head:
                    joint.GetArticulationBody().mass = mass * head_f;
                    break;
                case RobotPartType.Clavicle:
                    joint.GetArticulationBody().mass = mass * clavicles_f;
                    break;
                case RobotPartType.Upperarm:
                    joint.GetArticulationBody().mass = mass * upperarms_f;
                    break;
                case RobotPartType.Lowerarm:
                    joint.GetArticulationBody().mass = mass * lowerarms_f;
                    break;
                case RobotPartType.Hand:
                    joint.GetArticulationBody().mass = mass * hands_f;
                    break;
                case RobotPartType.Finger:
                    joint.GetArticulationBody().mass = mass * fingers_f;
                    break;
            }
        }
    }

    // HELPERS

    static void UpdateRotationState(RotationVelocityDirection direction, RotationDrive driveType, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        switch (driveType) {
            case RotationDrive.XDrive:
                jointController.rotationXState = direction;
                break;
            case RotationDrive.YDrive:
                jointController.rotationYState = direction;
                break;
            case RotationDrive.ZDrive:
                jointController.rotationZState = direction;
                break;
        }        
    }
}
