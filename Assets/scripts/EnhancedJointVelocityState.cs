using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;


public class EnhancedJointVelocityState : JointVelocityActiveState
{
        /// <summary>
        /// 获取特定关节的速度。
        /// </summary>
        /// <param name="jointId">要查询的关节 ID。</param>
        /// <returns>关节的速度向量，如果关节未找到或无效，则返回零向量。</returns>
        /// 
        public Vector3 GetJointVelocity(HandJointId jointId)
        {
            if (Hand.GetJointPose(jointId, out Pose jointPose) &&
                JointDeltaProvider.GetPositionDelta(jointId, out Vector3 velocity))
            {
                return velocity / Time.deltaTime; // 转换为速度（单位/秒）。
            }
            return Vector3.zero;
        }
    public Vector3 GetJointposition(HandJointId jointId)
    {
        if (Hand.GetJointPose(jointId, out Pose jointPose) &&
            JointDeltaProvider.GetPositionDelta(jointId, out Vector3 velocity))
        {
            return velocity;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// 获取特定关节的方向向量。
    /// </summary>
    /// <param name="jointId">要查询的关节 ID。</param>
    /// <param name="relativeTo">方向的参考坐标系。</param>
    /// <returns>方向向量。</returns>
    public Vector3 GetJointDirection(HandJointId jointId, RelativeTo relativeTo)
        {
            if (!Hand.GetJointPose(jointId, out Pose jointPose))
            {
                return Vector3.zero;
            }

            switch (relativeTo)
            {
                case RelativeTo.Hand:
                    return jointPose.forward; // 相对于手的前向向量。
                case RelativeTo.World:
                    return jointPose.position.normalized; // 相对于世界的方向向量。
                case RelativeTo.Head:
                    if (Hmd != null && Hmd.TryGetRootPose(out Pose headPose))
                    {
                        return (jointPose.position - headPose.position).normalized; // 相对于头部的方向向量。
                    }
                    break;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// 获取所有关节的速度和方向状态。
        /// </summary>
        /// <returns>包含速度和方向的字典，键为关节配置。</returns>
        public Dictionary<HandJointId, (Vector3 Velocity, Vector3 Direction)> GetAllJointStates()
        {
            Dictionary<HandJointId, (Vector3 Velocity, Vector3 Direction)> jointStates =
                new Dictionary<HandJointId, (Vector3 Velocity, Vector3 Direction)>();

            foreach (var config in FeatureConfigs)
            {
                if (Hand.GetJointPose(config.Feature, out Pose _))
                {
                    Vector3 velocity = GetJointVelocity(config.Feature);
                    Vector3 direction = GetJointDirection(config.Feature, config.RelativeTo);
                    jointStates[config.Feature] = (velocity, direction);
                }
            }

            return jointStates;
        }
        public bool Ifactive()
        {
            return Active;
        }
    }

