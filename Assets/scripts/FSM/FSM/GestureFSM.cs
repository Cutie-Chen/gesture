using Oculus.Interaction;
using TMPro;
using UnityEngine;


namespace AI.FSM {
    public class GestureFSM : FSMBase {
        [HideInInspector]
        public Grabbable grabbedItem;
        public OneGrabRotateTransformer rotateTransformer;
        public OneGrabTranslateTransformer translateTransformer;
        public InteractableGroupView[] interactableGroupViews;
        public TextMeshProUGUI text;
        protected override void SetUpFSM() {
            base.SetUpFSM();

            DefaultState defaultState = new DefaultState();
            defaultState.AddMap(FSMTriggerID.ItemGrabbed, FSMStateID.Select);
            _states.Add(defaultState);

            SelectState selectState = new SelectState();
            selectState.AddMap(FSMTriggerID.GrabPointNearCorner, FSMStateID.Rotate);
            selectState.AddMap(FSMTriggerID.GrabPointAwayCorner, FSMStateID.Translate);
            selectState.AddMap(FSMTriggerID.OutOfItem, FSMStateID.Default);
            selectState.AddMap(FSMTriggerID.NoPose, FSMStateID.Default);
            _states.Add(selectState);

            TranslateState translateState = new TranslateState();
            translateState.AddMap(FSMTriggerID.NoPose, FSMStateID.Default);
            _states.Add(translateState);

            RotateState rotateState = new RotateState();
            rotateState.AddMap(FSMTriggerID.NoPose, FSMStateID.Default);
            _states.Add(rotateState);
        }
    }
}