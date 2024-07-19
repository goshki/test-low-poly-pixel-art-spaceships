using System;
using Sirenix.OdinInspector;

namespace Vigeo.Mites {

    public static class BooleanValueExtensions {

        public static bool ToBoolean(this BooleanValue @this) =>
            @this == BooleanValue.True;
    }

    public enum BooleanValue {
        [LabelText(" TRUE")]
        True,
        [LabelText(" FALSE")]
        False
    }

    public abstract class ComparisonMode {
        
        public enum Boolean {
            [LabelText(" ==")]
            Equal,
            [LabelText(" !=")]
            NotEqual
        }
        
        public enum Containment {
            [LabelText(" ∈"), PropertyTooltip("Is element of")]
            In,
            [LabelText(" ∉"), PropertyTooltip("Is NOT element of")]
            NotIn
        }
        
        public enum Relation {
            [LabelText(" <")]
            StrictlyLowerThan,
            [LabelText(" <=")]
            LowerThan,
            [LabelText(" ==")]
            Equal,
            [LabelText(" >=")]
            GreaterThan,
            [LabelText(" >")]
            StrictlyGreaterThan,
            [LabelText(" !=")]
            NotEqual
        }

        public enum Reference {
            [LabelText(" !==")]
            NotSame,
            [LabelText(" ==")]
            Same
        }
        
        public enum Collection {
            [LabelText("Any")]
            Any,
            [LabelText("All")]
            All
        }
    }

    /* [EditorIcon("atom-icon-kingsyellow")] */
    [Serializable]
    public abstract class Condition {

        public abstract bool IsMet();
    }
    
    [Serializable]
    public abstract class Condition<T> : Condition {
        
        // Always met by default (but can additionally be checked by event data)
        public override bool IsMet() => true;

        public abstract bool IsMet(T parameter);
    }
}
