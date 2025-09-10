using UnityEngine;

public class ObjectScaler : MonoBehaviour // ������ԭ�������� AdvancedUIScaler
{
    [Header("Ŀ��UIԪ��")]
    [Tooltip("��ϣ��ͨ���ı�sizeDelta��������С��UIԪ�ص�RectTransform�����")]
    public RectTransform targetRectTransform;

    [Header("�ߴ��������")]
    [Tooltip("����ߴ�ĳ��������磬1.1 ��ʾ����10%�����������1.0��")]
    public float increaseSizeMultiplier = 1.1f;

    [Tooltip("��С�ߴ�ĳ��������磬0.9 ��ʾ��С10%��������С��1.0�Ҵ���0��")]
    public float decreaseSizeMultiplier = 0.9f;

    [Header("�ߴ����� (��� sizeDelta)")]
    [Tooltip("�������С sizeDelta (���, �߶�)��X��Ӧ���, Y��Ӧ�߶ȡ�")]
    public Vector2 minSizeDelta = new Vector2(50f, 50f);

    [Tooltip("�������� sizeDelta (���, �߶�)��X��Ӧ���, Y��Ӧ�߶ȡ�")]
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
            Debug.Log($"UISizeDeltaScaler: Ϊ '{targetRectTransform.gameObject.name}' ��ʼ������ʼ SizeDelta: {_initialSizeDelta}");
        }
        else
        {
            Debug.LogError("UISizeDeltaScaler: 'Target Rect Transform' δ��Inspector��ָ��!", this);
        }
    }

    /// <summary>
    /// ִ������ߴ����
    /// </summary>
    public void PerformIncreaseSize()
    {
        if (!_isInitialized && targetRectTransform != null) InitializeScaler();
        if (targetRectTransform == null)
        {
            Debug.LogError("UISizeDeltaScaler - PerformIncreaseSize: 'Target Rect Transform' δָ�����޷�����ߴ硣", this);
            return;
        }

        if (increaseSizeMultiplier <= 1.0f)
        {
            Debug.LogWarning($"UISizeDeltaScaler - PerformIncreaseSize: 'Increase Size Multiplier' ({increaseSizeMultiplier}) ����һ����Ч�Ĵ���1.0��ֵ���ߴ���ܲ�������", this);
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
            Debug.Log($"UISizeDeltaScaler - PerformIncreaseSize: increaseSizeMultiplier ({increaseSizeMultiplier}) ���ᵼ�³ߴ����󣬱��ֵ�ǰ�ߴ硣");
        }

        // Ӧ�óߴ�����
        newSizeDelta.x = Mathf.Clamp(newSizeDelta.x, minSizeDelta.x, maxSizeDelta.x);
        newSizeDelta.y = Mathf.Clamp(newSizeDelta.y, minSizeDelta.y, maxSizeDelta.y);

        targetRectTransform.sizeDelta = newSizeDelta;
        Debug.Log($"UISizeDeltaScaler - IncreaseSize: ����='{targetRectTransform.gameObject.name}', ʹ�ñ���={increaseSizeMultiplier}, ǰSizeDelta={currentSizeDelta}, ��SizeDelta={targetRectTransform.sizeDelta}");
    }

    /// <summary>
    /// ִ�м�С�ߴ����
    /// </summary>
    public void PerformDecreaseSize()
    {
        if (!_isInitialized && targetRectTransform != null) InitializeScaler();
        if (targetRectTransform == null)
        {
            Debug.LogError("UISizeDeltaScaler - PerformDecreaseSize: 'Target Rect Transform' δָ�����޷���С�ߴ硣", this);
            return;
        }

        if (decreaseSizeMultiplier >= 1.0f || decreaseSizeMultiplier <= 0f)
        {
            Debug.LogWarning($"UISizeDeltaScaler - PerformDecreaseSize: 'Decrease Size Multiplier' ({decreaseSizeMultiplier}) ����һ����Ч��0��1֮���ֵ���ߴ���ܲ����С����Ϊ�쳣��", this);
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
            Debug.Log($"UISizeDeltaScaler - PerformDecreaseSize: decreaseSizeMultiplier ({decreaseSizeMultiplier}) ���ᵼ�³ߴ��С�����ֵ�ǰ�ߴ硣");
        }

        // Ӧ�óߴ�����
        newSizeDelta.x = Mathf.Clamp(newSizeDelta.x, minSizeDelta.x, maxSizeDelta.x);
        newSizeDelta.y = Mathf.Clamp(newSizeDelta.y, minSizeDelta.y, maxSizeDelta.y);

        targetRectTransform.sizeDelta = newSizeDelta;
        Debug.Log($"UISizeDeltaScaler - DecreaseSize: ����='{targetRectTransform.gameObject.name}', ʹ�ñ���={decreaseSizeMultiplier}, ǰSizeDelta={currentSizeDelta}, ��SizeDelta={targetRectTransform.sizeDelta}");
    }

    /// <summary>
    /// ��Ŀ�����ĳߴ�ָ�����ʼ״̬��
    /// </summary>
    public void ResetSizeToInitial()
    {
        if (!_isInitialized && targetRectTransform != null) InitializeScaler();
        if (targetRectTransform == null)
        {
            Debug.LogError("UISizeDeltaScaler - ResetSize: 'Target Rect Transform' δָ�����޷���λ��", this);
            return;
        }

        if (_isInitialized) // ȷ����ʼֵ�ѳɹ�����
        {
            targetRectTransform.sizeDelta = _initialSizeDelta;
            Debug.Log($"UISizeDeltaScaler - ResetSize: ����='{targetRectTransform.gameObject.name}' �Ѹ�λ����ʼSizeDelta {_initialSizeDelta}");
        }
        else
        {
            Debug.LogWarning($"UISizeDeltaScaler - ResetSize: ���� '{targetRectTransform.gameObject.name}' �ĳߴ������δ��ȷ��ʼ�����޷�׼ȷ��λ��", this);
        }
    }
}