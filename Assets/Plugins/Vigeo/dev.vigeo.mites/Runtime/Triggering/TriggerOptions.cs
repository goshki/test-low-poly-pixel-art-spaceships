using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vigeo.Mites;

namespace Vigeo {

    [Serializable, InlineProperty]
    public struct TriggerOptions {
        
        [Title("Triggering configuration")]
        [SerializeReference, Space]
        public List<Condition> conditions;
        
        [LabelWidth(57), HorizontalGroup(70)]
        public bool oneShot;
        
        [HideLabel, ShowInInspector, ShowIf("oneShot"), ReadOnly, PropertySpace(SpaceBefore = 2)]
        [HorizontalGroup]
        public bool Triggered { get; set; }
        
        public bool CanBeTriggered => (oneShot && Triggered) == false;
        
        public bool ConditionsMatch =>
            conditions.All(condition => condition.IsMet());
    }
}
