using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Vigeo.Mites {

    /* [EditorIcon("atom-icon-lush")] */
    [CreateAssetMenu(menuName = "Vigeo/Mites/Variables/Boolean (Conditional)")]
    [HideMonoScript]
    public class ConditionalBooleanVariableMite : BooleanVariableMite, AcyclicValueCalculator<bool> {

        protected override string TypeDescription => string.Join("\n",
            TypeInfo,
            "Returns TRUE (<i>read-only</i> and calculated <i>dynamically</i>) if all conditions are met. " +
            "In case of a cyclic condition dependency FALSE will be returned for each value calculation attempt."
        );

        AcyclicValueCalculator<bool> AcyclicValueCalculator<bool>.Instance { get; set; }

        [Title("Variable conditions")]
        [SerializeReference, Space]
        [InfoBox("Watch out for cyclic dependencies (this can happen if any condition depends on this variable).", Icon = SdfIconType.ExclamationTriangleFill)]
        public List<Condition> conditions;

        [ShowInInspector, ReadOnly, LabelText("Value"), LabelWidth(37), PropertySpace, HideInPlayMode]
        public override bool InitialValue {
            get => false;
            set {}
        }

        [ShowInInspector, ReadOnly, LabelText("Value"), LabelWidth(37), PropertySpace, HideInEditorMode]
        public override bool Value {
            get => ((AcyclicValueCalculator<bool>) this).CalculateValue(() =>
                conditions.All(condition => condition.IsMet())
            );
            set {}
        }
    }
}
