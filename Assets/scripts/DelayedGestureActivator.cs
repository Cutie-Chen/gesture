using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DelayedGestureActivator : MonoBehaviour
{
    [Tooltip("手势需要保持的秒数才能被激活。建议从较小的值如0.3-0.5秒开始测试。")]
    public float holdDuration = 0.5f;

    [Tooltip("当手势成功保持指定时间后触发此事件")]
    public UnityEvent onGestureHeld;

    [Tooltip("当一个已确认（已保持）的手势随后被释放时触发此事件")]
    public UnityEvent onConfirmedGestureReleased; // <--- 新增的事件

    [Tooltip("当手势在计时期间释放时触发（可选）")]
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
            if (!gestureWasConfirmedBeforeThisRelease) // 只有在未确认前释放才触发 Early release
            {
                onGestureReleasedEarly.Invoke();
                Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: onGestureReleasedEarly invoked.");
            }
        }

        // 重置状态标志
        _isPotentiallyHolding = false;
        _isGestureConfirmedHeld = false;

        // 如果这个“释放”信号是在一个手势已经被确认为“保持”之后发生的
        if (gestureWasConfirmedBeforeThisRelease)
        {
            Debug.Log($"[{Time.timeSinceLevelLoad:F2}s] {gameObject.name}: Confirmed gesture has now been released. Invoking onConfirmedGestureReleased.");
            onConfirmedGestureReleased.Invoke(); // <--- 触发新增的事件
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