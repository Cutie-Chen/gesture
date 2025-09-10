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
    private RectTransform _uiElement; // UI��RectTransform���
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandStart;
   
    [SerializeField]
    private JointVelocityActiveState.RelativeTo _relativeTo = JointVelocityActiveState.RelativeTo.World;
    public EnhancedJointVelocityState velocityState;
    //public ActiveStateGroup activestategroup;
    public float sensitivity = 5f; // ����UI�˶������ж�
    private GameObject selectedObject = null; // ��ǰ��ѡ�е�UIԪ��
    //public Camera worldCamera; // ���ڼ�������������
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
                // ��ȡ�ض��ؽڵ��ٶȺͷ���
                Vector3 wristPosition = velocityState.GetJointposition(_jointToLog);
                Vector3 wristVelocity = velocityState.GetJointVelocity(_jointToLog);
                Vector3 wristDirection = velocityState.GetJointDirection(_jointToLog, _relativeTo);

              

                _uiElement.anchoredPosition += new Vector2(wristPosition.x, wristPosition.y) * sensitivity;

            }

            

        }
    }
        
   

