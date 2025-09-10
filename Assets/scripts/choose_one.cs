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
    public HandJointId jointToTrack = HandJointId.HandStart; // 跟踪的关节，手腕
    [SerializeField]
    public List<GameObject> objectsToSelect = new List<GameObject>(); // 需要选择的多个UI元素（UI 物体）
    private List<RectTransform> objectRectTransforms = new List<RectTransform>(); // 存储所有UI元素的RectTransform
    public ActiveStateGroup activestategroup;
    //public bool isselect;
    public TextMeshProUGUI t;
    private GameObject selectedObject = null; // 当前被选中的UI元素
    private GameObject lastselect = null;
    void Start()
    {
        // 获取所有UI元素的RectTransform组件
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
    // 判断手部关节是否在UI元素范围内
    public bool IsJointInsideObject(Vector3 handPosition, RectTransform rectTransform)
    {
        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners); // 获取UI元素的四个角的世界坐标

        // 获取UI元素的2D边界框
        Vector3 topLeft = worldCorners[1];  // 左上角
        Vector3 bottomRight = worldCorners[3];  // 右下角

        // 获取手部关节的位置，并投影到屏幕空间
        Vector3 jointPosition = new Vector3(handPosition.x, handPosition.y, 0);  // 忽略 Z 轴

        // 判断关节是否在UI元素的2D边界框内
        bool isInside = jointPosition.x >= topLeft.x && jointPosition.x <= bottomRight.x &&
                        jointPosition.y >= bottomRight.y && jointPosition.y <= topLeft.y;

        return isInside;
    }

    // 更新每一帧并检查手势关节是否在物体内
    void Update()
    {
        Pose currentPose;
        if (activestategroup.Active)
        {
            // 获取当前手部关节的位置
            if (hand.GetJointPose(jointToTrack, out currentPose))
            {
                Vector3 handPosition = currentPose.position;  // 获取关节位置

                selectedObject = null;  // 默认没有选中任何对象

                // 遍历所有UI元素，检查手指是否在某个UI元素的区域内
                foreach (var rectTransform in objectRectTransforms)
                {
                    if (IsJointInsideObject(handPosition, rectTransform))
                    {
                        selectedObject = rectTransform.gameObject; // 设置为当前选中的UI元素
                        break; // 一旦选中一个元素，跳出循环
                    }
                }
            }

            // 根据选中的UI元素显示文本
            if (selectedObject != null)
            {
                selectedObject.tag = "select";
                t.text = $"选中了: {selectedObject.name}+{selectedObject.tag}";
                selectedObject.tag = "select";

                lastselect.tag = "unselect";
            }
            else
            {
                t.text = "未选中";

            }
            lastselect = selectedObject;
        }
    }

}
