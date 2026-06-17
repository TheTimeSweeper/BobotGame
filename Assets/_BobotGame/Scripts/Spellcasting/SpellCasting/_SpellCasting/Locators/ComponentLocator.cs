using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class ComponentLocator<T> : MonoBehaviour where T : Object
    {
        [SerializeField]
        protected T[] componentList;

        public T[] ComponentList { get => componentList; }

        protected Dictionary<string, T> nameToComponent = new Dictionary<string, T>();

        protected virtual void Awake()
        {
            for (int i = 0; i < componentList.Length; i++)
            {
                AddChild(componentList[i]);
            }
        }

        public void AddChild(T component)
        {
            string name = string.Empty;
            if (component is ILabeled)
            {
                name = (component as ILabeled).Label;
            } else
            {
                name = component.name;
            }
            if (nameToComponent.ContainsKey(name))
            {
                Debug.LogError($"child with the name {name} already exists. multiple children with the same name are not supported");
                return;
            }
            nameToComponent[name] = component;
        }

        public virtual T LocateByName(string name)
        {
            if (nameToComponent.ContainsKey(name))
            {
                return nameToComponent[name];
            }
            Debug.LogError($"could not find child with the name {name}", this);
            return default;
        }

        public virtual GameObject LocateByNameGameObject<T2>(string name) where T2 : Component, T
        {
            if (nameToComponent.ContainsKey(name))
            {
                return (nameToComponent[name] as Component).gameObject;
            }
            Debug.LogError($"could not find child with the name {name}", this);
            return default;
        }

        public virtual T[] GetAll()
        {
            return componentList;
        }
    }
}