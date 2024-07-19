using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo.Mites {

    /* [EditorIcon("atom-icon-purple")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Actions/Set Float Variable Value")]
    [HideMonoScript]
    public class SetFloatVariableValueActionMite : ActionMite {
        
        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Sets value of a float variable."
        );
        
        [HideLabel]
        [TitleGroup("Specific action setup", GroupID = "A")]
        [HorizontalGroup("", GroupID = "A/Horizontal", Width = 80)]
        public float value;
        
        [LabelText("⇒"), LabelWidth(15)]
        [HorizontalGroup("", GroupID = "A/Horizontal")]
        public FloatVariableMite variable;

        protected override void PerformAction() =>
            variable.Apply(variable => variable.Value = value);
    }
}
