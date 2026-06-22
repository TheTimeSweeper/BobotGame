using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    [RequireComponent(typeof(CameraController), typeof(CharacterMaster))]
    public class CameraControllerTracker : MonoBehaviour
    {
        public CameraController cameraController;
        public CharacterMaster characterMaster;

        public static List<CameraControllerTracker> instanceList = new List<CameraControllerTracker>();

        void OnEnable()
        {
            instanceList.Add(this);
        }
        void OnDisable()
        {
            instanceList.Remove(this);
        }

        public static CameraControllerTracker FindByCurrentBody(GameObject gob)
        {
            for (int i = 0; i < instanceList.Count; i++)
            {
                if(instanceList[i].characterMaster.CurrentBody.gameObject == gob)
                {
                    return instanceList[i];
                }
            }
            return null;
        }
    }
}
