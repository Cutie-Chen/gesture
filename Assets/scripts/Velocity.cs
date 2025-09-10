using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
public class Velocity : JointVelocityActiveState  // �̳�JointVelocityActiveState
{
    // �����ڴ˴�������Ҫ����Ĺؽ�
    [Tooltip("The specific joint to log the velocity for.")]
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandIndexTip;  // Ĭ��ѡȡĳ���ؽڣ����磺��ָ�⣩
    [SerializeField]
    private JointVelocityData jointVelocityData;
    public TextMeshProUGUI t;
    // ��ÿ�θ���ʱ����ؽ��ٶȺͷ���
    protected override void Start()
    {
        base.Start(); // ���ø���� Start �������г�ʼ��
    }

    protected override void Update()
    {
        base.Update();
        // ÿ֡����ʱ����鲢���Ŀ��ؽڵ��ٶȺͷ���
        //LogJointVelocity();
        UpdateJointVelocity();
    }
    // ���¹ؽڵ��ٶȺͷ���
    private void UpdateJointVelocity()
    {
        // ����Ƿ񼤻�
       // if (Active)
        //{
            // ��� FeatureStates �ֵ����Ƿ��иùؽڵ��ٶ���Ϣ
            foreach (var featureStatePair in FeatureStates)
            {
                var config = featureStatePair.Key;
                var featureState = featureStatePair.Value;

                // �����ǰ�ؽ����û�ѡ��Ĺؽ�
                if (config.Feature == _jointToLog)
                {
                    // ���� JointVelocityData �е��ٶȺͷ���
                    jointVelocityData.UpdateVelocity(
                        featureState.Amount * featureState.TargetVector, // �����ٶ�
                        featureState.TargetVector // ����

                    );
                    t.text = (featureState.Amount * featureState.TargetVector).ToString();

                    return;
                }
            }

            // ���û���ҵ��ùؽڵ��ٶ����ݣ������ʾ
            //Debug.LogWarning($"Joint {_jointToLog} not found in the FeatureStates.");
       //}
        /*else
        {
            Debug.Log("Active state is not reached yet, no velocity data available.");
        }*/
    }
    // �����ǰѡ��Ĺؽڵ��ٶȺͷ���
    /*private void LogJointVelocity()
    {
        // ����Ƿ񼤻�
        if (Active)
        {
            // ��� FeatureStates �ֵ����Ƿ��иùؽڵ��ٶ���Ϣ
            foreach (var featureStatePair in FeatureStates)
            {
                var config = featureStatePair.Key;
                var featureState = featureStatePair.Value;

                // �����ǰ�ؽ����û�ѡ��Ĺؽ�
                if (config.Feature == _jointToLog)
                {
                    // ����ؽڵ��ٶȺͷ���
                    Debug.Log($"Joint: {_jointToLog}, Velocity: {featureState.Amount} units/s, Direction: {featureState.TargetVector}");
                    return;
                }
            }

            // ���û���ҵ��ùؽڵ��ٶ����ݣ������ʾ
            Debug.LogWarning($"Joint {_jointToLog} not found in the FeatureStates.");
        }
        else
        {
            Debug.Log("Active state is not reached yet, no velocity data available.");
        }
    }*/

}
