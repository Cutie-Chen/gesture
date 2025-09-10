using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
using Oculus.Interaction;


public class LogBoneLocation : MonoBehaviour
{
    [SerializeField]
    private Hand hand;
    private Pose currentPose;
    [SerializeField]
    private HandJointId handJointId = HandJointId.HandStart; // TO DO: Change this to your bone.
    public TextMeshProUGUI t;
    [SerializeField]
    private RectTransform _uiElement; // UI的RectTransform组件
    [SerializeField]
    private JointVelocityActiveState.RelativeTo _relativeTo = JointVelocityActiveState.RelativeTo.World;
    public EnhancedJointVelocityState velocityState;
    public ActiveStateGroup activestategroup;
    //public choose_one choose;
    public float sensitivity = 10f; // 调整UI运动的敏感度
    void Update()
    {
        if (activestategroup.Active)
        {
            hand.GetJointPose(handJointId, out currentPose);
            _uiElement.anchoredPosition = new Vector2(currentPose.position.x, currentPose.position.y) ;
            
        }
    }
}