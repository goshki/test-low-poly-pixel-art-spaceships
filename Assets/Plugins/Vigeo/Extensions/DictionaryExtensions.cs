using System;
using System.Collections.Generic;

namespace Vigeo {

    public static class DictionaryExtensions {

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) {
            return dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider) {
            return dictionary.TryGetValue(key, out TValue value) ? value : defaultValueProvider();
        }

        /// <summary>
        /// Deconstructs dictionary key-value pair into a key/value tuple.
        /// </summary>
        /// <example>
        /// Given <code>Dictionary&lt;string, object&gt; objectMap</code>, iterate it using:
        /// <code>
        /// foreach (var (key, value) in objectMap) {
        ///		// ...
        /// }
        /// </code>
        /// </example>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value) {
            key = pair.Key;
            value = pair.Value;
        }

        public static void OnBeforeSerialize<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, List<TKey> keys, List<TValue> values) {
            keys.Clear();
            values.Clear();
            foreach (var (key, value) in dictionary) {
                keys.Add(key);
                values.Add(value);
            }
        }

        public static void OnAfterDeserialize<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, List<TKey> keys, List<TValue> values) {
            dictionary.Clear();
            int entriesTotal = Math.Min(keys.Count, values.Count);
            for (int i = 0; i < entriesTotal; i++) {
                dictionary.Add(keys[i], values[i]);
            }
        }
    }
}
