using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Vigeo.Mites {

#if UNITY_EDITOR
    
    // This part is responsible for showing a list of actions referencing this variable.
    public partial class VariableMite {

        public static List<ActionMite> actions = new();

        private void RefreshActions() {
            actions.Clear();
            string[] actionAssetGUIDs = AssetDatabase.FindAssets($"t:{typeof(ActionMite)}", new[] { "Assets" });
            foreach (string actionAssetGUID in actionAssetGUIDs) {
                string actionAssetPath = AssetDatabase.GUIDToAssetPath(actionAssetGUID);
                var action = AssetDatabase.LoadAssetAtPath<ActionMite>(actionAssetPath);
                actions.Add(action);
            }
        }

        private bool IsActionReferencingVariable(ActionMite action, VariableMite variable) => action switch {
            SetBooleanVariableValueAction setBooleanVariableValueAction => setBooleanVariableValueAction.variable == variable,
            _ => false
        };
        
        [VerticalGroup("", GroupID = "DEV")]
        [FoldoutGroup("EDITOR: Variable references", GroupID = "DEV/O"), TitleGroup("DEV/O/Actions referencing this variable")]
        [OnInspectorGUI]
        private void OnInspectorGUI() {
            if (GUILayout.Button("Refresh actions")) {
                RefreshActions();
            }
            GUI.enabled = false;
            foreach (var action in actions) {
                if (IsActionReferencingVariable(action, this)) {
                    EditorGUILayout.ObjectField(action, typeof(ActionMite), false);
                }
            }
            GUI.enabled = true;
        }
    }
    
#endif
    
    public abstract partial class VariableMite : ScriptableMite {

        private string id;

        public string Id {
            get => id;
            set => id = value;
        }
    }
    
#if UNITY_EDITOR
    
    // This part is responsible for displaying current value no matter if editor mode or play mode.
    public partial class VariableMite<T> {
        
        [TitleGroup("DEV/O/Value debugging")]
        [ShowInInspector, LabelWidth(82)]
        protected virtual T CurrentValue => Value;
    }
    
#endif    
    
#if UNITY_EDITOR
    
    // This part is responsible for resetting all instances when Unity Editor exits play mode.
    public partial class VariableMite<T> {

        private static readonly HashSet<VariableMite<T>> instances = new();
        
        private static void HandlePlayModeStateChange(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingPlayMode || state == PlayModeStateChange.EnteredEditMode) {
                foreach (var instance in instances) {
                    instance.ResetValue();
                }
            }
        }
    }
    
#endif

    /* [EditorIcon("atom-icon-lush")] */
    public abstract partial class VariableMite<T> : VariableMite, IEquatable<VariableMite<T>> where T : new() {
        
        [SerializeField, HideInInspector]
        private T initialValue;
        
        [ShowInInspector, LabelText("Value"), LabelWidth(37), PropertySpace, HideInPlayMode]
        public virtual T InitialValue { get => initialValue; set => initialValue = value; }

        [SerializeField, HideInInspector]
        protected T value;

        [ShowInInspector, LabelText("Value"), LabelWidth(37), PropertySpace, HideInEditorMode]
        public virtual T Value {
            get => value;
            set => this.value = value;
        }

        protected virtual void ResetValue() {
            value = initialValue;
            //Debug.Log($"[ResetValue] initial: {initialValue}, value: {value} (name: {name})"); // uncomment in case of issues with resetting mite value
        }

        protected void OnEnable() {
            ResetValue();
            
#if UNITY_EDITOR
            
            if (EditorSettings.enterPlayModeOptionsEnabled) {
                instances.Add(this);
                EditorApplication.playModeStateChanged -= HandlePlayModeStateChange;
                EditorApplication.playModeStateChanged += HandlePlayModeStateChange;
            }
            
#endif
            
        }
        
        private void OnDisable() {
            // NOTE: This will not be called when deleting the Atom from the editor.
            // Therefore, there might still be null instances, but even though not ideal,
            // it should not cause any problems.
            // More info: https://issuetracker.unity3d.com/issues/ondisable-and-ondestroy-methods-are-not-called-when-a-scriptableobject-is-deleted-manually-in-project-window
            
#if UNITY_EDITOR
            
            instances.Remove(this);
            
#endif
            
        }

        public bool Equals(VariableMite<T> other) {
            return other == this;
        }
    }
}
