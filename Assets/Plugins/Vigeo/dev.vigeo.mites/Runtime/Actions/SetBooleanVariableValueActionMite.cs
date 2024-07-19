using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo.Mites {

    /* [EditorIcon("atom-icon-purple")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Actions/Set Boolean Variable Value")]
    [HideMonoScript]
    public class SetBooleanVariableValueAction : ActionMite {
        
        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Sets value of a boolean variable."
        );
        
        [HideLabel]
        [TitleGroup("Specific action setup", GroupID = "A")]
        [HorizontalGroup("", GroupID = "A/Horizontal", Width = 80)]
        public BooleanValue value;
        
        [LabelText("⇒"), LabelWidth(15)]
        [HorizontalGroup("", GroupID = "A/Horizontal")]
        public BooleanVariableMite variable;

        protected override void PerformAction() =>
            variable.Apply(variable => variable.Value = value.ToBoolean());
    }
}
