using UnityEngine;
using TMPro;
using System.IO; // 用于文件读写
using System;   // 用于 DateTime 和 Guid

using Oculus.Interaction;
public class GestureFunctionDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    // --- 日志记录相关的静态成员 ---
    // 静态成员确保即使有多个此脚本的实例，也只操作同一个日志文件和会话
    private static string currentLogFilePath;
    private static string currentSessionID;
    private static bool isLoggerInitialized = false; //确保初始化只执行一次
    private static readonly object fileWriteLock = new object(); // 用于同步文件写入操作，防止冲突

    private const string LOG_FILE_NAME = "MyGestureActionsLog.csv"; // 您可以自定义日志文件名
    private const string CSV_FILE_HEADER = "Timestamp,SessionID,FrameCount,GestureName,Status";

    [SerializeField]
    public ActiveStateGroup T5;
    public ActiveStateGroup T9;
    void Awake()
    {
        // 调用一次性的共享日志初始化函数
        InitializeSharedFileLogger();
    }

    // 静态的初始化方法，确保只执行一次
    private static void InitializeSharedFileLogger()
    {
        lock (fileWriteLock) // 使用锁确保线程安全（尽管Unity主线程操作，但这是好习惯）
        {
            if (isLoggerInitialized)
            {
                return; // 如果已经初始化过了，就直接返回
            }

            currentSessionID = Guid.NewGuid().ToString();
            // Application.persistentDataPath 是在Quest等设备上安全可写的标准路径
            currentLogFilePath = Path.Combine(Application.persistentDataPath, LOG_FILE_NAME);

            try
            {
                if (!File.Exists(currentLogFilePath))
                {
                    // 文件不存在，创建并写入表头
                    File.WriteAllText(currentLogFilePath, CSV_FILE_HEADER + Environment.NewLine);
                    Debug.Log($"手势日志文件已创建: {currentLogFilePath}");
                }
                else
                {
                    // 文件已存在，可以追加一个新会话开始的标记（可选）
                    File.AppendAllText(currentLogFilePath, $"# New Session Started: {currentSessionID}{Environment.NewLine}");
                    Debug.Log($"手势日志将追加到现有文件: {currentLogFilePath}");
                }
                isLoggerInitialized = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"初始化手势日志文件失败: {e.Message}");
                // 初始化失败，isLoggerInitialized 保持 false，后续日志记录会受影响
            }
        }
    }

    // 内部方法，用于将条目写入日志文件
    private void WriteToLog(string gestureNameForLog)
    {
        if (!isLoggerInitialized)
        {
            Debug.LogWarning("日志记录器未初始化，无法记录。");
            // 可以尝试再次初始化，或者提示用户检查设置
            // InitializeSharedFileLogger(); // 再次尝试初始化
            // if (!isLoggerInitialized) return; // 如果还是失败，则放弃
            return;
        }

        if (string.IsNullOrEmpty(gestureNameForLog))
        {
            Debug.LogWarning("手势日志名称为空，不进行记录。");
            return;
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        int frameCount = Time.frameCount;
        // 对于您的需求，状态总是“Triggered”或“Executed”
        string status = "Triggered";
        string logLine = $"{timestamp},{currentSessionID},{frameCount},{EscapeCSVField(gestureNameForLog)},{status}";

        lock (fileWriteLock) // 写入文件时加锁
        {
            try
            {
                File.AppendAllText(currentLogFilePath, logLine + Environment.NewLine);
            }
            catch (Exception e)
            {
                Debug.LogError($"写入日志条目失败: {e.Message} | 条目: {logLine}");
            }
        }
    }

    // CSV转义辅助函数
    private string EscapeCSVField(string field)
    {
        if (string.IsNullOrEmpty(field)) return "";
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }

    // --- 您原来的方法，现在集成了日志记录 ---
    public void Zoom()
    {
        displayText.text = "T1 缩放-放大";
        WriteToLog("T1_Zoom_Enlarge");
    }
    public void Scale()
    {
        displayText.text = "T1 缩放-缩小";
        WriteToLog("T1_Zoom_Shrink");
    }
    public void SwitchSlice_up()
    {
        displayText.text = "T2 切换断层-上";
        WriteToLog("T2_SwitchSlice_Up");
    }
    public void SwitchSlice_down()
    {
        displayText.text = "T2 切换断层-下";
        WriteToLog("T2_SwitchSlice_Down");
    }
    public void Pan()
    {
        if (!T5 && !T9) {
            displayText.text = "T3 平移（上下左右）";
            WriteToLog("T3_Pan");
        }
    }

    public void Rotate()
    {
        displayText.text = "T4 旋转（顺、逆时针）";
        WriteToLog("T4_Rotate");
    }

    public void LayoutPanels1()
    {
        displayText.text = "T5 布局-1";
        WriteToLog("T5_Layout_1");
    }
    public void LayoutPanels2()
    {
        displayText.text = "T5 布局-2";
        WriteToLog("T5_Layout_2");
    }
    public void LayoutPanels3()
    {
        displayText.text = "T5 布局-3";
        WriteToLog("T5_Layout_3");
    }
    public void LayoutPanels4()
    {
        displayText.text = "T5 布局-4";
        WriteToLog("T5_Layout_4");
    }

    public void OpenRuler()
    {
        displayText.text = "T6 打开标尺";
        WriteToLog("T6_Ruler_Open");
    }

    public void CloseRuler()
    {
        displayText.text = "T6 关闭标尺";
        WriteToLog("T6_Ruler_Close");
    }

    public void AdjustWindowLevel_up()
    {
        displayText.text = "T7 调节窗位-增高";
        WriteToLog("T7_WindowLevel_Up");
    }

    public void AdjustWindowLevel_down()
    {
        displayText.text = "T7 调节窗位-降低";
        WriteToLog("T7_WindowLevel_Down");
    }
    public void AdjustWindowWidth_up()
    {
        displayText.text = "T8 调节窗宽-增高";
        WriteToLog("T8_WindowWidth_Up");
    }

    public void AdjustWindowWidth_down()
    {
        displayText.text = "T8 调节窗宽-降低";
        WriteToLog("T8_WindowWidth_Down");
    }
    public void PresetWindow1()
    {
        displayText.text = "T9 预设窗位窗宽1";
        WriteToLog("T9_PresetWindow_1");
    }
    public void PresetWindow2()
    {
        displayText.text = "T9 预设窗位窗宽2";
        WriteToLog("T9_PresetWindow_2");
    }
    public void ActivateSequence()
    {
        displayText.text = "T10 激活病人片子序列";
        WriteToLog("T10_PatientSequence_Activate");
    }

    public void DeactivateSequence()
    {
        displayText.text = "T10 关闭病人片子序列";
        WriteToLog("T10_PatientSequence_Deactivate");
    }

    public void SwitchPanel()
    {
        displayText.text = "T11 切换面板（上下左右）";
        WriteToLog("T11_SwitchPanel");
    }

    public void OpenInfoWindow()
    {
        displayText.text = "T12 激活信息窗口";
        WriteToLog("T12_InfoWindow_Open");
    }

    public void CloseInfoWindow()
    {
        displayText.text = "T12 关闭信息窗口";
        WriteToLog("T12_InfoWindow_Close");
    }

    public void ResetToOriginal()
    {
        displayText.text = "T13 复原到初始图像";
        WriteToLog("T13_ResetToOriginal");
    }

    public void UndoLastStep()
    {
        displayText.text = "T14 撤销上一步操作";
        WriteToLog("T14_UndoLastStep");
    }

    public void Flip_sp()
    {
        displayText.text = "T15 翻转-水平";
        WriteToLog("T15_Flip_Horizontal");
    }
    public void Flip_sz()
    {
        displayText.text = "T15 翻转-竖直";
        WriteToLog("T15_Flip_Vertical");
    }

    public void clean_text()
    {
        if (displayText != null) // 确保displayText已在Inspector中赋值
        {
            displayText.text = " ";
        }
        // 根据您的要求，这里不记录特定的手势日志
        // 如果需要，可以记录一个通用事件：WriteToLog("UI_Cleared");
    }

    // (可选) 在Inspector中添加一个按钮，方便在编辑器模式下快速打开日志文件夹
    [ContextMenu("打开日志文件夹 (Open Log Folder)")]
    void EditorHelper_OpenLogFileDirectory()
    {
        if (!isLoggerInitialized || string.IsNullOrEmpty(currentLogFilePath))
        {
            // 尝试初始化，以防在编辑器中未运行Awake时调用
            InitializeSharedFileLogger();
            if (!isLoggerInitialized || string.IsNullOrEmpty(currentLogFilePath))
            {
                Debug.LogError("日志路径未初始化或初始化失败，无法打开文件夹。");
                return;
            }
        }
        try
        {
            string directoryPath = Path.GetDirectoryName(currentLogFilePath);
            if (directoryPath != null)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = directoryPath,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                Debug.LogError("无法确定日志文件的目录路径。");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"无法打开日志文件夹: {e.Message}");
            // 作为备选方案，尝试打开 persistentDataPath 的根目录
            try { System.Diagnostics.Process.Start(Application.persistentDataPath); } catch { }
        }
    }
}