using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
using Oculus.Interaction;
public class SyncObjectWithVelocity : MonoBehaviour
{
    public TextMeshProUGUI t;
    [SerializeField]
    //private JointVelocityData jointVelocityData;  
    private Rigidbody _rigidbody;
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandStart;
    [SerializeField]
    private JointVelocityActiveState.RelativeTo _relativeTo = JointVelocityActiveState.RelativeTo.World;
    public EnhancedJointVelocityState velocityState;
    public ActiveStateGroup activestategroup;
    public float sensitivity = 10f;
    private void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update(){
        //if (jointVelocityData != null)
        //{
            // 获取关节的速度和方向
            //Vector3 velocity = jointVelocityData.velocity;
            //Vector3 direction = jointVelocityData.direction;
            //EnhancedJointVelocityState velocityState = e.GetComponent<EnhancedJointVelocityState>();
        if (activestategroup.Active)
        {
            // 获取特定关节的速度和方向。
            Vector3 wristVelocity = velocityState.GetJointVelocity(_jointToLog);
            Vector3 wristDirection = velocityState.GetJointDirection(_jointToLog, _relativeTo);
            Vector3 wristPosition = velocityState.GetJointposition(_jointToLog);

            //t.text = wristVelocity.ToString();
            /*// 获取所有关节的状态。
            var jointStates = velocityState.GetAllJointStates();
            foreach (var jointState in jointStates)
            {
                Debug.Log($"Joint: {jointState.Key}, Velocity: {jointState.Value.Velocity}, Direction: {jointState.Value.Direction}");
            }*/

            t.text = wristPosition.ToString();
            //Debug.Log(velocity);
            //Debug.Log(direction);
            // 将物体的速度设置为关节的速度
            if (_rigidbody != null)
            {
                _rigidbody.velocity = wristVelocity * sensitivity;
                //transform.position = wristPosition * sensitivity;

                transform.forward = wristDirection.normalized; // 设置物体的前进方向

            }
        }
        //}
    }
}
