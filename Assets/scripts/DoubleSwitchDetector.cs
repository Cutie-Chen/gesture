using Oculus.Interaction;
using UnityEngine;
using TMPro;
public class DoubleSwitchDetector : MonoBehaviour, IActiveState
{
    [SerializeField] private ActiveStateGroup _fistDetector;
    [SerializeField] private ActiveStateGroup _thumbOutDetector;

    // 手势组合的时间限制：双击手势要在这个时间范围内完成
    [SerializeField, Range(0.1f, 3f)] private float _maxInterval = 0.8f;

    // 每个手势要保持的最短时间，防止“抖动式误触”
    [SerializeField, Range(0.05f, 0.5f)] private float _minHoldTime = 0.1f;
    public TextMeshProUGUI t;

    // 状态机中的四个状态，按顺序执行
    private enum State { Idle, FistDetected, ThumbOut1, FistAgain }
    private State _currentState = State.Idle;

    // 当前状态的进入时间
    private float _stateEnterTime;

    // 从第一次“握拳”开始计时的起点，用于整体时间控制
    private float _firstStepTime;

    // 检测结果（是否完成一整套动作）
    private bool _activated=false;
    public bool Active => _activated;

    // 用于检测“手势是否刚刚触发”
    private bool _wasFistActive = false;
    private bool _wasThumbOutActive = false;
    private bool window_open = false;//判断当前窗口出于什么状态
    private void Update()
    {
        bool isFistNow = _fistDetector.Active;
        bool isThumbNow = _thumbOutDetector.Active;

        bool fistJustActivated = isFistNow && !_wasFistActive;
        bool thumbJustActivated = isThumbNow && !_wasThumbOutActive;

        _wasFistActive = isFistNow;
        _wasThumbOutActive = isThumbNow;

        // 如果窗口是打开状态，且用户只是握拳一次，就关闭窗口
        if (window_open && _currentState == State.Idle && fistJustActivated)
        {
            window_open = false;
            t.text = "T12 关闭信息窗口";
            ResetState();
            return;
        }

        switch (_currentState)
        {
            case State.Idle:
                if (fistJustActivated)
                {
                    StartStep(State.FistDetected); // 第一步：握拳
                    //t.text = "握拳！";
                }
                break;

            case State.FistDetected:
                if (TimeInState() > _minHoldTime && thumbJustActivated)
                {
                    StartStep(State.ThumbOut1); // 第二步：拇指伸出
                    //t.text = "大拇指！";
                }
                else if (TimeInState() > _maxInterval * 0.5f)
                {
                    //t.text = "超时";
                    ResetState();
                }
                break;

            case State.ThumbOut1:
                if (fistJustActivated)
                {
                    StartStep(State.FistAgain); // 第三步：再握拳
                    //t.text = "第二次握拳！";
                }
                else if (TimeInState() > _maxInterval * 0.5f)
                {
                    //t.text = "超时";
                    ResetState();
                }
                break;

            case State.FistAgain:
                if (TimeInState() > _minHoldTime && thumbJustActivated)
                {
                    _activated = true;

                    // 🟢 只有在窗口关闭的情况下，T12 才执行“打开”
                    if (!window_open)
                    {
                        window_open = true;
                        t.text = "T12 激活信息窗口";
                    }

                    ResetState();
                }
                else if (TimeSinceFirstStep() > _maxInterval)
                {
                    t.text = "整体超时";
                    ResetState();
                    return;
                }
                break;
        }
    }


    // 进入新状态的统一方法（并记录时间）
    private void StartStep(State newState)
    {
        _currentState = newState;
        _stateEnterTime = Time.time;

        // 记录第一步开始的时间（用于总时长判断）
        if (newState == State.FistDetected)
            _firstStepTime = Time.time;
    }
    // 重置状态机
    private void ResetState()
    {
        _currentState = State.Idle;
        _stateEnterTime = 0;
        _firstStepTime = 0;
        _activated = false;
    }

    // 当前状态持续时间
    private float TimeInState() => Time.time - _stateEnterTime;

    // 从第一步（握拳）到现在的总时间
    private float TimeSinceFirstStep() => Time.time - _firstStepTime;
}