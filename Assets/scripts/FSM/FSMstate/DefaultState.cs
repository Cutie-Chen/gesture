namespace AI.FSM {
    public class DefaultState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Default;
        }
    }
}