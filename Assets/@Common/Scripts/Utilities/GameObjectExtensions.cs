using System;
using UnityEngine;

namespace Vigeo {
    
    public static class GameObjectExtensions {

        public static T WithComponent<T>(this Component @this, Action<T> block) {
            if (@this.TryGetComponent(out T component)) {
                block(component);
            }
            return component;
        }
        
        public static T WithComponent<T>(this GameObject @this, Action<T> block) {
            if (@this.TryGetComponent(out T component)) {
                block(component);
            }
            return component;
        }
        
        public static T WithComponentInChildren<T>(this Component @this, Action<T> block) {
            T component = @this.GetComponentInChildren<T>();
            if (component != null) {
                block(component);
            }
            return component;
        }
        
        public static T WithComponentInChildren<T>(this GameObject @this, Action<T> block) {
            T component = @this.GetComponentInChildren<T>();
            if (component != null) {
                block(component);
            }
            return component;
        }
        
        public static void WithComponentsInChildren<T>(this Component @this, Action<T> block) {
            var components = @this.GetComponentsInChildren<T>();
            foreach (var component in components) {
                block(component);
            }
        }
        
        public static void WithComponentsInChildren<T>(this GameObject @this, Action<T> block) {
            var components = @this.GetComponentsInChildren<T>();
            foreach (var component in components) {
                block(component);
            }
        }
        
        public static void IfNull<T>(this T @this, Action block) where T : Component {
            if (@this == null) {
                block();
            }
        }
        
        public static void IfNullLogWarning<T>(this T @this, string message, GameObject gameObject = null) where T : Component {
            if (@this == null) {
                Debug.LogWarning(message, gameObject);
            }
        }
    }
}
