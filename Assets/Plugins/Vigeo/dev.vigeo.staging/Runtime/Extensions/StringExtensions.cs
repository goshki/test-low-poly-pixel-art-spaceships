using System;
using System.Collections.Generic;
using System.Text;

namespace Vigeo {

    public static class StringExtensions {
        
        public static bool IsNullOrWhitespace(this string @this) {
            if (!string.IsNullOrEmpty(@this)) {
                for (int index = 0; index < @this.Length; ++index) {
                    if (!char.IsWhiteSpace(@this[index])) {
                        return false;
                    }
                }
            }
            return true;
        }

        public static string DefaultIfEmpty(this string @this, string defaultValue) =>
            string.IsNullOrWhiteSpace(@this) ? defaultValue : @this;
        
        public static void WhenNotEmpty(this string @this, Action block) {
            if (@this == null || string.IsNullOrWhiteSpace(@this)) {
                return;
            }
            block();
        }
        
        public static void WithNotEmpty(this string @this, Action<string> block) {
            if (@this == null || string.IsNullOrWhiteSpace(@this)) {
                return;
            }
            block(@this);
        }

        public static string JoinAnd<T>(this IEnumerable<T> @this, string separator = ", ", string lastSeparator = " and ") =>
            @this.Run(values => new StringBuilder().Apply(buffer => {
                using var enumerator = values.GetEnumerator();
                if (enumerator.MoveNext()) {
                    buffer.Append(enumerator.Current);
                }
                if (enumerator.MoveNext()) {
                    var value = enumerator.Current;
                    while (enumerator.MoveNext()) {
                        buffer.Append(separator);
                        buffer.Append(value);
                        value = enumerator.Current;
                    }

                    buffer.Append(lastSeparator);
                    buffer.Append(value);
                }
            }).ToString());
    }
}
