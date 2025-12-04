using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 如果你需要操作UI组件，保留这个引用

public class MainMenuManager : MonoBehaviour
{
    [Header("在此处直接修改当前被试名字")]
    [Tooltip("每次换人实验时，在这里改一下，比如 P01, P02...")]
    public string participantName = "P01";

    // 全局变量
    public static string GlobalParticipantID = "P01";

    void Start()
    {
        // 游戏一开始，就把你填的名字存到全局变量里
        GlobalParticipantID = participantName;
    }

    // --- 绑定到【手势按钮】的方法 ---
    public void OnStartTest(string sceneName)
    {
        // 确保名字是最新的
        GlobalParticipantID = participantName;

        // 检查场景是否存在，防止报错
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"无法加载场景: {sceneName}，请检查 Build Settings！");
        }
    }

    // --- 绑定到【退出程序按钮】的方法 (新增) ---
    public void OnQuitButtonClick()
    {
        Debug.Log("正在退出程序...");

        // 1. 在打包后的 .exe / .apk 中，这行代码会关闭程序
        Application.Quit();

        // 2. 在 Unity 编辑器里，这行代码会停止播放（方便你测试）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}