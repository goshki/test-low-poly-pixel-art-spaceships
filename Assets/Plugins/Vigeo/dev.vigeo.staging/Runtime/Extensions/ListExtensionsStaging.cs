using System;
using System.Collections;
using System.Collections.Generic;

namespace Vigeo {
    
    public static class ListExtensionsStaging {
        
        public static bool Any<T>(this IList<T> @this, Func<T, bool> predicate) {
            for (int i = 0; @this != null && i < @this.Count; ++i) {
                if (predicate(@this[i])) return true;
            }
            return false;
        }
        
        public static bool All<T>(this IList<T> @this, Func<T, bool> predicate) {
            for (int i = 0; @this != null && i < @this.Count; ++i) {
                if (predicate(@this[i]) == false) return false;
            }
            return true;
        }
        
        public static T First<T>(this IList<T> @this, Func<T, bool> predicate) where T : class {
            for (int i = 0; @this != null && i < @this.Count; ++i) {
                if (predicate(@this[i])) return @this[i];
            }
            return null;
        }
        
        // ReSharper disable once MethodOverloadWithOptionalParameter // we need [defaultValue] to prevent clash with class-constrained version of [First()]
        public static T? First<T>(this IList<T> @this, Func<T, bool> predicate, T defaultValue = default) where T : struct {
            for (int i = 0; @this != null && i < @this.Count; ++i) {
                if (predicate(@this[i])) return @this[i];
            }
            return default;
        }
        
        public static List<T> Filter<T>(this IList<T> @this, Func<T, bool> predicate) where T : class {
            var matchingElements = new List<T>();
            for (int i = 0; @this != null && i < @this.Count; ++i) {
                if (predicate(@this[i])) {
                    matchingElements.Add(@this[i]);
                }
            }
            return matchingElements;
        }
        
        // ReSharper disable once MethodOverloadWithOptionalParameter // we need [defaultValue] to prevent clash with class-constrained version of [Filter()]
        public static List<T> Filter<T>(this IList<T> @this, Func<T, bool> predicate, T defaultValue = default) where T : struct {
            var matchingElements = new List<T>();
            foreach (T element in @this) {
                if (predicate(element)) {
                    matchingElements.Add(element);
                }
            }
            return matchingElements;
        }
        
        public static List<T> FilterByType<T>(this IList @this) where T : class {
            var matchingElements = new List<T>();
            for (int i = 0; @this != null && i < @this.Count; ++i) {
                if (@this[i] is T) {
                    matchingElements.Add(@this[i] as T);
                }
            }
            return matchingElements;
        }
        
        // ReSharper disable once MethodOverloadWithOptionalParameter // we need [defaultValue] to prevent clash with class-constrained version of [FilterByType()]
        public static List<T> FilterByType<T>(this IList @this, T defaultValue = default) where T : struct {
            var matchingElements = new List<T>();
            for (int i = 0; @this != null && i < @this.Count; ++i) {
                if (@this[i] is T) {
                    matchingElements.Add((T) @this[i]);
                }
            }
            return matchingElements;
        }
        
        public static bool AddIfNotExists<T>(this IList<T> @this, T item) {
            if (!@this.Contains(item)) {
                @this.Add(item);
                return true;
            }
            return false;
        }
        
        public static bool RemoveIfExists<T>(this IList<T> @this, T item) {
            return @this.Remove(item);
        }
        
        public static T NextAfter<T>(this IList<T> @this, T item) where T : class {
            if (@this.IndexOf(item) < 0) {
                return null;
            }
            var nextItemIndex = @this.IndexOf(item) + 1;
            if (nextItemIndex < @this.Count) {
                return @this[nextItemIndex];
            }
            return null;
        }
        
        // ReSharper disable once MethodOverloadWithOptionalParameter // we need [defaultValue] to prevent clash with class-constrained version of [First()]
        public static T? NextAfter<T>(this IList<T> @this, T item, T defaultValue = default) where T : struct {
            if (@this.IndexOf(item) < 0) {
                return default;
            }
            var nextItemIndex = @this.IndexOf(item) + 1;
            if (nextItemIndex < @this.Count) {
                return @this[nextItemIndex];
            }
            return default;
        }
        
        public static IEnumerable<T> Reversed<T>(this List<T> @this) {
            for (int i = @this.Count-1; i >= 0; i--) {
                yield return @this[i];
            }
        }

        public static T Random<T>(this List<T> @this) => @this.Count switch {
            0 => throw new IndexOutOfRangeException("List needs at least one entry to call Random()"),
            1 => @this[0],
            _ => @this[new Random().Next(0, @this.Count)]
        };
    }
}
