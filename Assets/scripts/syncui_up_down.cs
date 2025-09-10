using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
using Oculus.Interaction;
using UnityEngine.UI;

public class syncui_up_down : MonoBehaviour
{
    public TextMeshProUGUI t;
    [SerializeField]
    private RectTransform _uiElement; // UI��RectTransform���
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandStart;
    [SerializeField]
    private JointVelocityActiveState.RelativeTo _relativeTo = JointVelocityActiveState.RelativeTo.World;
    public EnhancedJointVelocityState velocityState;
    public ActiveStateGroup activestategroup;
    public float sensitivity = 10f; // ����UI�˶������ж�
    //public Camera worldCamera; // ���ڼ�������������
    //public Image image;
    private void Awake()
    {
        if (_uiElement == null)
        {
            Debug.LogError("UI Element's RectTransform is not assigned!");
        }
        
    }

    private void Update()
    {
        if (activestategroup.Active)
        {
            // ��ȡ�ض��ؽڵ��ٶȺͷ���
            Vector3 wristPosition = velocityState.GetJointposition(_jointToLog);
            Vector3 wristVelocity = velocityState.GetJointVelocity(_jointToLog);
            Vector3 wristDirection = velocityState.GetJointDirection(_jointToLog, _relativeTo);

            // ���UIԪ�ر����䣬��ͬ����λ��
            if (_uiElement != null)
            {


                /* // ����������ת��Ϊ��Ļ����
                 Vector3 screenPosition = worldCamera.WorldToScreenPoint(wristPosition);

                 // ����Ļ����ת��ΪUI����
                 Vector2 uiPosition;
                 RectTransformUtility.ScreenPointToLocalPointInRectangle(_uiElement.GetComponent<RectTransform>(), screenPosition, worldCamera, out uiPosition);

                 // ʹ��ת�����UI����������UIԪ�ص�λ��
                 _uiElement.anchoredPosition += uiPosition * sensitivity;*/
                // ʹ���ٶȸ���UIԪ�ص�λ��
                _uiElement.anchoredPosition += new Vector2(0, wristPosition.y) * sensitivity;
               
                // ����UIԪ�صķ��������Ҫ��
                //_uiElement.rotation = Quaternion.LookRotation(wristDirection.normalized, Vector3.up);
            }
        }
    }
}
