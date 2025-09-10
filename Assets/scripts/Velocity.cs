using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
public class Velocity : JointVelocityActiveState  // 继承JointVelocityActiveState
{
    // 可以在此处定义需要输出的关节
    [Tooltip("The specific joint to log the velocity for.")]
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandIndexTip;  // 默认选取某个关节（例如：手指尖）
    [SerializeField]
    private JointVelocityData jointVelocityData;
    public TextMeshProUGUI t;
    // 在每次更新时输出关节速度和方向
    protected override void Start()
    {
        base.Start(); // 调用父类的 Start 方法进行初始化
    }

    protected override void Update()
    {
        base.Update();
        // 每帧更新时，检查并输出目标关节的速度和方向
        //LogJointVelocity();
        UpdateJointVelocity();
    }
    // 更新关节的速度和方向
    private void UpdateJointVelocity()
    {
        // 检查是否激活
       // if (Active)
        //{
            // 检查 FeatureStates 字典中是否有该关节的速度信息
            foreach (var featureStatePair in FeatureStates)
            {
                var config = featureStatePair.Key;
                var featureState = featureStatePair.Value;

                // 如果当前关节是用户选择的关节
                if (config.Feature == _jointToLog)
                {
                    // 更新 JointVelocityData 中的速度和方向
                    jointVelocityData.UpdateVelocity(
                        featureState.Amount * featureState.TargetVector, // 计算速度
                        featureState.TargetVector // 方向

                    );
                    t.text = (featureState.Amount * featureState.TargetVector).ToString();

                    return;
                }
            }

            // 如果没有找到该关节的速度数据，输出提示
            //Debug.LogWarning($"Joint {_jointToLog} not found in the FeatureStates.");
       //}
        /*else
        {
            Debug.Log("Active state is not reached yet, no velocity data available.");
        }*/
    }
    // 输出当前选择的关节的速度和方向
    /*private void LogJointVelocity()
    {
        // 检查是否激活
        if (Active)
        {
            // 检查 FeatureStates 字典中是否有该关节的速度信息
            foreach (var featureStatePair in FeatureStates)
            {
                var config = featureStatePair.Key;
                var featureState = featureStatePair.Value;

                // 如果当前关节是用户选择的关节
                if (config.Feature == _jointToLog)
                {
                    // 输出关节的速度和方向
                    Debug.Log($"Joint: {_jointToLog}, Velocity: {featureState.Amount} units/s, Direction: {featureState.TargetVector}");
                    return;
                }
            }

            // 如果没有找到该关节的速度数据，输出提示
            Debug.LogWarning($"Joint {_jointToLog} not found in the FeatureStates.");
        }
        else
        {
            Debug.Log("Active state is not reached yet, no velocity data available.");
        }
    }*/

}
