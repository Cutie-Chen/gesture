using System;
using UnityEngine;
using Oculus.Interaction.Input;
using System.Collections.Generic;
using TMPro;

namespace Oculus.Interaction.PoseDetection
{
    public class DoubleVelocitySequence : MonoBehaviour, IActiveState
    {
        [SerializeField]
        private JointVelocityActiveState forward_dector;
        [SerializeField]
        private JointVelocityActiveState back_dector;
        [SerializeField]
        private TransformRecognizerActiveState tran;
        [SerializeField]
        private ShapeRecognizerActiveState shape;

        [SerializeField, Min(0.1f)]
        private float _maxInterval = 0.5f;

        [SerializeField, Min(0)]
        private float _cooldown = 0.2f;

        public TextMeshProUGUI t;

        private enum DetectionState
        {
            WaitingFirst,
            WaitingSecond,
            Cooldown
        }

        private DetectionState _currentState;
        private float _firstDetectionTime;
        private float _lastDetectionTime;
        private bool _activated;
        private bool f_activate = false;

        private bool windowOpen = false; // 窗口是否打开

        public bool Active => _activated;

        private void Update()
        {
            if (tran.Active && shape.Active)
            {
                UpdateStateMachine();
            }
            CleanState();
        }

        private void UpdateStateMachine()
        {
            switch (_currentState)
            {
                case DetectionState.WaitingFirst:
                    if (forward_dector.Active)
                    {
                        f_activate = true;
                    }
                    if (f_activate && back_dector.Active)
                    {
                        _firstDetectionTime = Time.time;
                        _currentState = DetectionState.WaitingSecond;
                        f_activate = false;
                    }
                    break;

                case DetectionState.WaitingSecond:
                    if (Time.time - _firstDetectionTime > _maxInterval)
                    {
                        ResetState();
                    }
                    else
                    {
                        if (forward_dector.Active)
                        {
                            f_activate = true;
                        }

                        if (f_activate && back_dector.Active)
                        {
                            _activated = true;
                            _lastDetectionTime = Time.time;
                            _currentState = DetectionState.Cooldown;
                            f_activate = false;

                            // 控制窗口开关逻辑
                            windowOpen = !windowOpen;
                            
                            t.text = windowOpen ? "T10 激活病人片子序列" : "T10 关闭病人片子序列";
                        }
                    }
                    break;

                case DetectionState.Cooldown:
                    if (Time.time - _lastDetectionTime > _cooldown)
                    {
                        ResetState();
                    }
                    break;
            }
        }

        private void CleanState()
        {
            if (_activated && Time.time - _lastDetectionTime > _cooldown)
            {
                _activated = false;
                f_activate = false;
            }
        }

        private void ResetState()
        {
            _currentState = DetectionState.WaitingFirst;
            _firstDetectionTime = 0;
            _activated = false;
            f_activate = false;
        }

        #region Inject
        public void InjectAllDoubleVelocitySequence(
            JointVelocityActiveState velocityDetector,
            float maxInterval,
            float cooldown)
        {
            InjectVelocityDetector(velocityDetector);
            _maxInterval = maxInterval;
            _cooldown = cooldown;
        }

        public void InjectVelocityDetector(JointVelocityActiveState velocityDetector)
        {
            forward_dector = velocityDetector;
        }
        #endregion
    }

}