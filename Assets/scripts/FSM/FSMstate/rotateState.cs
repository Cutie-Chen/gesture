namespace AI.FSM {
    public class RotateState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Rotate;

        }
        // 从GestureFSM获取grabbedItem，并调用对应函数处理
        public override void OnStateEnter(FSMBase fsm) {
            base.OnStateEnter(fsm);
            var gestureFSM = fsm as GestureFSM;
            gestureFSM.rotateTransformer.Initialize(gestureFSM.grabbedItem);
            gestureFSM.rotateTransformer.BeginTransform();
            
        }
        public override void OnStateStay(FSMBase fsm) {
            base.OnStateStay(fsm);
            var gestureFSM = fsm as GestureFSM;
            gestureFSM.rotateTransformer.UpdateTransform();

        }
        public override void OnStateExit(FSMBase fsm) {
            base.OnStateExit(fsm);
            var gestureFSM = fsm as GestureFSM;
            gestureFSM.rotateTransformer.EndTransform();
        }
    }
}