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

    // 内部变量
    private StreamWriter logFileWriter;
    private bool isRecording = false;
    private float recordingStartTime;

    void Start()
    {
        // 自动获取主菜单设置的用户名
        if (!string.IsNullOrEmpty(MainMenuManager.GlobalParticipantID))
            participantID = MainMenuManager.GlobalParticipantID;

        // 初始化状态
        if (startButton) startButton.SetActive(true);
        if (stopButton) stopButton.SetActive(false);
        if (countdownText) countdownText.text = "Ready"; // 初始显示 Ready
    }

    void Update()
    {
        if (isRecording)
        {
            float timeSinceStart = Time.time - recordingStartTime;
            float delta = Time.deltaTime;

            double[] predictionResult = nicerAPI.generatePrediction(
                handJoint, wristJoint, elbowJoint, shoulderJoint,
                gender, delta, timeSinceStart
            );

            if (logFileWriter != null)
            {
                // 写入文件
                string dataLine = $"{DateTime.Now:HH:mm:ss.fff},{timeSinceStart:F3},{predictionResult[1]:F4},{predictionResult[0]:F4}";
                logFileWriter.WriteLine(dataLine);
            }
        }
    }

    // =========================================================
    // 供 Meta Poke 按钮调用的公共函数
    // =========================================================

    // ★★★ 请把这个绑定到 Start Button 的 When Select / OnClick ★★★
    public void Click_StartTest()
    {
        // 1. 立即给视觉反馈！告诉用户“我收到了！”
        if (countdownText) countdownText.text = "Button Pressed!";

        // 2. 只有没在录制时才启动
        if (!isRecording)
        {
            StartCoroutine(StartFlowRoutine());
        }
    }

    // ★★★ 请把这个绑定到 Stop Button 的 When Select / OnClick ★★★
    public void Click_StopTest()
    {
        if (countdownText) countdownText.text = "Stopping...";

        if (isRecording)
        {
            StopAndSaveFlow();
        }
    }

    // =========================================================
    // 内部逻辑
    // =========================================================

    IEnumerator StartFlowRoutine()
    {
        // 稍微等一下让用户看清 "Button Pressed"
        yield return new WaitForSeconds(0.5f);

        // 隐藏开始按钮，避免误触
        if (startButton) startButton.SetActive(false);

        // 倒计时 3, 2, 1
        string[] counts = { "3", "2", "1" };
        foreach (var c in counts)
        {
            if (countdownText) countdownText.text = c;
            yield return new WaitForSeconds(1.0f);
        }

        // 显示 GO
        if (countdownText) countdownText.text = "GO";

        // 真正的逻辑开始：建文件、记时间
        InitializeLogFile();
        recordingStartTime = Time.time;
        isRecording = true;

        // 激活结束按钮
        if (stopButton) stopButton.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        if (countdownText) countdownText.text = ""; // 清空文字，不挡视线
    }

    private void StopAndSaveFlow()
    {
        isRecording = false;

        if (logFileWriter != null)
        {
            logFileWriter.Flush();
            logFileWriter.Close();
            logFileWriter = null;
        }

        // 跳转回主菜单
        SceneManager.LoadScene(mainMenuSceneName);
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
            if (countdownText) countdownText.text = "File Error!";
        }
    }

    void OnDestroy()
    {
        if (logFileWriter != null) logFileWriter.Close();
    }
}