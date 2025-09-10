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
    public RectTransform image; // ��Ҫ��ת��ͼƬ����
    private Vector2 initialFingerPos; // ��ָ�ĳ�ʼλ��
    private Quaternion initialImageRotation; // ��ʼ��ͼƬ��ת
    Vector3 fingerPos;
    private bool isRotating = false; // �ж��Ƿ�����ת״̬
    [SerializeField]
    private HandJointId _jointToLog = HandJointId.HandStart;
    [SerializeField, Interface(typeof(IHand))]
    private UnityEngine.Object _hand;
    private IHand Hand;
    public JointDeltaProvider JointDelta;
    // ͼƬ������Բ�뾶�������������Σ��ҳߴ���֪��
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
        
        // ����ͼƬ�Ĵ�С��һ�������Σ���ȡ���İ뾶
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
            // ����ָ��λ��ӳ�䵽����Բ��
            Vector2 mappedFingerPos = MapToCircumference(fingerPos);
            //text.text = $"{ mappedFingerPos}";
            // ������ת�Ƕ�
            float angle = CalculateRotationAngle(initialFingerPos, mappedFingerPos);

            // ���ݽǶ���תͼƬ
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
    

    // ����ָλ��ӳ�䵽ͼƬ����Բ��
    Vector2 MapToCircumference(Vector3 fingerPos)
    {
        // ������ָλ�õĵ�λ����

        direction.x =fingerPos.x - image.position.x;
        direction.y=fingerPos.y -image.position.y;
        direction.Normalize();
        point.x=image.position.x + direction.x * radius;
        point.y = image.position.y + direction.y * radius;
        // ��������Բ�ϵĽ���
        return point;
    }

    // ������������֮��ĽǶ�
    float CalculateRotationAngle(Vector2 startPos, Vector2 endPos)
    {
        // ������ʼ��ͽ����������
        Vector2 startDir;
        Vector2 endDir;
        startDir.x= startPos.x - image.position.x;
        startDir.y = startPos.y - image.position.y;
        endDir.x = endPos.x - image.position.x;
        endDir.y = endPos.y - image.position.y;

        // �������ǵļн�
        float angle = Vector2.SignedAngle(startDir, endDir);

        return angle;
    }

 
    public bool IsJointInsideObject(Vector3 handPosition,float threshold)
    {
        // ��ȡ UI Ԫ�ص��������귶Χ
        //EventSystem system = new EventSystem();
        //system.RaycastAll()
        Vector3[] worldCorners = new Vector3[4];
        image.GetWorldCorners(worldCorners);

        // ��ȡ UI Ԫ�ص� 2D �߽��
        
        Vector3 topLeft = worldCorners[1];  // ���Ͻ�
        Vector3 topRight = worldCorners[2]; // ���Ͻ�
        Vector3 bottomLeft = worldCorners[0];  // ���½�
        Vector3 bottomRight = worldCorners[3];
        // ��ȡ�ֲ��ؽڵ�λ�ã���ͶӰ����Ļ�ռ�
        Vector3 jointPosition = new Vector3(handPosition.x, handPosition.y, 0);  // ���� Z ��

        
        // ������ָ��ÿ���ǵľ��룬���ж��Ƿ�С����ֵ
        if (Vector3.Distance(jointPosition, topLeft) < threshold ||
            Vector3.Distance(jointPosition, topRight) < threshold ||
            Vector3.Distance(jointPosition, bottomLeft) < threshold ||
            Vector3.Distance(jointPosition, bottomRight) < threshold)
        {
            initialFingerPos = getposition();
            initialImageRotation = image.rotation;
            return true;  // ��ָ�ӽ��ĸ����е�����һ��
        }
        else
            return false;

        
    }
    
}
