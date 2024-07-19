using System;
using System.Collections;
using UnityEngine;

namespace Vigeo {

    /// <summary>
    /// Logs current frame number if activation key is currently pressed. Will
    /// wait until end of frame in LateUpdate() before logging.
    /// </summary>
    public class DebugFrameCounter : MonoBehaviour {

        [SerializeField]
        private KeyCode activationKey = KeyCode.Equals;

        private bool loggingOn;

        private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private void Update() {
            loggingOn = Input.GetKey(activationKey);
        }

        private void LateUpdate() {
            StartCoroutine(LogFrameNumber());
        }

        private IEnumerator LogFrameNumber() {
            yield return waitForEndOfFrame;
            if (loggingOn) {
                Debug.Log($"[DebugFrameCounter] FRAME: {Time.frameCount}");
            }
        }
    }
}
