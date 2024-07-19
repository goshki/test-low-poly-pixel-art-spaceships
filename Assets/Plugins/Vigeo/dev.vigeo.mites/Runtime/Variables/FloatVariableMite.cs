using System;
using Sirenix.OdinInspector;
using UnityEngine;
using static Vigeo.Mites.ComparisonMode.Relation;

namespace Vigeo.Mites {
    
    [Serializable, HideReferenceObjectPicker]
    public class FloatVariableMiteCondition : Condition {

        [HideLabel, HorizontalGroup("")]
        public FloatVariableMite variable;
        
        [HideLabel, HorizontalGroup("", Width = 40)]
        public ComparisonMode.Relation comparisonMode;

        [HideLabel, HorizontalGroup("", Width = 62)]
        public float targetValue;

        public override bool IsMet() => variable.RunWithValue(value => comparisonMode switch {
            StrictlyLowerThan => value < targetValue,
            LowerThan => value <= targetValue,
            Equal => Math.Abs(value - targetValue) < float.Epsilon,
            GreaterThan => value >= targetValue,
            StrictlyGreaterThan => value > targetValue,
            NotEqual => Math.Abs(value - targetValue) >= float.Epsilon,
            _ => false
        });
    }
    
    [Serializable, HideReferenceObjectPicker]
    public class FloatToFloatVariableMiteCondition : Condition {

        [HideLabel, HorizontalGroup("")]
        public FloatVariableMite variable;
        
        [HideLabel, HorizontalGroup("", Width = 40)]
        public ComparisonMode.Relation comparisonMode;

        [HideLabel, HorizontalGroup("")]
        public FloatVariableMite targetVariable;

        public override bool IsMet() => (variable, targetVariable).RunWithValues((float value, float targetValue) => comparisonMode switch {
            StrictlyLowerThan => value < targetValue,
            LowerThan => value <= targetValue,
            Equal => Math.Abs(value - targetValue) < float.Epsilon,
            GreaterThan => value >= targetValue,
            StrictlyGreaterThan => value > targetValue,
            NotEqual => Math.Abs(value - targetValue) >= float.Epsilon,
            _ => false
        });
    }

    /* [EditorIcon("atom-icon-lush")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Variables/Float")]
    [HideMonoScript]
    public class FloatVariableMite : VariableMite<float> {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Holds a simple float value."
        );
    }
}
