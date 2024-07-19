using System;
using System.Collections.Generic;

namespace Vigeo.Events {
    
    public interface EventData {}

    internal class EventDispatcher {

        private readonly Dictionary<Type, Action<object>> events = new();

        private readonly Dictionary<object, Action<object>> callbacks = new();

        public void Register<T>(Action<T> callback) where T : struct {
            Action<object> callbackAction = (o) => callback((T) o);
            if (events.ContainsKey(typeof(T))) {
                events[typeof(T)] = events[typeof(T)] + callbackAction;
            } else {
                events.Add(typeof(T), callbackAction);
            }
            callbacks.Add(callback, callbackAction);
        }

        public void Unregister<T>(Action<T> callback) where T : struct {
            if (events.ContainsKey(typeof(T)) && callbacks.TryGetValue(callback, out var callbackAction)) {
                events[typeof(T)] -= callbackAction;
            }
            callbacks.Remove(callback);
        }

        public void Raise<T>(T eventData) where T : struct {
            events.GetValueOrDefault(typeof(T), null)?.Invoke(eventData);
        }
    }
    
    /* [EditorIcon("atom-icon-cherry")] */
    public static class Event {

        private static readonly EventDispatcher dispatcher = new();

        public static void Register<T>(Action<T> callback) where T : struct {
            dispatcher.Register(callback);
        }

        public static void Unregister<T>(Action<T> callback) where T : struct {
            dispatcher.Unregister(callback);
        }

        public static void Raise<T>(T eventData) where T : struct {
            dispatcher.Raise(eventData);
        }
    }
}
