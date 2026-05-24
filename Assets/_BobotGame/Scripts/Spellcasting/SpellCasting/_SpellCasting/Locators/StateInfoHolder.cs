using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class StateInfoHolder : MonoBehaviour
    {
        [SerializeField]
        protected ActiveStateInfo[] stateInfoList;

        public Dictionary<Type, ActiveStateInfo> typeToStateInfo = new Dictionary<Type, ActiveStateInfo>();

        protected virtual void Awake()
        {
            for (int i = 0; i < stateInfoList.Length; i++)
            {
                AddChild(stateInfoList[i]);
            }
        }

        public void AddChild(ActiveStateInfo component)
        {
            Type type = component.GetType();
            if (typeToStateInfo.ContainsKey(type))
            {
                Debug.LogError($"child with the type {type} already exists. multiple children with the same type are not supported");
                return;
            }
            typeToStateInfo[type] = component;
        }

        public ActiveStateInfo GetStateInfo(Type type)
        {
            if (typeToStateInfo.ContainsKey(type))
            {
                return typeToStateInfo[type];
            }
            Debug.LogError($"could not find child with the type {type}", this);
            return default;
        }   
    }
}