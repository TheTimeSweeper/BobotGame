using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class ReRigger : MonoBehaviour
    {

        [SerializeField]
        private Transform[] adder;
        [SerializeField]
        private ReRigGroup[] overrides;

        void LateUpdate()
        {
            for (int i = 0; i < overrides.Length; i++)
            {
                overrides[i].UpdatePosition();
            }
        }

        [System.Serializable]
        public struct ReRigGroup
        {
            public Transform transform;
            public Vector3 localPositionOverride;

            public void UpdatePosition()
            {
                transform.localPosition = localPositionOverride;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Add From Adder")]
        void AddFromAdder()
        {
            UnityEditor.Undo.RecordObject(this, "adding");
            Array.Resize(ref overrides, overrides.Length + adder.Length);
            for (int i = 0; i < adder.Length; i++)
            {
                overrides[i + overrides.Length - 1] = new ReRigGroup { transform = adder[i].transform };
            }
        }
        [ContextMenu("Replace From Adder")]
        void ReplaceFromAdder()
        {
            UnityEditor.Undo.RecordObject(this, "replacing");
            Array.Resize(ref overrides, adder.Length);
            for (int i = 0; i < adder.Length; i++)
            {
                overrides[i ] = new ReRigGroup { transform = adder[i].transform };
            }
        }


        [ContextMenu("Set Local Positions from Transform")]
        void AutomaticallySetLocalPositions()
        {
            UnityEditor.Undo.RecordObject(this, "setting");

            for (int i = 0; i < overrides.Length; i++)
            {
                if (!overrides[i].transform)
                    continue;

                overrides[i].localPositionOverride = overrides[i].transform.localPosition;
            }
        }

        [ContextMenu("Cleanup Local Positions Zeroes")]
        void CleanupLocalPositions()
        {
            UnityEditor.Undo.RecordObject(this, "cleaning");
            var newOverrides = new List<ReRigGroup>();
            for (int i = 0; i < overrides.Length; i++)
            {
                if (!overrides[i].transform || overrides[i].localPositionOverride == Vector3.zero)
                    continue;

                newOverrides.Add(overrides[i]);
            }
            overrides = newOverrides.ToArray();
        }
#endif


        [ContextMenu("Update Bone Positions")]
        void UpdateBonePositions()
        {

            for (int i = 0; i < overrides.Length; i++)
            {
                overrides[i].UpdatePosition();
            }
        }
    }
}