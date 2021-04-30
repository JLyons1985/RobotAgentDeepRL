using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RobotAgent : Agent
{
    [SerializeField] private RobotController controller;
    public override void OnEpisodeBegin()
    {
        controller.StopAllJointRotations();
        controller.StopAllJointVelocities();
        controller.ResetJoints();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // CG 3 floats
        sensor.AddObservation(controller.GetCenterOfGravity());
        // Head Joint Position and Velocity 9 floats
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Head).GetArticulationBody().gameObject.transform.localPosition);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Neck).GetArticulationBody().jointPosition[0]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Neck).GetArticulationBody().jointPosition[1]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Neck).GetArticulationBody().jointPosition[2]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Neck).GetArticulationBody().jointVelocity[0]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Neck).GetArticulationBody().jointVelocity[1]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Neck).GetArticulationBody().jointVelocity[2]);
        // Spine Position and Velocities 14 floats
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine01").GetArticulationBody().jointPosition[0]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine01").GetArticulationBody().jointPosition[1]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine01").GetArticulationBody().jointPosition[2]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine02").GetArticulationBody().jointPosition[0]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine02").GetArticulationBody().jointPosition[1]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine02").GetArticulationBody().jointPosition[2]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Thorax && joint.partName == "Spine03").GetArticulationBody().jointPosition[0]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine01").GetArticulationBody().jointVelocity[0]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine01").GetArticulationBody().jointVelocity[1]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine01").GetArticulationBody().jointVelocity[2]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine02").GetArticulationBody().jointVelocity[0]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine02").GetArticulationBody().jointVelocity[1]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine02").GetArticulationBody().jointVelocity[2]);
        sensor.AddObservation(controller.joints.Find(joint => joint.partType == RobotPartType.Thorax && joint.partName == "Spine03").GetArticulationBody().jointVelocity[0]);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        ActionSegment<float> conActions = actions.ContinuousActions;

        // // Neck 0 - 2
        // ArticulationJointController neckController = controller.joints.Find(joint => joint.partType == RobotPartType.Neck).GetJointController();
        // neckController.ChangeVelocityTo(conActions[0], RotationDrive.XDrive);
        // neckController.ChangeVelocityTo(conActions[1], RotationDrive.YDrive);
        // neckController.ChangeVelocityTo(conActions[2], RotationDrive.ZDrive);
        // // Spine01 3 - 5
        // ArticulationJointController spine01Controller = controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine01").GetJointController();
        // spine01Controller.ChangeVelocityTo(conActions[3], RotationDrive.XDrive);
        // spine01Controller.ChangeVelocityTo(conActions[4], RotationDrive.YDrive);
        // spine01Controller.ChangeVelocityTo(conActions[5], RotationDrive.ZDrive);
        // // Spine02 6 - 8
        // ArticulationJointController spine02Controller = controller.joints.Find(joint => joint.partType == RobotPartType.Abdomen && joint.partName == "Spine02").GetJointController();
        // spine02Controller.ChangeVelocityTo(conActions[6], RotationDrive.XDrive);
        // spine02Controller.ChangeVelocityTo(conActions[7], RotationDrive.YDrive);
        // spine02Controller.ChangeVelocityTo(conActions[8], RotationDrive.ZDrive);
        // // Spine03 9 - 11
        // ArticulationJointController spine03Controller = controller.joints.Find(joint => joint.partType == RobotPartType.Thorax && joint.partName == "Spine03").GetJointController();
        // spine03Controller.ChangeVelocityTo(conActions[9], RotationDrive.XDrive);
        // spine03Controller.ChangeVelocityTo(conActions[10], RotationDrive.YDrive);
        // spine03Controller.ChangeVelocityTo(conActions[11], RotationDrive.ZDrive);

        // Testing Biceps
        

        // Calculate rewards
        CalculateReward();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = continuousActions[3] = continuousActions[6] = continuousActions[9] = Input.GetAxisRaw("Pitch");
        continuousActions[1] = continuousActions[4] = continuousActions[7] = continuousActions[10] = Input.GetAxisRaw("Roll");
        continuousActions[2] = continuousActions[5] = continuousActions[8] = continuousActions[11] = Input.GetAxisRaw("Yaw");
    }

    private void CalculateReward() {
        Vector3 cg = controller.GetCenterOfGravity();
        Vector3 hips_cg = transform.InverseTransformVector(controller.joints.Find(joint => joint.partType == RobotPartType.Hips).GetArticulationBody().worldCenterOfMass);
        float distance_to_center_of_gravity = Vector3.Distance(new Vector3(cg.x, 0, cg.z), new Vector3(hips_cg.x, 0, hips_cg.z));
        if (distance_to_center_of_gravity < 0.05) {
            SetReward(0.2f);
        } else {
            SetReward(- distance_to_center_of_gravity);
        }
    }

    private void OnTriggerEnter(Collider other) {
               
    }

    private RotationVelocityDirection GetRotationDirection(float inputVal) {
        if (inputVal > 0)
        {
            return RotationVelocityDirection.Positive;
        }
        else if (inputVal < 0)
        {
            return RotationVelocityDirection.Negative;
        }
        else
        {
            return RotationVelocityDirection.None;
        }
    }

}
