using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace SpellCasting.World
{
    public class Room : MonoBehaviour
    {
        [SerializeField]
        private List<Door> doors;
        public List<Door> Doors => doors;

        [SerializeField]
        private List<OverlapZone> overlapZones;
        public List<OverlapZone> OverlapZones => overlapZones;

        //jam this should be in a roominfo scriptableojbect but I can't be fucked
        [SerializeField]
        private float roomCost = 1;
        public float RoomCost => roomCost;

        [SerializeField]
        public float RoomWeight = 1;

        private Collider[] _colliders;

        [SerializeField]
        private bool validateRooms = false;

        public void OnValidate()
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "setting door indices");

            var getDoors = GetComponentsInChildren<Door>();
            for (int i = 0; i < getDoors.Length; i++)
            {
                if (!doors.Contains(getDoors[i]))
                {
                    doors.Add(getDoors[i]);
                }
            }

            for (int i = doors.Count - 1; i >= 0; i--)
            {
                if (doors[i] == null)
                {
                    doors.RemoveAt(i);
                    continue;
                }
                UnityEditor.Undo.RecordObject(doors[i], "setting door indices");
                doors[i].DoorIndex = i;
            }

            if (!validateRooms)
                return;

            validateRooms = false;

            Transform parent;
            for (int i = doors.Count - 1; i >= 0; i--)
            {
                if (doors[i] == null)
                    break;

                parent = doors[i].transform.parent;
                while (parent != null)
                {
                    if(parent.localPosition + parent.localScale + parent.localRotation.eulerAngles != Vector3.one)
                    {
                        Debug.LogError($"parent {parent.name} of door {doors[i].name} is invalid. make sure localscale and localposition are default", parent);
                    }
                    parent = parent.parent;
                }
            }

            for (int i = overlapZones.Count - 1; i >= 0; i--)
            {
                if (overlapZones[i] == null)
                    break;
                parent = overlapZones[i].transform.parent;
                while (parent != null)
                {
                    if (parent.localPosition + parent.localScale + parent.localRotation.eulerAngles != Vector3.one)
                    {
                        Debug.LogError($"parent {parent.name} of overlapZone {overlapZones[i].name} is invalid. make sure localscale and localposition are default", parent);
                    }
                    parent = parent.parent;
                }
            }
#endif
        }
    }
}