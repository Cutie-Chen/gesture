using System.Collections.Generic;
using UnityEngine;

namespace AI.FSM {
    /// <summary>
    /// <para>有限状态机</para>
    /// <para>子类需要调用父类Start, Update方法，调用并重写setUpFSM方法</para>
    /// </summary>
    public class FSMBase : MonoBehaviour {
        /// <summary>
        /// 初始化状态机，子类需要调用base.setUpFSM()
        /// </summary>
        protected virtual void SetUpFSM() {
            _states = new List<FSMState>();
        }
        /// <summary>
        /// 初始化其他组件
        /// </summary>
        protected virtual void Init() {
        }
        public void loadDefaultState() {
            // 加载默认状态
            _currentState = _defaultState = _states.Find(s => s.StateID == defaultStateID);
            _currentState.OnStateEnter(this);
        }
        private void Awake() {
            Init();
            SetUpFSM();
            loadDefaultState();
        }
        private void Update() {
            // 更新状态
            _currentState.Reason(this);
            _currentState.OnStateStay(this);
            _settledTriggers.Clear();
        }
        private void FixedUpdate() {
            _currentState.OnStateFixedStay(this);
        }
        /// <summary>
        /// 切换当前状态至目标状态
        /// </summary>
        /// <param name="targetStateID">目标状态ID</param>
        public void ChangeActiveState(FSMStateID targetStateID) {
            _currentState.OnStateExit(this);
            _currentState = targetStateID == FSMStateID.Default ? _defaultState : _states.Find(s => s.StateID == targetStateID);
            _currentState.OnStateEnter(this);
        }

        public void SetTrigger(FSMTriggerID triggerID) {
            _settledTriggers.Add(triggerID);
        }
        public void ResetTrigger(FSMTriggerID triggerID) {
            _settledTriggers.Remove(triggerID);
        }
        public bool HasTrigger(FSMTriggerID triggerID) {
            return _settledTriggers.Contains(triggerID);
        }

        public FSMStateID defaultStateID;
        protected FSMState _defaultState;
        protected List<FSMState> _states;
        public FSMState _currentState;
        protected SortedSet<FSMTriggerID> _settledTriggers = new SortedSet<FSMTriggerID>();
        public FSMStateID CurrentState {
            get {
                return _currentState.StateID;
            }
        }
        public Animator animator;
    }
}