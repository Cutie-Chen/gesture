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
    //public Transform handTransform; // �ֲ�λ��
    public Transform headTransform; // ͷ����ͨ��Ϊ�������
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandStart;
    //public EnhancedJointVelocityState velocityState;
    public JointDeltaProvider JointDelta;
    public float maxRayDistance = 20f; // ����������
    [SerializeField, Interface(typeof(IHand))]
    private UnityEngine.Object _hand;
    private IHand Hand;
    //public GameObject targetObject;
    
    // ���ߵĳ���
    public float rayLength = 20f;
    public TextMeshProUGUI t;
    // LineRenderer ���
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
        // ��ȡ�ֲ���ͷ����λ���뷽��

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

        /*// ����ͷ�����ֲ��ķ���
        Vector3 headToHandDirection = (handPosition - headPosition).normalized;

        // ���ͷ��ǰ������ֲ����򣬼����ۺ�Ͷ�䷽��
        float weightHead = 0.7f; // ͷ������Ȩ��
        float weightHand = 0.3f; // �ֲ�����Ȩ��
        Vector3 combinedDirection = (weightHead * headForward + weightHand * headToHandDirection).normalized;*/

        //Vector3 combinedDirection = fingerDirection;
        // ��������
        Ray ray = new Ray(handPosition, headForward);
        Vector3 rayEnd = handPosition + headForward * maxRayDistance;
        RaycastHit hit;
        lineRenderer.SetPosition(0, handPosition);
        lineRenderer.SetPosition(1, rayEnd);
        if (Physics.Raycast(ray, out hit, maxRayDistance))
        {
            // ��⵽����
            if (hit.collider.gameObject.CompareTag("taga"))
            {
                //Debug.Log("Selected Object: " + selectedObject.name);

                t.text = "ѡ����";
            }
        }
       
        else
            t.text = "δѡ��";

    }

   
}
