using System;
using System.Collections.Generic;
using UnityEngine;

namespace Void.Core
{
    public class ServiceLocator : MonoBehaviour
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        
        public void Register<T>(T instance) where T : class
        {
            _services.Add(typeof(T), instance);
        }

        public T Get<T>() where T : class
        {
            if(_services.TryGetValue(typeof(T), out var instance))
            {
                return (T)instance;
            }

            Debug.LogError($"Service of type {typeof(T)} not found.");
            return null;
        }
    }
}