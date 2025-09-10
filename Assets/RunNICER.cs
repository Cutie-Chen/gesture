using UnityEngine;
using System.IO; // 1. 引入 IO 命名空间用于文件操作
using NICER_Unity_API;

public class RunNICER : MonoBehaviour
{
    [Header("API 组件链接")]
    [Tooltip("请将场景中挂载了 NICER_API 组件的游戏对象拖到这里")]
    public NICER_API nicerAPI;

    [Header("实时关节数据 (从您的角色模型上拖拽)")]
    public Transform shoulderJoint;
    public Transform elbowJoint;
    public Transform wristJoint;
    public Transform handJoint;

    [Header("用户参数")]
    [Tooltip("用户的生理性别，用于计算模型参数")]
    public string gender = "Male";

    [Header("输出结果")]
    [Tooltip("当前计算出的疲劳度 (0 到 100)")]
    public double fatigueLevel;
    [Tooltip("预计的最大续航时间 (秒)")]
    public double enduranceTime;

    [Header("文件日志设置")]
    [Tooltip("日志文件的名称")]
    public string logFileName = "nicer_log.csv";

    private float totalTime = 0f;
    private StreamWriter logFileWriter; // 2. 创建一个 StreamWriter 变量用于写入文件

    void Start()
    {
        // 检查必要的组件和关节是否已经链接
        if (nicerAPI == null || shoulderJoint == null || elbowJoint == null || wristJoint == null || handJoint == null)
        {
            Debug.LogError("错误：请在 Inspector 窗口中链接所有的 API 组件和关节 Transform！");
            this.enabled = false;
            return;
        }

        // --- 文件初始化操作 ---
        InitializeLogFile();

        Debug.Log("NICER 实时追踪已准备就绪，日志记录已启动。");
    }

    void Update()
    {
        totalTime += Time.deltaTime;

        double[] predictionResult = nicerAPI.generatePrediction(
            handJoint,
            wristJoint,
            elbowJoint,
            shoulderJoint,
            gender,
            Time.deltaTime,
            totalTime
        );

        enduranceTime = predictionResult[0];
        fatigueLevel = predictionResult[1];

        // 4. 将当前帧的数据写入文件
        if (logFileWriter != null)
        {
            string dataLine = $"{totalTime:F3},{fatigueLevel:F4},{enduranceTime:F4}";
            logFileWriter.WriteLine(dataLine);
        }
    }

    // 5. 当脚本销毁或应用退出时，确保文件被正确关闭
    void OnDestroy()
    {
        if (logFileWriter != null)
        {
            Debug.Log("正在关闭日志文件...");
            logFileWriter.Close();
            logFileWriter = null;
        }
    }

    // 3. 初始化文件和写入器的方法
    private void InitializeLogFile()
    {
        // Application.persistentDataPath 是 Unity 保证在所有平台上都可写的安全路径
        string filePath = Path.Combine(Application.persistentDataPath, logFileName);

        try
        {
            // 创建或覆盖文件，并准备写入。设置为 false 表示不追加，每次运行都创建新文件。
            logFileWriter = new StreamWriter(filePath, false);

            // 写入 CSV 文件的表头
            logFileWriter.WriteLine("TotalTime,FatigueLevel,EnduranceTime");

            Debug.Log($"日志文件已在以下位置创建: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"无法创建日志文件: {e.Message}");
            logFileWriter = null; // 如果失败，确保写入器为空
        }
    }
}