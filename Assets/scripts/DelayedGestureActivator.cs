using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DelayedGestureActivator : MonoBehaviour
{
    [Tooltip("������Ҫ���ֵ��������ܱ��������ӽ�С��ֵ��0.3-0.5�뿪ʼ���ԡ�")]
    public float holdDuration = 0.5f;

    [Tooltip("�����Ƴɹ�����ָ��ʱ��󴥷����¼�")]
    public UnityEvent onGestureHeld;

    [Tooltip("��һ����ȷ�ϣ��ѱ��֣�����������ͷ�ʱ�������¼�")]
    public UnityEvent onConfirmedGestureReleased; // <--- �������¼�

    [Tooltip("�������ڼ�ʱ�ڼ��ͷ�ʱ��������ѡ��")]
    public UnityEvent onGestureReleasedEarly;

    private Coroutine _holdCoroutine;
    private bool _isPotentiallyHolding = false;
    private bool _isGestureConfirmedHeld = false;

    public void OnPoseInitiallyDetected()
    {
        if (_isGestureConfirmedHeld && _isPotentiallyHolding)
        {
            Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: PoseInitiallyDetected called, but gesture already confirmed and potentially holding. No action.");
            return;
        }

        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Previous hold coroutine stopped.");
        }

        _isPotentiallyHolding = true;
        _isGestureConfirmedHeld = false;
        _holdCoroutine = StartCoroutine(HoldCheckCoroutine());
        Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Pose initially detected. Starting {holdDuration}s hold timer.");
    }

    public void OnPoseReleased()
    {
        Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Pose released signal received (called by SDK's immediate unselect).");

        bool gestureWasConfirmedBeforeThisRelease = _isGestureConfirmedHeld;

        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            _holdCoroutine = null;
            Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Hold timer cancelled because pose was released.");
            if (!gestureWasConfirmedBeforeThisRelease) // ֻ����δȷ��ǰ�ͷŲŴ��� Early release
            {
                onGestureReleasedEarly.Invoke();
                Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: onGestureReleasedEarly invoked.");
            }
        }

        // ����״̬��־
        _isPotentiallyHolding = false;
        _isGestureConfirmedHeld = false;

        // ���������ͷš��ź�����һ�������Ѿ���ȷ��Ϊ�����֡�֮������
        if (gestureWasConfirmedBeforeThisRelease)
        {
            Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Confirmed gesture has now been released. Invoking onConfirmedGestureReleased.");
            onConfirmedGestureReleased.Invoke(); // <--- �����������¼�
        }
    }

    private IEnumerator HoldCheckCoroutine()
    {
        yield return new WaitForSeconds(holdDuration);

        if (_isPotentiallyHolding)
        {
            _isGestureConfirmedHeld = true;
            Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Pose successfully held for {holdDuration}s. Invoking onGestureHeld.");
            onGestureHeld.Invoke();
        }
        else
        {
            Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Hold check coroutine finished, but pose was released during the wait. Not invoking onGestureHeld.");
        }
        _holdCoroutine = null;
    }

    void OnDisable()
    {
        if (_holdCoroutine != null)
        {
            StopCoroutine(_holdCoroutine);
            _holdCoroutine = null;
        }
        _isPotentiallyHolding = false;
        _isGestureConfirmedHeld = false;
        Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Disabled. All states reset.");
    }
}