using System;

namespace Vigeo.Mites {

    public static class VariableMiteExtensions {
        
        public static void RunWithValue<T>(this VariableMite<T> @this, Action<T> block) where T : new() {
            if (@this == null || @this.Value == null) {
                return;
            }
            block(@this.Value);
        }
        
        public static R RunWithValue<T, R>(this VariableMite<T> @this, Func<T, R> block) where T : new() {
            if (@this == null || @this.Value == null) {
                return default;
            }
            return block(@this.Value);
        }
        
        public static void RunWithInitialValue<T>(this VariableMite<T> @this, T initialValue, Action<T> block) where T : new() {
            if (@this == null) {
                return;
            }
            if (@this.Value == null) {
                @this.Value = initialValue;
            }
            block(@this.Value);
        }
        
        public static R RunWithInitialValue<T, R>(this VariableMite<T> @this, T initialValue, Func<T, R> block) where T : new() {
            if (@this == null) {
                return default;
            }
            if (@this.Value == null) {
                @this.Value = initialValue;
            }
            return block(@this.Value);
        }

        public static void RunWithValues<T1, T2, VT1, VT2>(this ValueTuple<VT1, VT2> @this, Action<T1, T2> block)
            where VT1 : VariableMite<T1> where VT2 : VariableMite<T2> where T1 : new() where T2 : new() {
            if (@this.Item1 == null || @this.Item1.Value == null || @this.Item2 == null || @this.Item2.Value == null) {
                return;
            }
            block(@this.Item1.Value, @this.Item2.Value);
        }
        
        public static void RunWithValues<T1, T2, VT2>(this ValueTuple<T1, VT2> @this, Action<T1, T2> block)
            where VT2 : VariableMite<T2> where T1 : new() where T2 : new() {
            if (@this.Item1 == null || @this.Item2 == null || @this.Item2.Value == null) {
                return;
            }
            block(@this.Item1, @this.Item2.Value);
        }
        
        public static void RunWithValues<T1, T2, VT1>(this ValueTuple<VT1, T2> @this, Action<T1, T2> block)
            where VT1 : VariableMite<T1> where T1 : new() where T2 : new() {
            if (@this.Item1 == null || @this.Item1.Value == null || @this.Item2 == null) {
                return;
            }
            block(@this.Item1.Value, @this.Item2);
        }

        public static R RunWithValues<T1, T2, VT1, VT2, R>(this ValueTuple<VT1, VT2> @this, Func<T1, T2, R> block) 
            where VT1 : VariableMite<T1> where VT2 : VariableMite<T2> where T1 : new() where T2 : new() {
            if (@this.Item1 == null || @this.Item1.Value == null || @this.Item2 == null || @this.Item2.Value == null) {
                return default;
            }
            return block(@this.Item1.Value, @this.Item2.Value);
        }
        
        public static R RunWithValues<T1, T2, VT1, R>(this ValueTuple<VT1, T2> @this, Func<T1, T2, R> block) 
            where VT1 : VariableMite<T1> where T1 : new() where T2 : new() {
            if (@this.Item1 == null || @this.Item1.Value == null || @this.Item2 == null) {
                return default;
            }
            return block(@this.Item1.Value, @this.Item2);
        }
        
        public static void RunWithValues<T1, T2, T3, VT1, VT2>(this ValueTuple<VT1, VT2, T3> @this, Action<T1, T2, T3> block)
            where VT1 : VariableMite<T1> where VT2 : VariableMite<T2> where T1 : new() where T2 : new() where T3 : new() {
            if (@this.Item1 == null || @this.Item1.Value == null || @this.Item2 == null || @this.Item2.Value == null || @this.Item3 == null) {
                return;
            }
            block(@this.Item1.Value, @this.Item2.Value, @this.Item3);
        }
    }
}
