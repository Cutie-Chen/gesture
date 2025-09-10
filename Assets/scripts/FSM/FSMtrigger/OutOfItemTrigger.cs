using System.Linq;

namespace AI.FSM {
    public class OutOfItemTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            //  检查InteractableGroupView的SelectingInteractorsCount是否为零
            //       如果后期有多个可操作的物体，也就是有多个InteractableView，那么这里的条件是总和为0
            var gestureFSM = fsm as GestureFSM;
            return gestureFSM.interactableGroupViews.All(view => view.SelectingInteractorsCount == 0);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.OutOfItem;
        }
    }
}