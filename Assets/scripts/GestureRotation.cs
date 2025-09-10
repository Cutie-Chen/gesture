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
    public HandJointId jointToTrack = HandJointId.HandStart; // 跟踪的关节，手腕
    public GameObject objectToRotate; // 需要旋转的物体
    [SerializeField]
    private RectTransform _uiElement; // UI的RectTransform组件
    private Vector3 lastPosition; // 上一帧的手部位置
    private float rotationAngle = 0f; // 当前物体的旋转角度
    private float rotationSpeed = 10f; // 旋转速度因子
    private float movementThreshold = 0.01f; // 最小移动阈值
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
            // 获取当前手部的位置
            if (hand.GetJointPose(jointToTrack, out Pose currentPose))
            {
                Vector3 currentPosition = currentPose.position;
                Vector3 wristPosition = velocityState.GetJointposition(jointToTrack);

                // 计算手部在上一帧和当前帧之间的位移
                Vector3 movementDelta = currentPosition - lastPosition;

                // 如果位移大于阈值，继续判断手势
                if (movementDelta.magnitude > movementThreshold)
                {
                    // 计算手部的旋转方向（XY平面上的旋转）
                    // 假设旋转发生在XY平面上，投影到XY平面
                    Vector3 lastPositionXY = new Vector3(lastPosition.x, lastPosition.y, 0);  // 将z轴设置为0
                    Vector3 currentPositionXY = new Vector3(currentPosition.x, currentPosition.y, 0);  // 将z轴设置为0

                    // 计算上一帧与当前帧在XY平面上的方向变化
                    Vector3 deltaXY = currentPositionXY - lastPositionXY;

                // 计算上一帧与当前帧之间的夹角
                //float angle = Vector3.SignedAngle(lastPositionXY, currentPositionXY, Vector3.forward);
                
                // 使用 Atan2 计算两点的极角
                float lastAngle = Mathf.Atan2(lastPositionXY.y, lastPositionXY.x) * Mathf.Rad2Deg; // 转为角度
                float currentAngle = Mathf.Atan2(currentPositionXY.y, currentPositionXY.x) * Mathf.Rad2Deg; // 转为角度

                // 计算角度差（考虑方向性）
                float angleDelta = Mathf.DeltaAngle(lastAngle, currentAngle); // 自动计算角度差（正负值）


                // 根据角度判断旋转方向，并累加旋转角度
                if (Mathf.Abs(angleDelta) > 0.5f) // 防止微小误差导致的旋转
                {
                    // 根据角度变化更新旋转角度
                    rotationAngle += angleDelta * rotationSpeed; // angleDelta 本身有方向性
                    objectToRotate.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);

                }

                /*if (Mathf.Abs(angle) > rotationThreshold) // 如果位移角度变化大，可能是画圈手势
                 {

                     rotationAngle += angle * rotationSpeed; // 使用原始角度而非绝对值
                 }
                 else
                 {

                    _uiElement.anchoredPosition += new Vector2(wristPosition.x, wristPosition.y) * sensitivity;
                }
*/
                // 更新物体的旋转
                objectToRotate.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                }

                // 更新上一帧的位置
                lastPosition = currentPosition;
            }
        /*}*/
    }
}
