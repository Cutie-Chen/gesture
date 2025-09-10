using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
using Oculus.Interaction;

public class GestureRotation : MonoBehaviour
{
    [SerializeField]
    private Hand hand;
    private Pose currentPose;
    public HandJointId jointToTrack = HandJointId.HandStart; // ���ٵĹؽڣ�����
    public GameObject objectToRotate; // ��Ҫ��ת������
    [SerializeField]
    private RectTransform _uiElement; // UI��RectTransform���
    private Vector3 lastPosition; // ��һ֡���ֲ�λ��
    private float rotationAngle = 0f; // ��ǰ�������ת�Ƕ�
    private float rotationSpeed = 10f; // ��ת�ٶ�����
    private float movementThreshold = 0.01f; // ��С�ƶ���ֵ
    private float rotationThreshold=5;
    public ActiveStateGroup activestategroup;
    public EnhancedJointVelocityState velocityState;
    public float sensitivity = 10f;
    void Start()
    {
        lastPosition = Vector3.zero;
    }

    void Update()
    {
        /*if (activestategroup.Active)
        {*/
            // ��ȡ��ǰ�ֲ���λ��
            if (hand.GetJointPose(jointToTrack, out Pose currentPose))
            {
                Vector3 currentPosition = currentPose.position;
                Vector3 wristPosition = velocityState.GetJointposition(jointToTrack);

                // �����ֲ�����һ֡�͵�ǰ֮֡���λ��
                Vector3 movementDelta = currentPosition - lastPosition;

                // ���λ�ƴ�����ֵ�������ж�����
                if (movementDelta.magnitude > movementThreshold)
                {
                    // �����ֲ�����ת����XYƽ���ϵ���ת��
                    // ������ת������XYƽ���ϣ�ͶӰ��XYƽ��
                    Vector3 lastPositionXY = new Vector3(lastPosition.x, lastPosition.y, 0);  // ��z������Ϊ0
                    Vector3 currentPositionXY = new Vector3(currentPosition.x, currentPosition.y, 0);  // ��z������Ϊ0

                    // ������һ֡�뵱ǰ֡��XYƽ���ϵķ���仯
                    Vector3 deltaXY = currentPositionXY - lastPositionXY;

                // ������һ֡�뵱ǰ֮֡��ļн�
                //float angle = Vector3.SignedAngle(lastPositionXY, currentPositionXY, Vector3.forward);
                
                // ʹ�� Atan2 ��������ļ���
                float lastAngle = Mathf.Atan2(lastPositionXY.y, lastPositionXY.x) * Mathf.Rad2Deg; // תΪ�Ƕ�
                float currentAngle = Mathf.Atan2(currentPositionXY.y, currentPositionXY.x) * Mathf.Rad2Deg; // תΪ�Ƕ�

                // ����ǶȲ���Ƿ����ԣ�
                float angleDelta = Mathf.DeltaAngle(lastAngle, currentAngle); // �Զ�����ǶȲ����ֵ��


                // ���ݽǶ��ж���ת���򣬲��ۼ���ת�Ƕ�
                if (Mathf.Abs(angleDelta) > 0.5f) // ��ֹ΢С���µ���ת
                {
                    // ���ݽǶȱ仯������ת�Ƕ�
                    rotationAngle += angleDelta * rotationSpeed; // angleDelta �����з�����
                    objectToRotate.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

                }

                /*if (Mathf.Abs(angle) > rotationThreshold) // ���λ�ƽǶȱ仯�󣬿����ǻ�Ȧ����
                 {

                     rotationAngle += angle * rotationSpeed; // ʹ��ԭʼ�Ƕȶ��Ǿ���ֵ
                 }
                 else
                 {

                    _uiElement.anchoredPosition += new Vector2(wristPosition.x, wristPosition.y) * sensitivity;
                }
*/
                // �����������ת
                objectToRotate.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                }

                // ������һ֡��λ��
                lastPosition = currentPosition;
            }
        /*}*/
    }
}
