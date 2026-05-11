using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class ComponentHolder : MonoBehaviour
    {
        public Component[] Components;
        private Dictionary<Type, Component> componentMap;

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            componentMap = new Dictionary<Type, Component>();
            for (int i = 0; i < Components.Length; i++)
            {
                componentMap[Components[i].GetType()] = Components[i];
            }
        }

        new public T GetComponent<T>() where T : Component
        {
            return componentMap[typeof(T)] as T;
        }
        new public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();
            return component ? true : false;
        }
    }
}