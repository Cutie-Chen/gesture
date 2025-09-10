using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.PoseDetection;

public class DoubleTapDetection : MonoBehaviour
{
    [SerializeField] private JointVelocityActiveState _tapDetector;
    [SerializeField, Range(0.1f, 0.5f)] private float _doubleTapTimeWindow = 0.3f;

    private bool _firstTapDetected;
    private float _firstTapTimestamp;
    private bool _doubleTapActive;

    public bool IsDoubleTapActive
    {
        get { return _doubleTapActive; }
    }

    private void Update()
    {
        CheckForDoubleTap();
    }

    private void CheckForDoubleTap()
    {
        if (_tapDetector.Active)
        {
            HandleTapEvent();
        }
        else
        {
            CheckTimeExpired();
        }
    }

    private void HandleTapEvent()
    {
        float currentTime = Time.time;

        if (!_firstTapDetected)
        {
            // 记录第一次点击
            _firstTapDetected = true;
            _firstTapTimestamp = currentTime;
        }
        else
        {
            // 检测第二次点击
            if (currentTime - _firstTapTimestamp <= _doubleTapTimeWindow)
            {
                _doubleTapActive = true;
                ResetDetection();
            }
        }
    }

    private void CheckTimeExpired()
    {
        if (_firstTapDetected &&
            Time.time - _firstTapTimestamp > _doubleTapTimeWindow)
        {
            ResetDetection();
        }
    }

    private void ResetDetection()
    {
        _firstTapDetected = false;
        _doubleTapActive = false;
        _firstTapTimestamp = 0;
    }

    // 手动重置状态（可选）
    public void ForceReset()
    {
        ResetDetection();
    }
}