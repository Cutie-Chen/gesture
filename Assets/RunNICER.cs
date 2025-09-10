using UnityEngine;
using System.IO; // 1. ���� IO �����ռ������ļ�����
using NICER_Unity_API;

public class RunNICER : MonoBehaviour
{
    [Header("API �������")]
    [Tooltip("�뽫�����й����� NICER_API �������Ϸ�����ϵ�����")]
    public NICER_API nicerAPI;

    [Header("ʵʱ�ؽ����� (�����Ľ�ɫģ������ק)")]
    public Transform shoulderJoint;
    public Transform elbowJoint;
    public Transform wristJoint;
    public Transform handJoint;

    [Header("�û�����")]
    [Tooltip("�û��������Ա����ڼ���ģ�Ͳ���")]
    public string gender = "Male";

    [Header("������")]
    [Tooltip("��ǰ�������ƣ�Ͷ� (0 �� 100)")]
    public double fatigueLevel;
    [Tooltip("Ԥ�Ƶ��������ʱ�� (��)")]
    public double enduranceTime;

    [Header("�ļ���־����")]
    [Tooltip("��־�ļ�������")]
    public string logFileName = "nicer_log.csv";

    private float totalTime = 0f;
    private StreamWriter logFileWriter; // 2. ����һ�� StreamWriter ��������д���ļ�

    void Start()
    {
        // ����Ҫ������͹ؽ��Ƿ��Ѿ�����
        if (nicerAPI == null || shoulderJoint == null || elbowJoint == null || wristJoint == null || handJoint == null)
        {
            Debug.LogError("�������� Inspector �������������е� API ����͹ؽ� Transform��");
            this.enabled = false;
            return;
        }

        // --- �ļ���ʼ������ ---
        InitializeLogFile();

        Debug.Log("NICER ʵʱ׷����׼����������־��¼��������");
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

        // 4. ����ǰ֡������д���ļ�
        if (logFileWriter != null)
        {
            string dataLine = $"{totalTime:F3},{fatigueLevel:F4},{enduranceTime:F4}";
            logFileWriter.WriteLine(dataLine);
        }
    }

    // 5. ���ű����ٻ�Ӧ���˳�ʱ��ȷ���ļ�����ȷ�ر�
    void OnDestroy()
    {
        if (logFileWriter != null)
        {
            Debug.Log("���ڹر���־�ļ�...");
            logFileWriter.Close();
            logFileWriter = null;
        }
    }

    // 3. ��ʼ���ļ���д�����ķ���
    private void InitializeLogFile()
    {
        // Application.persistentDataPath �� Unity ��֤������ƽ̨�϶���д�İ�ȫ·��
        string filePath = Path.Combine(Application.persistentDataPath, logFileName);

        try
        {
            // �����򸲸��ļ�����׼��д�롣����Ϊ false ��ʾ��׷�ӣ�ÿ�����ж��������ļ���
            logFileWriter = new StreamWriter(filePath, false);

            // д�� CSV �ļ��ı�ͷ
            logFileWriter.WriteLine("TotalTime,FatigueLevel,EnduranceTime");

            Debug.Log($"��־�ļ���������λ�ô���: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"�޷�������־�ļ�: {e.Message}");
            logFileWriter = null; // ���ʧ�ܣ�ȷ��д����Ϊ��
        }
    }
}