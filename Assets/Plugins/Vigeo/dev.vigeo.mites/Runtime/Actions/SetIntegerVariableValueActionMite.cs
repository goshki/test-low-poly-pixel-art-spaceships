using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo.Mites {

    /* [EditorIcon("atom-icon-purple")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Actions/Set Integer Variable Value")]
    [HideMonoScript]
    public class SetIntegerVariableValueActionMite : ActionMite {
        
        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Sets value of an integer variable."
        );
        
        [HideLabel]
        [TitleGroup("Specific action setup", GroupID = "A")]
        [HorizontalGroup("", GroupID = "A/Horizontal", Width = 80)]
        public int value;
        
        [LabelText("⇒"), LabelWidth(15)]
        [HorizontalGroup("", GroupID = "A/Horizontal")]
        public IntegerVariableMite variable;

        protected override void PerformAction() =>
            variable.Apply(variable => variable.Value = value);
    }
}
