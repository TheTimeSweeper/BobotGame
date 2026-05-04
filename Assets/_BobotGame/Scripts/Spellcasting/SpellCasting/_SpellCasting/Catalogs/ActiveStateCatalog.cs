using ActiveStates;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SpellCasting
{

    public class ActiveStateCatalog : Singleton<ActiveStateCatalog>
    {
        public static Dictionary<string, Type> StateTypes;

        protected override void InitOnce()
        {
            StateTypes = new Dictionary<string, Type>();

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(ActiveState));

            IEnumerable<Type> types = assembly.GetTypes().Where(type => type != typeof(ActiveState) && typeof(ActiveState).IsAssignableFrom(type));
            foreach (Type type in types)
            {
                StateTypes.Add(type.FullName, type);
            }
        }

        public static ActiveState InstantiateState(SerializableActiveState state) => InstantiateState<ActiveState>(state.activeStateName);
        public static ActiveState InstantiateState(string state) => InstantiateState<ActiveState>(state);
        public static T InstantiateState<T>(SerializableActiveState state) where T : ActiveState => InstantiateState<T>(state.activeStateName);
        public static T InstantiateState<T>(string state) where T : ActiveState
        {
            if (string.IsNullOrEmpty(state) || !StateTypes.ContainsKey(state))
            {
                Debug.LogError($"stateType {state} could not be found");
                return null;
            }

            return InstantiateState<T>(StateTypes[state]);
        }

        public static ActiveState InstantiateState(Type state) => InstantiateState<ActiveState>(state);
        public static T InstantiateState<T>(Type state) where T : ActiveState
        {
            object newState = Activator.CreateInstance(state);

            if (newState is not T)
            {
                Debug.LogError($"stateType {newState.GetType()} is not of type {typeof(T).ToString()}");
                return null;
            }
            //Util.Log($"successfully entered state {newState.GetType().ToString()}");
            return newState as T;
        }
    }
}