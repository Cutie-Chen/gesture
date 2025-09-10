using UnityEngine;
using TMPro;
using System.IO; // �����ļ���д
using System;   // ���� DateTime �� Guid

using Oculus.Interaction;
public class GestureFunctionDisplay : MonoBehaviour
{
    public TextMeshProUGUI displayText;

    // --- ��־��¼��صľ�̬��Ա ---
    // ��̬��Աȷ����ʹ�ж���˽ű���ʵ����Ҳֻ����ͬһ����־�ļ��ͻỰ
    private static string currentLogFilePath;
    private static string currentSessionID;
    private static bool isLoggerInitialized = false; //ȷ����ʼ��ִֻ��һ��
    private static readonly object fileWriteLock = new object(); // ����ͬ���ļ�д���������ֹ��ͻ

    private const string LOG_FILE_NAME = "MyGestureActionsLog.csv"; // �������Զ�����־�ļ���
    private const string CSV_FILE_HEADER = "Timestamp,SessionID,FrameCount,GestureName,Status";

    [SerializeField]
    public ActiveStateGroup T5;
    public ActiveStateGroup T9;
    void Awake()
    {
        // ����һ���ԵĹ�����־��ʼ������
        InitializeSharedFileLogger();
    }

    // ��̬�ĳ�ʼ��������ȷ��ִֻ��һ��
    private static void InitializeSharedFileLogger()
    {
        lock (fileWriteLock) // ʹ����ȷ���̰߳�ȫ������Unity���̲߳����������Ǻ�ϰ�ߣ�
        {
            if (isLoggerInitialized)
            {
                return; // ����Ѿ���ʼ�����ˣ���ֱ�ӷ���
            }

            currentSessionID = Guid.NewGuid().ToString();
            // Application.persistentDataPath ����Quest���豸�ϰ�ȫ��д�ı�׼·��
            currentLogFilePath = Path.Combine(Application.persistentDataPath, LOG_FILE_NAME);

            try
            {
                if (!File.Exists(currentLogFilePath))
                {
                    // �ļ������ڣ�������д���ͷ
                    File.WriteAllText(currentLogFilePath, CSV_FILE_HEADER + Environment.NewLine);
                    Debug.Log($"������־�ļ��Ѵ���: {currentLogFilePath}");
                }
                else
                {
                    // �ļ��Ѵ��ڣ�����׷��һ���»Ự��ʼ�ı�ǣ���ѡ��
                    File.AppendAllText(currentLogFilePath, $"# New Session Started: {currentSessionID}{Environment.NewLine}");
                    Debug.Log($"������־��׷�ӵ������ļ�: {currentLogFilePath}");
                }
                isLoggerInitialized = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"��ʼ��������־�ļ�ʧ��: {e.Message}");
                // ��ʼ��ʧ�ܣ�isLoggerInitialized ���� false��������־��¼����Ӱ��
            }
        }
    }

    // �ڲ����������ڽ���Ŀд����־�ļ�
    private void WriteToLog(string gestureNameForLog)
    {
        if (!isLoggerInitialized)
        {
            Debug.LogWarning("��־��¼��δ��ʼ�����޷���¼��");
            // ���Գ����ٴγ�ʼ����������ʾ�û��������
            // InitializeSharedFileLogger(); // �ٴγ��Գ�ʼ��
            // if (!isLoggerInitialized) return; // �������ʧ�ܣ������
            return;
        }

        if (string.IsNullOrEmpty(gestureNameForLog))
        {
            Debug.LogWarning("������־����Ϊ�գ������м�¼��");
            return;
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        int frameCount = Time.frameCount;
        // ������������״̬���ǡ�Triggered����Executed��
        string status = "Triggered";
        string logLine = $"{timestamp},{currentSessionID},{frameCount},{EscapeCSVField(gestureNameForLog)},{status}";

        lock (fileWriteLock) // д���ļ�ʱ����
        {
            try
            {
                File.AppendAllText(currentLogFilePath, logLine + Environment.NewLine);
            }
            catch (Exception e)
            {
                Debug.LogError($"д����־��Ŀʧ��: {e.Message} | ��Ŀ: {logLine}");
            }
        }
    }

    // CSVת�帨������
    private string EscapeCSVField(string field)
    {
        if (string.IsNullOrEmpty(field)) return "";
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }

    // --- ��ԭ���ķ��������ڼ�������־��¼ ---
    public void Zoom()
    {
        displayText.text = "T1 ����-�Ŵ�";
        WriteToLog("T1_Zoom_Enlarge");
    }
    public void Scale()
    {
        displayText.text = "T1 ����-��С";
        WriteToLog("T1_Zoom_Shrink");
    }
    public void SwitchSlice_up()
    {
        displayText.text = "T2 �л��ϲ�-��";
        WriteToLog("T2_SwitchSlice_Up");
    }
    public void SwitchSlice_down()
    {
        displayText.text = "T2 �л��ϲ�-��";
        WriteToLog("T2_SwitchSlice_Down");
    }
    public void Pan()
    {
        if (!T5 && !T9) {
            displayText.text = "T3 ƽ�ƣ��������ң�";
            WriteToLog("T3_Pan");
        }
    }

    public void Rotate()
    {
        displayText.text = "T4 ��ת��˳����ʱ�룩";
        WriteToLog("T4_Rotate");
    }

    public void LayoutPanels1()
    {
        displayText.text = "T5 ����-1";
        WriteToLog("T5_Layout_1");
    }
    public void LayoutPanels2()
    {
        displayText.text = "T5 ����-2";
        WriteToLog("T5_Layout_2");
    }
    public void LayoutPanels3()
    {
        displayText.text = "T5 ����-3";
        WriteToLog("T5_Layout_3");
    }
    public void LayoutPanels4()
    {
        displayText.text = "T5 ����-4";
        WriteToLog("T5_Layout_4");
    }

    public void OpenRuler()
    {
        displayText.text = "T6 �򿪱��";
        WriteToLog("T6_Ruler_Open");
    }

    public void CloseRuler()
    {
        displayText.text = "T6 �رձ��";
        WriteToLog("T6_Ruler_Close");
    }

    public void AdjustWindowLevel_up()
    {
        displayText.text = "T7 ���ڴ�λ-����";
        WriteToLog("T7_WindowLevel_Up");
    }

    public void AdjustWindowLevel_down()
    {
        displayText.text = "T7 ���ڴ�λ-����";
        WriteToLog("T7_WindowLevel_Down");
    }
    public void AdjustWindowWidth_up()
    {
        displayText.text = "T8 ���ڴ���-����";
        WriteToLog("T8_WindowWidth_Up");
    }

    public void AdjustWindowWidth_down()
    {
        displayText.text = "T8 ���ڴ���-����";
        WriteToLog("T8_WindowWidth_Down");
    }
    public void PresetWindow1()
    {
        displayText.text = "T9 Ԥ�贰λ����1";
        WriteToLog("T9_PresetWindow_1");
    }
    public void PresetWindow2()
    {
        displayText.text = "T9 Ԥ�贰λ����2";
        WriteToLog("T9_PresetWindow_2");
    }
    public void ActivateSequence()
    {
        displayText.text = "T10 �����Ƭ������";
        WriteToLog("T10_PatientSequence_Activate");
    }

    public void DeactivateSequence()
    {
        displayText.text = "T10 �رղ���Ƭ������";
        WriteToLog("T10_PatientSequence_Deactivate");
    }

    public void SwitchPanel()
    {
        displayText.text = "T11 �л���壨�������ң�";
        WriteToLog("T11_SwitchPanel");
    }

    public void OpenInfoWindow()
    {
        displayText.text = "T12 ������Ϣ����";
        WriteToLog("T12_InfoWindow_Open");
    }

    public void CloseInfoWindow()
    {
        displayText.text = "T12 �ر���Ϣ����";
        WriteToLog("T12_InfoWindow_Close");
    }

    public void ResetToOriginal()
    {
        displayText.text = "T13 ��ԭ����ʼͼ��";
        WriteToLog("T13_ResetToOriginal");
    }

    public void UndoLastStep()
    {
        displayText.text = "T14 ������һ������";
        WriteToLog("T14_UndoLastStep");
    }

    public void Flip_sp()
    {
        displayText.text = "T15 ��ת-ˮƽ";
        WriteToLog("T15_Flip_Horizontal");
    }
    public void Flip_sz()
    {
        displayText.text = "T15 ��ת-��ֱ";
        WriteToLog("T15_Flip_Vertical");
    }

    public void clean_text()
    {
        if (displayText != null) // ȷ��displayText����Inspector�и�ֵ
        {
            displayText.text = " ";
        }
        // ��������Ҫ�����ﲻ��¼�ض���������־
        // �����Ҫ�����Լ�¼һ��ͨ���¼���WriteToLog("UI_Cleared");
    }

    // (��ѡ) ��Inspector�����һ����ť�������ڱ༭��ģʽ�¿��ٴ���־�ļ���
    [ContextMenu("����־�ļ��� (Open Log Folder)")]
    void EditorHelper_OpenLogFileDirectory()
    {
        if (!isLoggerInitialized || string.IsNullOrEmpty(currentLogFilePath))
        {
            // ���Գ�ʼ�����Է��ڱ༭����δ����Awakeʱ����
            InitializeSharedFileLogger();
            if (!isLoggerInitialized || string.IsNullOrEmpty(currentLogFilePath))
            {
                Debug.LogError("��־·��δ��ʼ�����ʼ��ʧ�ܣ��޷����ļ��С�");
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
                Debug.LogError("�޷�ȷ����־�ļ���Ŀ¼·����");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"�޷�����־�ļ���: {e.Message}");
            // ��Ϊ��ѡ���������Դ� persistentDataPath �ĸ�Ŀ¼
            try { System.Diagnostics.Process.Start(Application.persistentDataPath); } catch { }
        }
    }
}