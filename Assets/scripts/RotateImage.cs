using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
using Oculus.Interaction;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class RotateImage : MonoBehaviour
{
    public TextMeshProUGUI text;
    public RectTransform image; // 需要旋转的图片对象
    private Vector2 initialFingerPos; // 手指的初始位置
    private Quaternion initialImageRotation; // 初始的图片旋转
    Vector3 fingerPos;
    private bool isRotating = false; // 判断是否处于旋转状态
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandStart;
    [SerializeField, Interface(typeof(IHand))]
    private UnityEngine.Object _hand;
    private IHand Hand;
    public JointDeltaProvider JointDelta;
    // 图片的外切圆半径（假设是正方形，且尺寸已知）
    private float radius;
    float threshold;
    Vector2 direction;
    Vector2 point;
    private void Awake()
    {
        Hand = _hand as IHand;
    }
    void Start()
    {
        
        // 假设图片的大小是一个正方形，获取它的半径
        radius = Mathf.Sqrt(Mathf.Pow(image.rect.width*image.localScale.x / 2, 2) + Mathf.Pow(image.rect.height*image.localScale.y / 2, 2));
        threshold = (image.rect.width * image.localScale.x) / 10;
    }

    void Update()
    {
        fingerPos = getposition();
        isRotating=IsJointInsideObject(fingerPos, threshold);
        initialFingerPos.x = getposition().x;
        initialFingerPos.y = getposition().y;
        initialImageRotation = image.rotation;
        isRotating = true;
        if (isRotating)
        {
            // 将手指的位置映射到外切圆上
            Vector2 mappedFingerPos = MapToCircumference(fingerPos);
            //text.text = $"{ mappedFingerPos}";
            // 计算旋转角度
            float angle = CalculateRotationAngle(initialFingerPos, mappedFingerPos);

            // 根据角度旋转图片
            image.rotation = initialImageRotation * Quaternion.Euler(0, 0, angle);
        }
    }
    Vector3 getposition()
    {
        if (Hand.GetJointPose(_jointToLog, out Pose jointPose) &&
          JointDelta.GetPositionDelta(_jointToLog, out Vector3 velocity))
        {
            fingerPos = jointPose.position;
        }
        return fingerPos;
    }
    

    // 将手指位置映射到图片外切圆上
    Vector2 MapToCircumference(Vector3 fingerPos)
    {
        // 计算手指位置的单位向量

        direction.x =fingerPos.x - image.position.x;
        direction.y=fingerPos.y -image.position.y;
        direction.Normalize();
        point.x=image.position.x + direction.x * radius;
        point.y = image.position.y + direction.y * radius;
        // 返回外切圆上的交点
        return point;
    }

    // 计算两个向量之间的角度
    float CalculateRotationAngle(Vector2 startPos, Vector2 endPos)
    {
        // 计算起始点和结束点的向量
        Vector2 startDir;
        Vector2 endDir;
        startDir.x= startPos.x - image.position.x;
        startDir.y = startPos.y - image.position.y;
        endDir.x = endPos.x - image.position.x;
        endDir.y = endPos.y - image.position.y;

        // 计算它们的夹角
        float angle = Vector2.SignedAngle(startDir, endDir);

        return angle;
    }

 
    public bool IsJointInsideObject(Vector3 handPosition,float threshold)
    {
        // 获取 UI 元素的世界坐标范围
        //EventSystem system = new EventSystem();
        //system.RaycastAll()
        Vector3[] worldCorners = new Vector3[4];
        image.GetWorldCorners(worldCorners);

        // 获取 UI 元素的 2D 边界框
        
        Vector3 topLeft = worldCorners[1];  // 左上角
        Vector3 topRight = worldCorners[2]; // 右上角
        Vector3 bottomLeft = worldCorners[0];  // 左下角
        Vector3 bottomRight = worldCorners[3];
        // 获取手部关节的位置，并投影到屏幕空间
        Vector3 jointPosition = new Vector3(handPosition.x, handPosition.y, 0);  // 忽略 Z 轴

        
        // 计算手指与每个角的距离，并判断是否小于阈值
        if (Vector3.Distance(jointPosition, topLeft) < threshold ||
            Vector3.Distance(jointPosition, topRight) < threshold ||
            Vector3.Distance(jointPosition, bottomLeft) < threshold ||
            Vector3.Distance(jointPosition, bottomRight) < threshold)
        {
            initialFingerPos = getposition();
            initialImageRotation = image.rotation;
            return true;  // 手指接近四个角中的任意一个
        }
        else
            return false;

        
    }
    
}
