using System;
using UnityEngine;

namespace SpellCasting
{
    //JAM I would prefer to abstract these to be able to add more gestures
    //well I'm doin it anyway
        //wait this wasn't even being used
    //public enum GestureTypeIndex
    //{
    //    SWIPE,
    //    SWIRL,
    //    SHAKE,
    //    MASH
    //}

    public abstract class AimGesture : ScriptableObject, IComparable<AimGesture>
    {
        public const float BASE_GESTURE_LOW_PRIO = 1;
        public const float BASE_GESTURE_MID_PRIO = 2;
        public const float BASE_GESTURE_HIGH_PRIO = 3;

        [SerializeField]
        private float priority;
        public float Priority => priority;

        //[SerializeField]
        //public GestureTypeIndex index;
        //public GestureTypeIndex Index => index;

        public int CompareTo(AimGesture other)
        {
            return (int)(other.Priority - Priority);
        }

        public abstract ScriptableObjectBehavior GetBehavior();
    }
}