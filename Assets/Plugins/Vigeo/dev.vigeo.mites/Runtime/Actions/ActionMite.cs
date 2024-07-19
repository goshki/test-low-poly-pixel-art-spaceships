using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Vigeo.Mites {

#if UNITY_EDITOR
    
    // This part is responsible for resetting all instances when Unity Editor exits play mode.
    public partial class ActionMite {

        private static readonly HashSet<ActionMite> instances = new();
        
        private static void HandlePlayModeStateChange(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingPlayMode || state == PlayModeStateChange.EnteredEditMode) {
                foreach (var instance in instances) {
                    instance.triggerOptions.Triggered = false;
                }
            }
        }
        
        private void OnEnable() {
            if (EditorSettings.enterPlayModeOptionsEnabled) {
                instances.Add(this);
                EditorApplication.playModeStateChanged -= HandlePlayModeStateChange;
                EditorApplication.playModeStateChanged += HandlePlayModeStateChange;
            }
        }
    }
    
    // This part is responsible for showing a list of reactions referencing this action.
    public abstract partial class ActionMite {

        private static List<EventReactionMite> reactions = new();
        
        private void RefreshReactions() {
            reactions.Clear();
            string[] reactionAssetGUIDs = AssetDatabase.FindAssets($"t:{typeof(EventReactionMite)}", new[] { "Assets" });
            foreach (string reactionAssetGUID in reactionAssetGUIDs) {
                string reactionAssetPath = AssetDatabase.GUIDToAssetPath(reactionAssetGUID);
                var reaction = AssetDatabase.LoadAssetAtPath<EventReactionMite>(reactionAssetPath);
                reactions.Add(reaction);
            }
        }

        [VerticalGroup("", GroupID = "DEV")]
        [FoldoutGroup("EDITOR: Action references", GroupID = "DEV/O"), TitleGroup("DEV/O/Reactions referencing this action")]
        [OnInspectorGUI]
        private void OnInspectorGUI() {
            if (GUILayout.Button("Refresh reactions")) {
                RefreshReactions();
            }
            GUI.enabled = false;
            foreach (var reaction in reactions) {
                if (reaction.actions.Contains(this)) {
                    EditorGUILayout.ObjectField(reaction, typeof(EventReactionMite), false);
                }
            }
            GUI.enabled = true;
        }
    }
    
#endif
    
    /* [EditorIcon("atom-icon-purple")] */
    public abstract partial class ActionMite : ScriptableMite {
        
        [SerializeField, HideLabel]
        protected TriggerOptions triggerOptions;

        protected virtual void PerformAction() {}

        public virtual void Trigger() =>
            (triggerOptions.CanBeTriggered && triggerOptions.ConditionsMatch).WhenTrue(() => {
                PerformAction();
                triggerOptions.Triggered = true;
            });

    }

    /* [EditorIcon("atom-icon-purple")] */
    public abstract class ActionMite<T> : ActionMite {
    
        protected virtual void PerformAction(T parameter) => base.PerformAction();
        
        public virtual void Trigger(T parameter) =>
            (triggerOptions.CanBeTriggered && triggerOptions.ConditionsMatch).WhenTrue(() => {
                PerformAction(parameter);
                triggerOptions.Triggered = true;
            });
    }
}
