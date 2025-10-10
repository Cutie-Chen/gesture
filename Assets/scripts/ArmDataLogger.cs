// 引用必要的命名空间
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ArmDataLogger : MonoBehaviour
{
    /*[Header("追踪组件引用")]
    [Tooltip("将配置为Body类型的那个OVRSkeleton组件（来自BodySkeletonReader对象）拖到这里")]
    public OVRSkeleton BodySkeleton; // 用于获取肩部、肘部等

    [Tooltip("将代表右手追踪的OVRSkeleton组件拖到这里")]
    public OVRSkeleton RightHandSkeleton; // 用于获取手腕、手部

    [Header("记录设置")]
    [Tooltip("勾选此项以开始或停止记录")]
    public bool IsRecording = false;

    [Tooltip("数据文件的保存路径和文件名")]
    public string FilePath = "Assets/arm_tracking_data.csv";

    // --- 私有变量 ---
    private List<string> _dataLines;
    private StringBuilder _stringBuilder;
    private bool _hasInitialized = false;

    // 我们需要追踪的关键关节的ID
    // 注意：当OVRSkeleton的Type为Body时，这里的ID也是OVRSkeleton.BoneId
    private readonly OVRSkeleton.BoneId[] _bodyJointsToTrack = new OVRSkeleton.BoneId[]
    {
        OVRSkeleton.BoneId.Body_Shoulder_R,
        OVRSkeleton.BoneId.Body_Elbow_R,
        OVRSkeleton.BoneId.Body_Wrist_R // 从身体骨骼里也能获取手腕
    };

    // 手部特有的关节
    private readonly OVRSkeleton.BoneId[] _handJointsToTrack = new OVRSkeleton.BoneId[]
    {
        OVRSkeleton.BoneId.Hand_IndexTip,
        OVRSkeleton.BoneId.Hand_ThumbTip
    };

    void Update()
    {
        if (!IsRecording) return;
        if (!_hasInitialized) InitializeRecording();

        float timestamp = Time.time;

        // --- 1. 记录身体关节 (从配置好的BodySkeleton读取) ---
        // 现在我们可以像处理手部一样，使用 .IsDataValid 属性了！
        if (BodySkeleton != null && BodySkeleton.IsDataValid)
        {
            foreach (var boneId in _bodyJointsToTrack)
            {
                foreach (var bone in BodySkeleton.Bones)
                {
                    if (bone.Id == boneId)
                    {
                        FormatAndAddLine(timestamp, bone.Id.ToString(), bone.Transform.position, bone.Transform.rotation);
                        break;
                    }
                }
            }
        }

        // --- 2. 记录手部特有关节 ---
        if (RightHandSkeleton != null && RightHandSkeleton.IsDataValid)
        {
            foreach (var boneId in _handJointsToTrack)
            {
                foreach (var bone in RightHandSkeleton.Bones)
                {
                    if (bone.Id == boneId)
                    {
                        FormatAndAddLine(timestamp, bone.Id.ToString(), bone.Transform.position, bone.Transform*/

}