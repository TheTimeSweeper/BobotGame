using System;
using UnityEngine;

namespace SpellCasting
{

    public class ActiveStateInfo : ScriptableObject
    {

    }

    public interface IHasStateInfo<T> : IHasStateInfoBase where T : ActiveStateInfo
    {
        T StateInfo { get; }
    }

    public interface IHasStateInfoBase
    {
        ActiveStateInfo AssignedStateInfo { get; set; }
        Type StateInfoType { get; }
    }

    public static class ActiveStateInfoUtils
    {
        public static void SetStateInfo(this IHasStateInfoBase stateInfo, StateInfoHolder holder)
        {
            stateInfo.AssignedStateInfo = holder.GetStateInfo(stateInfo.StateInfoType);
        }
    }
}