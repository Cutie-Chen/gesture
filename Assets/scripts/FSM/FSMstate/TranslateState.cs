namespace AI.FSM {
    public class TranslateState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Translate;
        }
        // 从GestureFSM获取grabbedItem，并调用对应函数处理
        public override void OnStateEnter(FSMBase fsm) {
            base.OnStateEnter(fsm);
            var gestureFSM = fsm as GestureFSM;
            gestureFSM.translateTransformer.Initialize(gestureFSM.grabbedItem);
            gestureFSM.translateTransformer.BeginTransform();
        }
        public override void OnStateStay(FSMBase fsm) {
            base.OnStateStay(fsm);
            var gestureFSM = fsm as GestureFSM;
            gestureFSM.translateTransformer.UpdateTransform();
        }
        public override void OnStateExit(FSMBase fsm) {
            base.OnStateExit(fsm);
            var gestureFSM = fsm as GestureFSM;
            gestureFSM.translateTransformer.EndTransform();
        }
    }
}