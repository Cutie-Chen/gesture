using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
using Oculus.Interaction;

public class SyncUIWithVelocity : MonoBehaviour
{
    //public TextMeshProUGUI t;
    [SerializeField]
    private RectTransform _uiElement; // UI的RectTransform组件
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandStart;
   
    [SerializeField]
    private JointVelocityActiveState.RelativeTo _relativeTo = JointVelocityActiveState.RelativeTo.World;
    public EnhancedJointVelocityState velocityState;
    //public ActiveStateGroup activestategroup;
    public float sensitivity = 5f; // 调整UI运动的敏感度
    private GameObject selectedObject = null; // 当前被选中的UI元素
    //public Camera worldCamera; // 用于计算的世界摄像机
   bool T3_bool=false;
    [SerializeField]
    public ActiveStateGroup T5;
    public ActiveStateGroup T9;
    private void Awake()
    {
        if (_uiElement == null)
        {
            Debug.LogError("UI Element's RectTransform is not assigned!");
        }
    }
    public void T3()
    {
        T3_bool = true;
    }
    public void notT3()
    {
        T3_bool = false;
    }
    private void Update()
    {
       
            if (T3_bool&&!T5&&!T9)
            {
                // 获取特定关节的速度和方向。
                Vector3 wristPosition = velocityState.GetJointposition(_jointToLog);
                Vector3 wristVelocity = velocityState.GetJointVelocity(_jointToLog);
                Vector3 wristDirection = velocityState.GetJointDirection(_jointToLog, _relativeTo);

              

                _uiElement.anchoredPosition += new Vector2(wristPosition.x, wristPosition.y) * sensitivity;

            }

            

        }
    }
        
   

