using UnityEngine;
using UnityEventSystem = UnityEngine.EventSystems.EventSystem;

namespace Vigeo {

    public class UpdateDragThresholdForDPI : MonoBehaviour {

        private void Start() {
            int defaultDragThreshold = UnityEventSystem.current.pixelDragThreshold;
            UnityEventSystem.current.pixelDragThreshold = defaultDragThreshold.InDp();
        }
    }
}
