using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

namespace Vigeo {

    [RequireComponent(typeof(Camera))]
    public class PixelPerfectCameraConfigurator : MonoBehaviour {
        
        private Resolution currentResolution;

        private Resolution PixelatedResolution => new() {
            width = pixelPerfectCamera ? currentResolution.width / pixelPerfectCamera.pixelRatio : currentResolution.width,
            height = pixelPerfectCamera ? currentResolution.height / pixelPerfectCamera.pixelRatio : currentResolution.height,
            refreshRateRatio = currentResolution.refreshRateRatio
        };

        private int CurrentPixelSize => pixelPerfectCamera ? pixelPerfectCamera.pixelRatio : 1;
        
        public enum TargetResolution {
            [LabelText("120")]
            _120 = 120,
            [LabelText("144")]
            _144 = 144,
            [LabelText("160")]
            _160 = 160,
            [LabelText("180")]
            _180 = 180,
            [LabelText("200")]
            _200 = 200,
            [LabelText("240")]
            _240 = 240,
            [LabelText("256")]
            _256 = 256,
            [LabelText("400")]
            _400 = 400,
            [LabelText("480")]
            _480 = 480,
            [LabelText("512")]
            _512 = 512
        }
        
        public enum TargetFramerate {
            [LabelText("12")]
            _12 = 12,
            [LabelText("24")]
            _24 = 24,
            [LabelText("30")]
            _30 = 30,
            [LabelText("60")]
            _60 = 60
        }

        [Serializable]
        public class Options : INotifyPropertyChanged {

            [SerializeField, HideInPlayMode]
            private TargetResolution targetResolution = TargetResolution._200;

            [Category("Graphics")]
            [DisplayName("Resolution")]
            public TargetResolution TargetResolution {
                get => targetResolution;
                set {
                    targetResolution = value;
                    OnPropertyChanged();
                }
            }

            [SerializeField, HideInPlayMode]
            private TargetFramerate targetFrameRate = TargetFramerate._60;

            [Category("Graphics")]
            [DisplayName("Framerate")]
            public TargetFramerate TargetFrameRate {
                get => targetFrameRate;
                set {
                    if (value != targetFrameRate) {
                        targetFrameRate = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = "") {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Camera pixelCamera;

        private PixelPerfectCamera pixelPerfectCamera;

        [InlineProperty, HideLabel]
        public Options options;

        private void Awake() {
            enabled = TryGetComponent(out pixelCamera);
            MaybeUpdateCurrentResolution();
        }
        
        private void Update() {
            MaybeUpdateCurrentResolution();
        }

        private void InitializePixelPerfectCamera() {
            if (TryGetComponent(out pixelPerfectCamera)) {
                DestroyImmediate(pixelPerfectCamera);
            }
            pixelPerfectCamera = gameObject.AddComponent<PixelPerfectCamera>();
            pixelPerfectCamera.assetsPPU = 1024;
            pixelPerfectCamera.upscaleRT = true;
            UpdateResolution(options.TargetResolution);
        }
        
        private void MaybeUpdateCurrentResolution() {
            var currentScreenResolution = Screen.currentResolution;
#if UNITY_EDITOR
            UnityEditor.PlayModeWindow.GetRenderingResolution(out uint editorGameWindowWidth, out uint editorGameWindowHeight);
            if (editorGameWindowWidth != currentScreenResolution.width || editorGameWindowHeight != currentScreenResolution.height) {
                currentScreenResolution = new Resolution() {
                    width = (int) editorGameWindowWidth,
                    height = (int) editorGameWindowHeight,
                    refreshRateRatio = Screen.currentResolution.refreshRateRatio
                };
            }
#endif
            if (currentResolution.width != currentScreenResolution.width || currentResolution.height != currentScreenResolution.height) {
                currentResolution = currentScreenResolution;
                //Debug.Log($"Current resolution: {currentResolution} (pixelated: {PixelatedResolution}, pixelSize: {CurrentPixelSize})");
            }
        }

        private void UpdateResolution(TargetResolution targetResolution) => ((int) targetResolution).Run(resolution => {
            pixelPerfectCamera.refResolutionX = resolution;
            pixelPerfectCamera.refResolutionY = resolution;
        });

        private void UpdateFrameRate(TargetFramerate targetFrameRate) => ((int) targetFrameRate).Run(frameRate => {
            if (Application.targetFrameRate != frameRate) {
                Application.targetFrameRate = frameRate;
            }
        });

        private void HandleOptionsPropertyChanged(object sender, PropertyChangedEventArgs eventArgs) => _ = eventArgs.PropertyName switch {
            nameof(Options.TargetResolution) => true.Also(() => UpdateResolution(options.TargetResolution)),
            nameof(Options.TargetFrameRate) => true.Also(() => UpdateFrameRate(options.TargetFrameRate)),
            _ => false
        };

        private IEnumerator Start() {
            InitializePixelPerfectCamera();
            // Delaying initial framerate update to ensure it works on mobile
            yield return new WaitForEndOfFrame();
            UpdateFrameRate(options.TargetFrameRate);
        }

        private void Initialize() {
            options.PropertyChanged += HandleOptionsPropertyChanged;
            SRDebug.Instance?.AddOptionContainer(options);
        }

        private void Uninitialize() {
            options.PropertyChanged -= HandleOptionsPropertyChanged;
            SRDebug.Instance?.RemoveOptionContainer(options);
        }

        private void OnEnable() {
            Initialize();
        }

        private void OnDisable() {
            Uninitialize();
        }
    }
}
