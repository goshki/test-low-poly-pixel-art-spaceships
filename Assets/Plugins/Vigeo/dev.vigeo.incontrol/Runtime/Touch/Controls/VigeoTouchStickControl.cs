using System;
using InControl;
using UnityEngine;

namespace Vigeo.InControl {
    
    public class VigeoTouchStickControl : TouchStickControl, VigeoTouchControl {
        
        [Serializable]
        public struct Configuration {

            public TouchControlAnchor anchor;

            public TouchUnitType offsetUnitType;

            public Vector2 offset;

            public Rect activeArea;
            
            public Vector2 knobSize;
            
            public Vector2 ringSize;
        }

        [SerializeField, HideInInspector]
        private Configuration configuration;
        
        private bool isVisible = true;
        
        /*
         * If [pressTarget] is set to some action, this touch stick will invoke this action also on press (i.e. a shooting stick can
         * also invoke [Fire] action when pressed without need to move the stick – this can be useful if there's auto-aiming and the
         * stick overrides shooting direction when moved but also just shoots at auto-aimed target when pressed).
         */
        [Header( "Extra Options" )]
        public ButtonTarget pressTarget = ButtonTarget.None;

        public override void SubmitControlState(ulong updateTick, float deltaTime) {
            base.SubmitControlState(updateTick, deltaTime);
            if (pressTarget != ButtonTarget.None) {
                SubmitButtonState(pressTarget, IsActive, updateTick, deltaTime );
            }
        }
        
        public override void CommitControlState(ulong updateTick, float deltaTime ) {
            if (pressTarget != ButtonTarget.None) {
                CommitButton(pressTarget);
            }
        }
        
        private void UpdateVisibilityState() {
            isVisible = ActiveArea != Rect.zero;
        }

        private void Awake() {
            UpdateVisibilityState();
        }

        public void Show() {
            if (isVisible) {
                return;
            }
            Anchor = configuration.anchor;
            OffsetUnitType = configuration.offsetUnitType;
            Offset = configuration.offset;
            ActiveArea = configuration.activeArea;
            knob.Size = configuration.knobSize;
            ring.Size = configuration.ringSize;
            UpdateVisibilityState();
        }

        public void Hide() {
            if (isVisible == false) {
                return;
            }
            configuration.anchor = Anchor;
            configuration.offsetUnitType = OffsetUnitType;
            configuration.offset = Offset;
            configuration.activeArea = ActiveArea;
            configuration.knobSize = knob.Size;
            configuration.ringSize = ring.Size;
            // Zero-out all control sizes to mitigate issue with hidden touch controls that still hijack input (i.e. virtual joystick
            // and up/down buttons still influence primary movement even after they are hidden).
            ActiveArea = Rect.zero;
            knob.Size = Vector2.zero;
            ring.Size = Vector2.zero;
            UpdateVisibilityState();
        }
        
        private void OnValidate() {
            configuration.anchor = Anchor;
            configuration.offsetUnitType = OffsetUnitType;
            configuration.offset = Offset;
            configuration.activeArea = ActiveArea;
            configuration.knobSize = knob.Size;
            configuration.ringSize = ring.Size;
            UpdateVisibilityState();
        }
    }
}
