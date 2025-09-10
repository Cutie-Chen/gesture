using Oculus.Interaction;
using UnityEngine;
using TMPro;

public class T15detect : MonoBehaviour, IActiveState
{
    [SerializeField] private ActiveStateGroup palmdown;
    [SerializeField] private ActiveStateGroup palmup;

    // ������ϵ�ʱ�����ƣ�˫������Ҫ�����ʱ�䷶Χ�����
    [SerializeField, Range(0.1f, 3f)] private float _maxInterval = 0.8f;

    // ÿ������Ҫ���ֵ����ʱ�䣬��ֹ������ʽ�󴥡�
    [SerializeField, Range(0.05f, 0.5f)] private float _minHoldTime = 0.1f;
    //public TextMeshProUGUI t;

    // ״̬���е��ĸ�״̬����˳��ִ��
    private enum State { Idle, T15 }
    private State _currentState = State.Idle;

    // ��ǰ״̬�Ľ���ʱ��
    private float _stateEnterTime;

    // �ӵ�һ�Ρ���ȭ����ʼ��ʱ����㣬��������ʱ�����
    private float _firstStepTime;

    // ��������Ƿ����һ���׶�����
    private bool _activated = false;
    public bool Active => _activated;

    // ���ڼ�⡰�����Ƿ�ոմ�����
    private bool _wasFistActive = false;
    private bool _wasThumbOutActive = false;

    private void Update()
    {
        // ��ǰ֡����������״̬
        bool isFistNow = palmdown.Active;
        bool isThumbNow = palmup.Active;

        // ��Ե������⣺�Ƿ񡰸ո�ʶ�𵽡�ĳ����
        bool LJustActivated = isFistNow && !_wasFistActive;
        bool touchJustActivated = isThumbNow && !_wasThumbOutActive;

        // ������һ֡��¼
        _wasFistActive = isFistNow;
        _wasThumbOutActive = isThumbNow;

        // ״̬�����߼�
        switch (_currentState)
        {
            case State.Idle:
                if (LJustActivated)
                {
                    StartStep(State.T15); 
                    //t.text = "xia";
                }
                break;

            case State.T15:
                if (TimeInState() > _minHoldTime && touchJustActivated)//thumbJustActivated)
                {
                    _activated = true;// �����ɹ�ʶ��
                    //t.text = "�ɹ�";

                }
                else if (TimeSinceFirstStep() > _maxInterval)
                {
                    //t.text = "���峬ʱ";
                    ResetState();// ���峬ʱ
                    return;
                }
                break;

        }
    }

    // ������״̬��ͳһ����������¼ʱ�䣩
    private void StartStep(State newState)
    {
        _currentState = newState;
        _stateEnterTime = Time.time;

        // ��¼��һ����ʼ��ʱ�䣨������ʱ���жϣ�
        if (newState == State.T15)
            _firstStepTime = Time.time;
    }
    // ����״̬��
    private void ResetState()
    {
        _currentState = State.Idle;
        _stateEnterTime = 0;
        _firstStepTime = 0;
        _activated = false;
    }

    // ��ǰ״̬����ʱ��
    private float TimeInState() => Time.time - _stateEnterTime;

    // �ӵ�һ������ȭ�������ڵ���ʱ��
    private float TimeSinceFirstStep() => Time.time - _firstStepTime;
}
