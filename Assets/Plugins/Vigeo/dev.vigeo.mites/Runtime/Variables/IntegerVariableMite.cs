using System;
using Sirenix.OdinInspector;
using UnityEngine;
using static Vigeo.Mites.ComparisonMode.Relation;

namespace Vigeo.Mites {
    
    [Serializable, HideReferenceObjectPicker]
    public class IntegerVariableMiteCondition : Condition {

        [HideLabel, HorizontalGroup("")]
        public IntegerVariableMite variable;
        
        [HideLabel, HorizontalGroup("", Width = 40)]
        public ComparisonMode.Relation comparisonMode;

        [HideLabel, HorizontalGroup("", Width = 62)]
        public int targetValue;

        public override bool IsMet() => variable.RunWithValue(value => comparisonMode switch {
            StrictlyLowerThan => value < targetValue,
            LowerThan => value <= targetValue,
            Equal => value == targetValue,
            GreaterThan => value >= targetValue,
            StrictlyGreaterThan => value > targetValue,
            NotEqual => value != targetValue,
            _ => false
        });
    }

    /* [EditorIcon("atom-icon-lush")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Variables/Integer")]
    [HideMonoScript]
    public class IntegerVariableMite : VariableMite<int> {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Holds a simple integer value."
        );
    }
}
