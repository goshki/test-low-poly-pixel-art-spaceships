using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using SRDebugger;
using UnityEngine;

namespace Vigeo {

    [HideMonoScript]
    public class SRDebuggerCommonOptions : MonoBehaviour {

        [Serializable]
        public class Options : INotifyPropertyChanged {

            private struct TimeScaleOption {

                public string label;

                public float value;
            }
            
            private static readonly TimeScaleOption[] TIMESCALE_OPTIONS = new[] {
                new TimeScaleOption { label = "          x0", value = 0 }, // extra spaces for pretty-printing in SRDebugger
                new TimeScaleOption { label = "        x1/25", value = 1.0f / 25.0f },
                new TimeScaleOption { label = "        x1/10", value = 1.0f / 10.0f },
                new TimeScaleOption { label = "        x1/5", value = 1.0f / 5.0f },
                new TimeScaleOption { label = "        x1/2", value = 1.0f / 2.0f },
                new TimeScaleOption { label = "          x1", value = 1.0f },
                new TimeScaleOption { label = "          x2", value = 2.0f },
                new TimeScaleOption { label = "          x5", value = 5.0f }
            };

            private static int DEFAULT_OPTION_INDEX = 5;

            private int currentOptionIndex = DEFAULT_OPTION_INDEX;
            
            [Category("Common / Time"), DisplayName("Time scale")]
            [ShowInInspector, PropertyOrder(1), LabelWidth(85)]
            public string TimeScale {
                get => TIMESCALE_OPTIONS[currentOptionIndex].label;
            }
            
            [Category("Common / Time"), DisplayName("<<"), Sort(-2)]
            [Button("<<"), HorizontalGroup(), HideInEditorMode]
            public void DecreaseTimeScale() {
                currentOptionIndex = Mathf.Clamp(--currentOptionIndex, 0, TIMESCALE_OPTIONS.Length - 1);
                Time.timeScale = TIMESCALE_OPTIONS[currentOptionIndex].value;
                OnPropertyChanged(nameof(TimeScale));
            }
            
            [Category("Common / Time"), DisplayName("||"), Sort(-1)]
            [Button("||"), HorizontalGroup(), HideInEditorMode]
            public void PauseTimeScale() {
                currentOptionIndex = 0;
                Time.timeScale = TIMESCALE_OPTIONS[currentOptionIndex].value;
                OnPropertyChanged(nameof(TimeScale));
            }
            
            [Category("Common / Time"), DisplayName(">"), Sort(1)]
            [Button(">"), HorizontalGroup(), HideInEditorMode]
            public void ResetTimeScale() {
                currentOptionIndex = DEFAULT_OPTION_INDEX;
                Time.timeScale = TIMESCALE_OPTIONS[currentOptionIndex].value;
                OnPropertyChanged(nameof(TimeScale));
            }
            
            [Category("Common / Time"), DisplayName(">>"), Sort(2)]
            [Button(">>"), HorizontalGroup(), HideInEditorMode]
            public void IncreaseTimeScale() {
                currentOptionIndex = Mathf.Clamp(++currentOptionIndex, 0, TIMESCALE_OPTIONS.Length - 1);
                Time.timeScale = TIMESCALE_OPTIONS[currentOptionIndex].value;
                OnPropertyChanged(nameof(TimeScale));
            }

            public event PropertyChangedEventHandler PropertyChanged;
            
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        [InlineProperty, HideLabel, PropertyOrder(-1000)]
        public Options options;

        private void HandleInstanceOptionsPropertyChanged(object sender, PropertyChangedEventArgs eventArgs) {}

        private void Initialize() {
            options.PropertyChanged += HandleInstanceOptionsPropertyChanged;
            SRDebug.Instance?.AddOptionContainer(options);
        }

        private void Uninitialize() {
            options.PropertyChanged -= HandleInstanceOptionsPropertyChanged;
            SRDebug.Instance?.RemoveOptionContainer(options);
        }

        private void OnEnable() {
            Initialize();
        }

        private void OnDisable() {
            Uninitialize();
        }
        
#if UNITY_EDITOR

        private void Update() {
            if (Input.GetKeyDown(KeyCode.LeftBracket)) {
                options.DecreaseTimeScale();
            }
            if (Input.GetKeyDown(KeyCode.RightBracket)) {
                options.IncreaseTimeScale();
            }
        }
        
#endif
        
    }
}
