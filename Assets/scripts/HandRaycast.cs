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
public class HandRaycast : MonoBehaviour
{
    //public Transform handTransform; // 手部位置
    public Transform headTransform; // 头部（通常为摄像机）
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandStart;
    //public EnhancedJointVelocityState velocityState;
    public JointDeltaProvider JointDelta;
    public float maxRayDistance = 20f; // 射线最大距离
    [SerializeField, Interface(typeof(IHand))]
    private UnityEngine.Object _hand;
    private IHand Hand;
    //public GameObject targetObject;
    
    // 射线的长度
    public float rayLength = 20f;
    public TextMeshProUGUI t;
    // LineRenderer 组件
    private LineRenderer lineRenderer;
    
    Vector3 handPosition;
    private void Awake()
    {
        Hand = _hand as IHand;
    }
    private void Start()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        //lineRenderer.material = new Material;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        //JointDelta = new JointDeltaProvider();
    }
    void Update()
    {
        // 获取手部和头部的位置与方向

        //jointDeltaProvider.GetPositionDelta(_jointToLog, out handPosition);
        //handPosition=velocityState.GetJointposition(_jointToLog);
        //JointDelta.GetPositionDelta(_jointToLog, out handPosition);
        if (Hand.GetJointPose(_jointToLog, out Pose jointPose) &&
            JointDelta.GetPositionDelta(_jointToLog, out Vector3 velocity))
        {
            handPosition= jointPose.position;
        }
        //handPosition = Vector3.zero;
        /*recognizer.GetFeatureVectorAndWristPos(
            TransformFeature.FingersUp,
            true,
            ref fingerDirection,
            ref wristPosition);*/
        Vector3 headForward = headTransform.forward;

        /*// 计算头部到手部的方向
        Vector3 headToHandDirection = (handPosition - headPosition).normalized;

        // 结合头部前方向和手部方向，计算综合投射方向
        float weightHead = 0.7f; // 头部方向权重
        float weightHand = 0.3f; // 手部方向权重
        Vector3 combinedDirection = (weightHead * headForward + weightHand * headToHandDirection).normalized;*/

        //Vector3 combinedDirection = fingerDirection;
        // 发射射线
        Ray ray = new Ray(handPosition, headForward);
        Vector3 rayEnd = handPosition + headForward * maxRayDistance;
        RaycastHit hit;
        lineRenderer.SetPosition(0, handPosition);
        lineRenderer.SetPosition(1, rayEnd);
        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            // 检测到物体
            if (hit.collider.gameObject.CompareTag("taga"))
            {
                //Debug.Log("Selected Object: " + selectedObject.name);

                t.text = "选中了";
            }
        }
       
        else
            t.text = "未选中";

    }

   
}
