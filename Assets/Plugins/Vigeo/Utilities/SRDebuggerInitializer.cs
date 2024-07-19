using Sirenix.OdinInspector;
using SRDebugger;
using UnityEngine;

namespace Vigeo {

    [HideMonoScript]
    public class SRDebuggerInitializer : MonoBehaviour {
        
        public enum ActivationMode {
            Inactive,
            ActivateOnMobile,
            ActivateAlways
        }

        [LabelWidth(95)]
        public ActivationMode activationMode;

        private bool ShouldActivate() {
            if (activationMode == ActivationMode.ActivateOnMobile) {
                return Application.platform == RuntimePlatform.Android;
            }
            return activationMode == ActivationMode.ActivateAlways;
        }

        private void OnEnable() {
            if (ShouldActivate()) {
#if UNITY_EDITOR
                Settings.Instance.UIScale = 0.5f;
#else
                Settings.Instance.UIScale = 1f;
#endif
                if (Application.isMobilePlatform) {
                    Settings.Instance.UIScale = 0.5f;
                }
                SRDebug.Init();
            }
            Destroy(this);
        }
    }
}
