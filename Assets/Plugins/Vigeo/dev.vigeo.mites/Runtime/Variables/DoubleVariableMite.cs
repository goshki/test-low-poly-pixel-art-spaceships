using System;
using Sirenix.OdinInspector;
using UnityEngine;
using static Vigeo.Mites.ComparisonMode.Relation;

namespace Vigeo.Mites {
    
    [Serializable, HideReferenceObjectPicker]
    public class DoubleVariableMiteCondition : Condition {

        [HideLabel, HorizontalGroup("")]
        public DoubleVariableMite variable;
        
        [HideLabel, HorizontalGroup("", Width = 40)]
        public ComparisonMode.Relation comparisonMode;

        [HideLabel, HorizontalGroup("", Width = 62)]
        public double targetValue;

        public override bool IsMet() => variable.RunWithValue(value => comparisonMode switch {
            StrictlyLowerThan => value < targetValue,
            LowerThan => value <= targetValue,
            Equal => Math.Abs(value - targetValue) < double.Epsilon,
            GreaterThan => value >= targetValue,
            StrictlyGreaterThan => value > targetValue,
            NotEqual => Math.Abs(value - targetValue) >= double.Epsilon,
            _ => false
        });
    }

    /* [EditorIcon("atom-icon-lush")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Variables/Double")]
    [HideMonoScript]
    public class DoubleVariableMite : VariableMite<double> {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Holds a simple double-precision value."
        );
    }
}
