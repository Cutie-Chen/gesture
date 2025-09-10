using Oculus.Interaction;

namespace AI.FSM {
    public class SelectState : FSMState {
        protected override void init() {
            this.StateID = FSMStateID.Select;
        }
       
        public override void OnStateStay(FSMBase fsm) {
            base.OnStateStay(fsm);
            var gestureFSM = fsm as GestureFSM;
            // todo: 将选中的物体保存到gestureFSM
            foreach(var view in gestureFSM.interactableGroupViews)
            {
                if(view.State== InteractableState.Select)
                {
                    gestureFSM.grabbedItem = view.GetComponent<Grabbable>();
                    break;
                }
            }
           
        }
        public override void OnStateExit(FSMBase fsm) {
            base.OnStateExit(fsm);
            var gestureFSM = fsm as GestureFSM;
            gestureFSM.grabbedItem = null;
        }
    }
}