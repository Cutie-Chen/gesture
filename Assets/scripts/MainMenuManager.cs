using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("在此处直接修改当前被试名字")]
    [Tooltip("每次换人实验时，在这里改一下，比如 P01, P02...")]
    public string participantName = "P01";

    // 全局变量，其他场景通过 MainMenuManager.GlobalParticipantID 访问
    public static string GlobalParticipantID = "P01";

    void Start()
    {
        // 游戏一开始，就把你填的名字存到全局变量里
        GlobalParticipantID = participantName;
    }

    // =========================================================
    // 绑定到各个【手势实验按钮】的方法
    // =========================================================
    public void OnStartTest(string sceneName)
    {
        // 1. 点击按钮时，再次确保名字是最新的（防止你运行后在Inspector改了没生效）
        GlobalParticipantID = participantName;

        // 2. 安全检查：防止场景名字写错导致报错
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"【错误】找不到场景: {sceneName}。请检查：\n1. Build Settings里是否添加了该场景？\n2. 名字是否拼写正确？");
        }
    }

    // =========================================================
    // 绑定到【退出/关闭按钮】的方法 (新增)
    // =========================================================
    public void OnQuitButtonClick()
    {
        Debug.Log("检测到退出操作，正在关闭程序...");

        // 1. 如果是打包后的 .exe 或 .apk，这行代码会关闭程序
        Application.Quit();

        // 2. 如果是在 Unity 编辑器里运行，这行代码会停止播放 (方便调试)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}