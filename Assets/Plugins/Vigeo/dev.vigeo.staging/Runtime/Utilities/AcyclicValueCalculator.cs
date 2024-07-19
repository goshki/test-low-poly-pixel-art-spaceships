using System;
using UnityEngine;

namespace Vigeo {

    public interface AcyclicValueCalculator<T> : IDisposable {

        protected AcyclicValueCalculator<T> Instance { get; set; }

        public T CalculateValue(Func<T> calculationBlock) {
            if (Instance != null) {
                Debug.LogError("Cyclic value calculation.");
                return default;
            }
            using (Instance = this) {
                return calculationBlock();
            }
        }

        void IDisposable.Dispose() {
            Instance = null;
        }
    }
}
