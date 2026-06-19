using System;
using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    public abstract class VariableStatModifier<T>
    {
        public T Value;
        public string ID;
        public float priority;

        public VariableStatModifier(T value, string iD, float priority)
        {
            Value = value;
            ID = iD;
            this.priority = priority;
        }

        public abstract void ModifyStat(ref T originalValue);
    }

    public class AddStatModifier : VariableStatModifier<float>
    {
        public AddStatModifier(float value, string iD, float priority) : base(value, iD, priority) { }

        public override void ModifyStat(ref float originalValue)
        {
            originalValue += Value;
        }
    }

    public class MultiplyStatModifier : VariableStatModifier<float>
    {
        public MultiplyStatModifier(float value, string iD, float priority) : base(value, iD, priority) { }

        public override void ModifyStat(ref float originalValue)
        {
            originalValue *= Value;
        }
    }

    public class OverrideStatModifier : VariableStatModifier<float>
    {
        public OverrideStatModifier(float value, string iD, float priority) : base(value, iD, priority) { }

        public override void ModifyStat(ref float originalValue)
        {
            originalValue = Value;
        }
    }

    [System.Serializable]
    public class VariableNumberStat : VariableStat<float>
    {
        public VariableNumberStat(float baseValue) : base(baseValue) { }

        public override void ApplyAddModifier(float value, string id, float priority = BASE_ADD_PRIORITY)
        {
            base.ApplyAddModifier(value, id, priority);
            Modifiers.Add(new AddStatModifier(value, id, priority));
        }

        public override void ApplyMultiplyModifier(float value, string id, float priority = BASE_MULTI_PRIORITY)
        {
            base.ApplyMultiplyModifier(value, id, priority);
            Modifiers.Add(new MultiplyStatModifier(value, id, priority));
        }

        public override void ApplyOverrideModifier(float value, string id, float priority = BASE_OVERRIDE_PRIORITY)
        {
            base.ApplyOverrideModifier(value, id, priority);
            Modifiers.Add(new OverrideStatModifier(value, id, priority));
        }

        public static implicit operator VariableNumberStat(float value)
        {
            return new VariableNumberStat(value);
        }

        public static implicit operator VariableNumberStat(int value)
        {
            return new VariableNumberStat(value);
        }

        public static implicit operator float(VariableNumberStat stat)
        {
            return stat.Value;
        }
        public static implicit operator int(VariableNumberStat stat)
        {
            return Mathf.RoundToInt(stat.Value);
        }

        public static float operator *(VariableNumberStat a,  VariableNumberStat b)
        {
            return a.Value * b.Value;
        }
        public static float operator +(VariableNumberStat a, VariableNumberStat b)
        {
            return a.Value + b.Value;
        }
        public static float operator -(VariableNumberStat a, VariableNumberStat b)
        {
            return a.Value - b.Value;
        }
        public static float operator /(VariableNumberStat a, VariableNumberStat b)
        {
            return a.Value / b.Value;
        }
    }

    public abstract class VariableStat
    {
    }

    public abstract class VariableStat<T> : VariableStat
    {
        public const float BASE_ADD_PRIORITY = 0;
        public const float BASE_MULTI_PRIORITY = 1;
        public const float BASE_OVERRIDE_PRIORITY = 2;

        protected VariableStat(T baseValue)
        {
            _baseValue = baseValue;
            _modifiers = new List<VariableStatModifier<T>>();
            dirty = true;
            UpdateValueWithModifiers();
        }

        protected bool dirty = true;

        [SerializeField]
        private T _baseValue;
        [SerializeField]
        private T _lastModifiedValue;
        public T Value
        {
            get
            {
                if (dirty)
                {
                    dirty = false;
                    _lastModifiedValue = UpdateValueWithModifiers();
                }
                return _lastModifiedValue;
            }
        }

        private T UpdateValueWithModifiers()
        {
            Modifiers.Sort((mod1, mod2) =>
            {
                if (mod1.priority > mod2.priority)
                    return -1;
                if (mod1.priority < mod2.priority)
                    return 1;
                return 0;
            });

            T modifiedValue = _baseValue;
            for (int i = 0; i < Modifiers.Count; i++)
            {
                Modifiers[i].ModifyStat(ref modifiedValue);
            }
            onValueChanged?.Invoke(modifiedValue);
            return modifiedValue;
        }

        public event Action<T> onValueChanged;

        public V Clone<V>() where V : VariableStat<T>, new()
        {
            V newStat = new V();
            newStat._baseValue = _baseValue;
            newStat._modifiers = Modifiers;
            return newStat;
        }

        private List<VariableStatModifier<T>> _modifiers;
        public List<VariableStatModifier<T>> Modifiers
        {
            get
            {
                if (_modifiers == null)
                {
                    _modifiers = new List<VariableStatModifier<T>>();
                }
                return _modifiers;
            }
        }

        public void DebugOverrideBaseValue(T value)
        {
            _baseValue = value;
            dirty = true;
        }
        public T DebugGetBaseValue()
        {
            return _baseValue;
        }

        public virtual void ApplyAddModifier(T value, string id, float priority = BASE_ADD_PRIORITY)
        {
            dirty = true;
        }
        public virtual void ApplyMultiplyModifier(T value, string id, float priority = BASE_MULTI_PRIORITY)
        {
            dirty = true;
        }
        public virtual void ApplyOverrideModifier(T value, string id, float priority = BASE_OVERRIDE_PRIORITY)
        {
            dirty = true;
        }

        public virtual void RemoveModifier(string id)
        {
            for (int i = 0; i < Modifiers.Count; i++)
            {
                if (Modifiers[i].ID == id)
                {
                    Modifiers.RemoveAt(i);
                    dirty = true;
                    return;
                }
            }
        }
    }
}