using System.Collections;
using UnityEngine;

namespace Vigeo.Test.Test20240417 {

    public class DebugDisplayInfo : MonoBehaviour {
        
        private uint logSize = 15;  // number of messages to keep
        
        private Queue myLogQueue = new();

        private string playModeWindowResolution = "";

        private void Start() {
            var scale = Screen.height * 160 / Screen.dpi;
            //Debug.Log($"DPI: {Screen.dpi}, RESOLUTION: {Screen.currentResolution.width}x{Screen.currentResolution.height}, SCALE: {scale}");
        }

        private void OnEnable() {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable() {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type) {
            myLogQueue.Enqueue("[" + type + "] : " + logString);
            if (type == LogType.Exception)
                myLogQueue.Enqueue(stackTrace);
            while (myLogQueue.Count > logSize)
                myLogQueue.Dequeue();
        }

        private void OnGUI() {
            GUI.contentColor = Color.black;
            GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
            GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
            GUILayout.EndArea();
        }

        private void Update() {
#if UNITY_EDITOR
            UnityEditor.PlayModeWindow.GetRenderingResolution(out uint width, out uint height);
            var currentPlayModeWindowResolution = $"{width}x{height}";
            if (currentPlayModeWindowResolution != playModeWindowResolution) {
                playModeWindowResolution = currentPlayModeWindowResolution;
                //Debug.Log(playModeWindowResolution);
            }
#endif
        }
    }
}
