using Oculus.Interaction;
using UnityEngine;
using TMPro;

public class T1detect : MonoBehaviour, IActiveState
{
    [SerializeField] private ActiveStateGroup L;
    [SerializeField] private ActiveStateGroup touch;

    // 手势组合的时间限制：双击手势要在这个时间范围内完成
    [SerializeField, Range(0.1f, 3f)] private float _maxInterval = 0.8f;

    // 每个手势要保持的最短时间，防止“抖动式误触”
    [SerializeField, Range(0.05f, 0.5f)] private float _minHoldTime = 0.1f;
    public TextMeshProUGUI t;

    // 状态机中的四个状态，按顺序执行
    private enum State { Idle, T1}
    private State _currentState = State.Idle;

    // 当前状态的进入时间
    private float _stateEnterTime;

    // 从第一次“握拳”开始计时的起点，用于整体时间控制
    private float _firstStepTime;

    // 检测结果（是否完成一整套动作）
    private bool _activated = false;
    public bool Active => _activated;

    // 用于检测“手势是否刚刚触发”
    private bool _wasFistActive = false;
    private bool _wasThumbOutActive = false;

    private void Update()
    {
        // 当前帧的两个手势状态
        bool isFistNow = L.Active;
        bool isThumbNow = touch.Active;

        // 边缘触发检测：是否“刚刚识别到”某手势
        bool LJustActivated = isFistNow && !_wasFistActive;
        bool touchJustActivated = isThumbNow && !_wasThumbOutActive;

        // 更新上一帧记录
        _wasFistActive = isFistNow;
        _wasThumbOutActive = isThumbNow;

        // 状态机主逻辑
        switch (_currentState)
        {
            case State.Idle:
                if (LJustActivated)
                {
                    StartStep(State.T1); // 第一步：握拳
                    //t.text = "没接触";
                }
                break;

            case State.T1:
                if (TimeInState() > _minHoldTime && touchJustActivated)//thumbJustActivated)
                {
                    _activated = true;// 动作成功识别
                    //t.text = "T1手势完成";
                }
                else if (TimeSinceFirstStep() > _maxInterval)
                {
                    //t.text = "整体超时";
                    ResetState();// 整体超时
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
        if (newState == State.T1)
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
