using SpellCasting;
using System;

namespace ActiveStates
{
    [System.Serializable]
    public struct SerializableActiveState
    {
        public string activeStateName;
        public Type _stateType;
        public Type stateType
        {
            get
            {
                if(_stateType == null)
                {
                    _stateType = ActiveStateCatalog.StateTypes[activeStateName];
                }
                return _stateType;
            }
        }

        public SerializableActiveState(Type type) : this()
        {
            _stateType = type;
            activeStateName = _stateType.FullName.ToString();
        }

        public static implicit operator ActiveState(SerializableActiveState state) { 
            return ActiveStateCatalog.InstantiateState(state);
        }
    }
}
