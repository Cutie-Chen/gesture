namespace AI.FSM {
    public class GrabPointAwayCorner : GrabPointChecker {
        public override bool HandleTrigger(FSMBase fsm) {
            var gestureFSM = fsm as GestureFSM;
            if (gestureFSM.grabbedItem.GrabPoints.Count != 1)
                return false;
            return !isGrabPointNearCorner(fsm);
        }

        protected override void init() {
            this.TriggerID = FSMTriggerID.GrabPointAwayCorner;
        }
    }
}