using System.Collections.Generic;

namespace Vigeo.Mites {

    /* [EditorIcon("atom-icon-piglet")] */
    public abstract class VariableListMite<T> : VariableMite<List<T>> {

        protected override void ResetValue() {
            if (InitialValue == null) {
                InitialValue = new List<T>();
            }
            if (Value == null) {
                Value = new List<T>();
            }
            Value.Clear();
            Value.AddRange(InitialValue);
        }
    }
}
