using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using NICER_Unity_API;
using TMPro;

public class RunNICER : MonoBehaviour
{
    [Header("=== 核心组件 ===")]
    public NICER_API nicerAPI;
    public Transform shoulderJoint;
    public Transform elbowJoint;
    public Transform wristJoint;
    public Transform handJoint;

    [Header("=== UI 反馈 (必须拖) ===")]
    public TextMeshProUGUI countdownText;      // 倒计时文字框 (Feedback Text)
    public GameObject startButton;  // 开始按钮物体
    public GameObject stopButton;   // 结束按钮物体

    [Header("=== 实验设置 ===")]
    public string gestureID = "T1";
    public string mainMenuSceneName = "MAIN";
    public string gender = "Male";
    private string participantID = "P01";

    // --- 内部变量 ---
    private StreamWriter logFileWriter;
    private bool isRecording = false;
    private float recordingStartTime;

    // ★★★ 新增：防止误触的安全锁 ★★★
    private bool isSceneReady = false;

    void Start()
    {

        // 获取用户名
        if (!string.IsNullOrEmpty(MainMenuManager.GlobalParticipantID))
            participantID = MainMenuManager.GlobalParticipantID;
        if (OVRManager.instance != null)
            OVRManager.instance.isInsightPassthroughEnabled = true;

        // 初始化 UI
        if (startButton) startButton.SetActive(true);
        if (stopButton) stopButton.SetActive(false);

        // 初始提示
        if (countdownText) countdownText.text = "Loading...";

        // ★★★ 启动安全倒计时：1秒后才允许点击 ★★★
        StartCoroutine(EnableInputRoutine());
    }
    // ★★★ 这里就是 FixPassthroughRoutine 的定义 ★★★
  

    // ★★★ 安全锁解锁协程 ★★★
    IEnumerator EnableInputRoutine()
    {
        // 强制等待 1 秒，让你的手从上一个场景的按钮位置移开
        yield return new WaitForSeconds(1.0f);

        isSceneReady = true; // 解锁！现在可以点击了
        if (countdownText) countdownText.text = "Ready";
        Debug.Log("【系统】场景加载完毕，输入已激活");
    }

    void Update()
    {
        if (isRecording)
        {
            float timeSinceStart = Time.time - recordingStartTime;
            float delta = Time.deltaTime;

            // NICER 计算
            double[] predictionResult = nicerAPI.generatePrediction(
                handJoint, wristJoint, elbowJoint, shoulderJoint,
                gender, delta, timeSinceStart
            );

            // 写入文件
            if (logFileWriter != null)
            {
                string dataLine = $"{DateTime.Now:HH:mm:ss.fff},{timeSinceStart:F3},{predictionResult[1]:F4},{predictionResult[0]:F4}";
                logFileWriter.WriteLine(dataLine);
            }
        }
    }

    // =========================================================
    // 按钮点击事件 (已添加防误触)
    // =========================================================

    public void Click_StartTest()
    {
        // ★★★ 如果场景还没准备好，直接忽略点击 ★★★
        if (!isSceneReady) return;

        Debug.LogError("【调试】检测到点击：开始按钮！");

        // 视觉反馈
        if (countdownText) countdownText.text = "Button Pressed!";

        if (!isRecording)
        {
            StartCoroutine(StartFlowRoutine());
        }
    }

    public void Click_StopTest()
    {
        // ★★★ 同样防止误触结束按钮 ★★★
        if (!isSceneReady) return;

        Debug.LogError("【调试】检测到点击：结束按钮！");

        if (countdownText) countdownText.text = "Stopping..."; // 反馈

        // 保存并跳转
        StopAndSaveFlow();
    }

    // =========================================================
    // 内部逻辑流程
    // =========================================================

    IEnumerator StartFlowRoutine()
    {
        // 稍微等待一下让用户看清 "Button Pressed"
        yield return new WaitForSeconds(0.5f);

        // 1. 隐藏开始按钮
        if (startButton) startButton.SetActive(false);

        // 2. 倒计时
        string[] counts = { "3", "2", "1" };
        foreach (var c in counts)
        {
            if (countdownText) countdownText.text = c;
            Debug.Log($"倒计时: {c}");
            yield return new WaitForSeconds(1.0f);
        }

        // 3. 正式开始
        if (countdownText) countdownText.text = "GO";

        InitializeLogFile(); // 创建文件
        recordingStartTime = Time.time;
        isRecording = true;

        // 4. 显示结束按钮
        if (stopButton) stopButton.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        if (countdownText) countdownText.text = "";
    }

    private void StopAndSaveFlow()
    {
        // 停止录制
        isRecording = false;

        // 保存文件
        if (logFileWriter != null)
        {
            logFileWriter.Flush();
            logFileWriter.Close();
            logFileWriter = null;
            Debug.Log("文件已保存关闭");
        }

        // 跳转回主菜单 (带防报错检查)
        if (Application.CanStreamedLevelBeLoaded(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError($"无法跳转，请检查 Build Settings 里是否有场景: {mainMenuSceneName}");
            if (countdownText) countdownText.text = "Scene Error!";
        }
    }

    private void InitializeLogFile()
    {
        string fileName = $"{participantID}_{gestureID}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        try
        {
            logFileWriter = new StreamWriter(filePath, false);
            logFileWriter.WriteLine("SystemTime,TimeSinceStart,FatigueLevel,EnduranceTime");
        }
        catch (Exception e)
        {
            Debug.LogError("文件创建失败: " + e.Message);
            if (countdownText) countdownText.text = "File Error!";
        }
    }

    void OnDestroy()
    {
        if (logFileWriter != null) logFileWriter.Close();
    }
}