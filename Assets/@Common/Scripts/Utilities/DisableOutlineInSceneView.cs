using UnityEngine;
using UnityEngine.Rendering;

namespace Vigeo {

    [ExecuteInEditMode]
    public class DisableOutlineInSceneView : MonoBehaviour {

        private GlobalKeyword useOutline;

        private bool isKeywordEnabled;

        private void Awake() {
            useOutline = GlobalKeyword.Create("USE_OUTLINE");
            Shader.EnableKeyword(useOutline);
        }

        public void OnEnable() {
            Camera.onPreRender += UpdateUseOutlineKeywordState;
        }

        public void OnDisable() {
            Camera.onPreRender -= UpdateUseOutlineKeywordState;
        }

        public void UpdateUseOutlineKeywordState(Camera camera) {
            if (camera.gameObject.name == "SceneCamera") {
                if (Shader.IsKeywordEnabled(useOutline)) {
                    Shader.DisableKeyword(useOutline);
                }
            } else {
                Shader.EnableKeyword(useOutline);
            }
        }
    }
}
