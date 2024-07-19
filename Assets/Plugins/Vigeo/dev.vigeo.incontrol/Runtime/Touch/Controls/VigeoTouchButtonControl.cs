using System;
using InControl;
using UnityEngine;

namespace Vigeo.InControl {
    
    public class VigeoTouchButtonControl : TouchButtonControl, VigeoTouchControl {
        
        [Serializable]
        public struct Configuration {

            public TouchControlAnchor anchor;

            public TouchUnitType offsetUnitType;

            public Vector2 offset;

            public Vector2 buttonSize;
        }

        [SerializeField, HideInInspector]
        private Configuration configuration;

        private bool isVisible = true;

        private void UpdateVisibilityState() {
            isVisible = button.Size != Vector2.zero;
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
            button.Size = configuration.buttonSize;
            UpdateVisibilityState();
        }

        public void Hide() {
            if (isVisible == false) {
                return;
            }
            configuration.anchor = Anchor;
            configuration.offsetUnitType = OffsetUnitType;
            configuration.offset = Offset;
            configuration.buttonSize = button.Size;
            // Zero-out all control sizes to mitigate issue with hidden touch controls that still hijack input (i.e. virtual joystick
            // and up/down buttons still influence primary movement even after they are hidden).
            button.Size = Vector2.zero;
            UpdateVisibilityState();
        }
        
        private void OnValidate() {
            configuration.anchor = Anchor;
            configuration.offsetUnitType = OffsetUnitType;
            configuration.offset = Offset;
            configuration.buttonSize = button.Size;
            UpdateVisibilityState();
        }
    }
}
