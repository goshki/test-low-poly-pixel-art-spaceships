using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo.Mites {

    /* [EditorIcon("atom-icon-purple")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Actions/Increment Timer Variable By Delta-Time")]
    [HideMonoScript]
    public class IncrementTimerVariableByDeltaTimeActionMite : ActionMite {
        
        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Increments value of a timer variable by delta-time."
        );
        
        [HideLabel]
        [TitleGroup("Specific action setup", GroupID = "A")]
        [HorizontalGroup("", GroupID = "A/Horizontal", Width = 80)]
        public float value;
        
        [LabelText("⇒"), LabelWidth(15)]
        [HorizontalGroup("", GroupID = "A/Horizontal")]
        public TimerVariableMite variable;

        protected override void PerformAction() =>
            variable.Apply(variable => variable.Value += Time.deltaTime);
    }
}
