using System;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Oculus.Interaction.PoseDetection;
using TMPro;
using Oculus.Interaction;
using UnityEngine.EventSystems;

public class choose_one : MonoBehaviour
{
    [SerializeField]
    private Hand hand;
    public HandJointId jointToTrack = HandJointId.HandStart; // ���ٵĹؽڣ�����
    [SerializeField]
    public List<GameObject> objectsToSelect = new List<GameObject>(); // ��Ҫѡ��Ķ��UIԪ�أ�UI ���壩
    private List<RectTransform> objectRectTransforms = new List<RectTransform>(); // �洢����UIԪ�ص�RectTransform
    public ActiveStateGroup activestategroup;
    //public bool isselect;
    public TextMeshProUGUI t;
    private GameObject selectedObject = null; // ��ǰ��ѡ�е�UIԪ��
    private GameObject lastselect = null;
    void Start()
    {
        // ��ȡ����UIԪ�ص�RectTransform���
        foreach (var obj in objectsToSelect)
        {
            var rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                objectRectTransforms.Add(rectTransform);
            }
            else
            {
                Debug.LogError($"Object {obj.name} does not have a RectTransform component.");
            }
        }
    }
    // Start is called before the first frame update
    // �ж��ֲ��ؽ��Ƿ���UIԪ�ط�Χ��
    public bool IsJointInsideObject(Vector3 handPosition, RectTransform rectTransform)
    {
        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners); // ��ȡUIԪ�ص��ĸ��ǵ���������

        // ��ȡUIԪ�ص�2D�߽��
        Vector3 topLeft = worldCorners[1];  // ���Ͻ�
        Vector3 bottomRight = worldCorners[3];  // ���½�

        // ��ȡ�ֲ��ؽڵ�λ�ã���ͶӰ����Ļ�ռ�
        Vector3 jointPosition = new Vector3(handPosition.x, handPosition.y, 0);  // ���� Z ��

        // �жϹؽ��Ƿ���UIԪ�ص�2D�߽����
        bool isInside = jointPosition.x >= topLeft.x && jointPosition.x <= bottomRight.x &&
                        jointPosition.y >= bottomRight.y && jointPosition.y <= topLeft.y;

        return isInside;
    }

    // ����ÿһ֡��������ƹؽ��Ƿ���������
    void Update()
    {
        Pose currentPose;
        if (activestategroup.Active)
        {
            // ��ȡ��ǰ�ֲ��ؽڵ�λ��
            if (hand.GetJointPose(jointToTrack, out currentPose))
            {
                Vector3 handPosition = currentPose.position;  // ��ȡ�ؽ�λ��

                selectedObject = null;  // Ĭ��û��ѡ���κζ���

                // ��������UIԪ�أ������ָ�Ƿ���ĳ��UIԪ�ص�������
                foreach (var rectTransform in objectRectTransforms)
                {
                    if (IsJointInsideObject(handPosition, rectTransform))
                    {
                        selectedObject = rectTransform.gameObject; // ����Ϊ��ǰѡ�е�UIԪ��
                        break; // һ��ѡ��һ��Ԫ�أ�����ѭ��
                    }
                }
            }

            // ����ѡ�е�UIԪ����ʾ�ı�
            if (selectedObject != null)
            {
                selectedObject.tag = "select";
                t.text = $"ѡ����: {selectedObject.name}+{selectedObject.tag}";
                selectedObject.tag = "select";

                lastselect.tag = "unselect";
            }
            else
            {
                t.text = "δѡ��";

            }
            lastselect = selectedObject;
        }
    }

}
