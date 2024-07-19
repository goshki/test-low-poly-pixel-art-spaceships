using UnityEngine;

namespace Vigeo.Test.Test20240417 {

    public class DebugOscillateColorAlpha : MonoBehaviour {

        public float amplitude = 0.5f;

        public float minAlpha = 0.5f;
        
        public float frequency = 1;

        private void Start() {
            GetComponent<Renderer>().sharedMaterial = new Material(GetComponent<Renderer>().sharedMaterial);
        }

        private void Update () {
            Material mat = GetComponent<Renderer>().sharedMaterial;
            Color col = mat.color;
            col.a = Mathf.Sin(Time.timeSinceLevelLoad * frequency) * amplitude + minAlpha;
            mat.color = col;
        }
    }
}
