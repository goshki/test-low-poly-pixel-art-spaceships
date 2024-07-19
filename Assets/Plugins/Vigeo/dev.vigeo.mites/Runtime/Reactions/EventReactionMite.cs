using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vigeo.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Vigeo.Mites {
    
#if UNITY_EDITOR
    
    // This part is responsible for resetting all instances when Unity Editor exits play mode.
    public partial class EventReactionMite {

        private static readonly HashSet<EventReactionMite> instances = new();
        
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
    
#endif

    /* [EditorIcon("atom-icon-sign-blue")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Event Reactions/Basic Event Reaction")]
    [HideMonoScript]
    public partial class EventReactionMite : ScriptableMite {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Performs a set of actions when all conditions are met."
        );
        
        [SerializeField, HideLabel]
        protected TriggerOptions triggerOptions;

        [Title("Specific reaction setup"), LabelText("Actions to perform"), Space, PropertyOrder(100)]
        public List<ActionMite> actions;

        public virtual bool Accepts() => triggerOptions.ConditionsMatch;

        public virtual bool Accepts(EventData eventData) =>
            triggerOptions.CanBeTriggered && Accepts();

        public virtual void Trigger() {
            actions.ForEach(action => action.Trigger());
            triggerOptions.Triggered = true;
        }
    }
    
    /* [EditorIcon("atom-icon-sign-blue")] */
    public abstract class EventReactionMite<T> : EventReactionMite where T : EventData {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Performs a set of actions when all conditions are met, including conditions matching specific event data " +
            "(marked with [➲] prefix).",
            $"This reaction type can perform advanced matching of data provided by <b>[{typeof(T).Name}]</b>."
        );
        
        protected List<Condition<T>> eventDataMatchingConditions =>
            triggerOptions.conditions.FilterByType<Condition<T>>();

        // Never accepts by default (accepts only if event data are accepted)
        public override bool Accepts() => false;

        // Accepts event data if any of type-specific event data conditions match
        protected virtual bool Accepts(T eventData) =>
            eventDataMatchingConditions.Any(condition => condition.IsMet(eventData));

        private bool AcceptsEventData(EventData eventData) => eventData switch {
            T data => Accepts(data),
            _ => false
        };

        public override bool Accepts(EventData eventData) =>
            triggerOptions.CanBeTriggered && triggerOptions.ConditionsMatch && AcceptsEventData(eventData);
    }
}
