using Oculus.Interaction;
using System.Linq;

namespace AI.FSM {
    public class ItemGrabbedTrigger : FSMTrigger {
        public override bool HandleTrigger(FSMBase fsm) {
            // 检查InteractableGroupView的State属性是否是已选中
            //       如果后期有多个可操作的物体，也就是有多个InteractableView，那么这里的条件是其中至少一个已选中
            var gestureFSM = fsm as GestureFSM;
            return gestureFSM.interactableGroupViews.Any(view => view.State == InteractableState.Select);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.ItemGrabbed;
        }
    }
}