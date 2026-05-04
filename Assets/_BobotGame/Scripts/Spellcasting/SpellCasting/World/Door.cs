using System;
using UnityEngine;
namespace SpellCasting.World
{
    [ExecuteAlways]
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private GameObject deactivateWhenOpen;
        [SerializeField]
        private GameObject activateWhenOpen;

        [SerializeField]
        private Direction facingDirection = Direction.NORTH;
        public Direction FacingDirection => facingDirection;

        [SerializeField]
        private bool occupied;
        public bool Occupied { get => occupied; set => occupied = value; }

        [SerializeField]
        private int doorIndex;
        public int DoorIndex { get => doorIndex; set => doorIndex = value; }

        public void SetOpen(bool open)
        {
            if (deactivateWhenOpen != null)
            {
                deactivateWhenOpen.SetActive(!open);
            }
            if (activateWhenOpen != null)
            {
                activateWhenOpen.SetActive(open);
            }

            //GetComponentInChildren<Renderer>()?.material.SetColor("_Color", closed ? Color.red : Color.cyan);
        }

        public void OnValidate()
        {
            var nip = GetComponentInParent<Room>();
            if (nip == null)
                return;

            nip.OnValidate();
        }
        private void OnDestroy()
        {
#if UNITY_EDITOR
            var nip = GetComponentInParent<Room>();
            if (nip == null)
                return;

            Debug.LogError($"don't forget to remove this from the Room {nip.name}", nip);
#endif
        }
    }
}