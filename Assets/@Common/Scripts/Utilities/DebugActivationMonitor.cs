using System.Reflection;
using UnityEngine;

namespace Vigeo {

    public class DebugActivationMonitor : MonoBehaviour {

        private void OnEnable() {
            Debug.Log($"[{GetType().Name}][{MethodBase.GetCurrentMethod().Name}]");
        }

        private void OnDisable() {
            Debug.Log($"[{GetType().Name}][{MethodBase.GetCurrentMethod().Name}]");
        }
    }
}
