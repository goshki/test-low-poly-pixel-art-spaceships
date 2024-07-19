using System;
using Sirenix.OdinInspector;
using UnityEngine;

using static Vigeo.Mites.ComparisonMode.Boolean;

namespace Vigeo.Mites {
    
    [Serializable, HideReferenceObjectPicker]
    public class BooleanVariableMiteCondition : Condition {

        [HideLabel, HorizontalGroup("")]
        public BooleanVariableMite variable;
        
        [HideLabel, HorizontalGroup("", Width = 40)]
        public ComparisonMode.Boolean comparisonMode;
        
        [HideLabel, HorizontalGroup("", Width = 62)]
        public BooleanValue expectedValue;

        public override bool IsMet() => variable.RunWithValue(value => comparisonMode switch {
            Equal => value == expectedValue.ToBoolean(),
            NotEqual => value != expectedValue.ToBoolean(),
            _ => false
        });
    }

    /* [EditorIcon("atom-icon-lush")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Variables/Boolean")]
    [HideMonoScript]
    public class BooleanVariableMite : VariableMite<bool> {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Holds a simple TRUE / FALSE boolean value."
        );
    }
}
