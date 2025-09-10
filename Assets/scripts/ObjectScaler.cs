using UnityEngine;

public class ObjectScaler : MonoBehaviour // 或者您原来的类名 AdvancedUIScaler
{
    [Header("目标UI元素")]
    [Tooltip("您希望通过改变sizeDelta来调整大小的UI元素的RectTransform组件。")]
    public RectTransform targetRectTransform;

    [Header("尺寸调整参数")]
    [Tooltip("增大尺寸的乘数（例如，1.1 表示增大10%）。必须大于1.0。")]
    public float increaseSizeMultiplier = 1.1f;

    [Tooltip("减小尺寸的乘数（例如，0.9 表示减小10%）。必须小于1.0且大于0。")]
    public float decreaseSizeMultiplier = 0.9f;

    [Header("尺寸限制 (针对 sizeDelta)")]
    [Tooltip("允许的最小 sizeDelta (宽度, 高度)。X对应宽度, Y对应高度。")]
    public Vector2 minSizeDelta = new Vector2(50f, 50f);

    [Tooltip("允许的最大 sizeDelta (宽度, 高度)。X对应宽度, Y对应高度。")]
    public Vector2 maxSizeDelta = new Vector2(1000f, 1000f);

    private Vector2 _initialSizeDelta;
    private bool _isInitialized = false;

    void Start()
    {
        InitializeScaler();
    }

    public void InitializeScaler()
    {
        if (targetRectTransform != null)
        {
            _initialSizeDelta = targetRectTransform.sizeDelta;
            _isInitialized = true;
            Debug.Log($"UISizeDeltaScaler: 为 '{targetRectTransform.gameObject.name}' 初始化。初始 SizeDelta: {_initialSizeDelta}");
        }
        else
        {
            Debug.LogError("UISizeDeltaScaler: 'Target Rect Transform' 未在Inspector中指定!", this);
        }
    }

    /// <summary>
    /// 执行增大尺寸操作
    /// </summary>
    public void PerformIncreaseSize()
    {
        if (!_isInitialized && targetRectTransform != null) InitializeScaler();
        if (targetRectTransform == null)
        {
            Debug.LogError("UISizeDeltaScaler - PerformIncreaseSize: 'Target Rect Transform' 未指定。无法增大尺寸。", this);
            return;
        }

        if (increaseSizeMultiplier <= 1.0f)
        {
            Debug.LogWarning($"UISizeDeltaScaler - PerformIncreaseSize: 'Increase Size Multiplier' ({increaseSizeMultiplier}) 不是一个有效的大于1.0的值。尺寸可能不会增大。", this);
        }

        Vector2 currentSizeDelta = targetRectTransform.sizeDelta;
        Vector2 newSizeDelta = currentSizeDelta;

        if (increaseSizeMultiplier > 1.0f)
        {
            newSizeDelta = new Vector2(
                currentSizeDelta.x * increaseSizeMultiplier,
                currentSizeDelta.y * increaseSizeMultiplier
            );
        }
        else
        {
            Debug.Log($"UISizeDeltaScaler - PerformIncreaseSize: increaseSizeMultiplier ({increaseSizeMultiplier}) 不会导致尺寸增大，保持当前尺寸。");
        }

        // 应用尺寸限制
        newSizeDelta.x = Mathf.Clamp(newSizeDelta.x, minSizeDelta.x, maxSizeDelta.x);
        newSizeDelta.y = Mathf.Clamp(newSizeDelta.y, minSizeDelta.y, maxSizeDelta.y);

        targetRectTransform.sizeDelta = newSizeDelta;
        Debug.Log($"UISizeDeltaScaler - IncreaseSize: 对象='{targetRectTransform.gameObject.name}', 使用倍数={increaseSizeMultiplier}, 前SizeDelta={currentSizeDelta}, 后SizeDelta={targetRectTransform.sizeDelta}");
    }

    /// <summary>
    /// 执行减小尺寸操作
    /// </summary>
    public void PerformDecreaseSize()
    {
        if (!_isInitialized && targetRectTransform != null) InitializeScaler();
        if (targetRectTransform == null)
        {
            Debug.LogError("UISizeDeltaScaler - PerformDecreaseSize: 'Target Rect Transform' 未指定。无法减小尺寸。", this);
            return;
        }

        if (decreaseSizeMultiplier >= 1.0f || decreaseSizeMultiplier <= 0f)
        {
            Debug.LogWarning($"UISizeDeltaScaler - PerformDecreaseSize: 'Decrease Size Multiplier' ({decreaseSizeMultiplier}) 不是一个有效的0到1之间的值。尺寸可能不会减小或行为异常。", this);
        }

        Vector2 currentSizeDelta = targetRectTransform.sizeDelta;
        Vector2 newSizeDelta = currentSizeDelta;

        if (decreaseSizeMultiplier > 0f && decreaseSizeMultiplier < 1.0f)
        {
            newSizeDelta = new Vector2(
                currentSizeDelta.x * decreaseSizeMultiplier,
                currentSizeDelta.y * decreaseSizeMultiplier
            );
        }
        else
        {
            Debug.Log($"UISizeDeltaScaler - PerformDecreaseSize: decreaseSizeMultiplier ({decreaseSizeMultiplier}) 不会导致尺寸减小，保持当前尺寸。");
        }

        // 应用尺寸限制
        newSizeDelta.x = Mathf.Clamp(newSizeDelta.x, minSizeDelta.x, maxSizeDelta.x);
        newSizeDelta.y = Mathf.Clamp(newSizeDelta.y, minSizeDelta.y, maxSizeDelta.y);

        targetRectTransform.sizeDelta = newSizeDelta;
        Debug.Log($"UISizeDeltaScaler - DecreaseSize: 对象='{targetRectTransform.gameObject.name}', 使用倍数={decreaseSizeMultiplier}, 前SizeDelta={currentSizeDelta}, 后SizeDelta={targetRectTransform.sizeDelta}");
    }

    /// <summary>
    /// 将目标对象的尺寸恢复到初始状态。
    /// </summary>
    public void ResetSizeToInitial()
    {
        if (!_isInitialized && targetRectTransform != null) InitializeScaler();
        if (targetRectTransform == null)
        {
            Debug.LogError("UISizeDeltaScaler - ResetSize: 'Target Rect Transform' 未指定。无法复位。", this);
            return;
        }

        if (_isInitialized) // 确保初始值已成功保存
        {
            targetRectTransform.sizeDelta = _initialSizeDelta;
            Debug.Log($"UISizeDeltaScaler - ResetSize: 对象='{targetRectTransform.gameObject.name}' 已复位到初始SizeDelta {_initialSizeDelta}");
        }
        else
        {
            Debug.LogWarning($"UISizeDeltaScaler - ResetSize: 对象 '{targetRectTransform.gameObject.name}' 的尺寸调整器未正确初始化。无法准确复位。", this);
        }
    }
}