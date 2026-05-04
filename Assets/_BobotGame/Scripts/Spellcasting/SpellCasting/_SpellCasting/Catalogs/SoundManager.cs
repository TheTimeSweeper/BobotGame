using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        public AudioClip[] AllSounds;

        private static bool[] _playedThisFrame;

        protected override void InitOnce()
        {
            base.InitOnce();
            _playedThisFrame = new bool[AllSounds.Length];
        }

        public static AudioClip GetSoundClip(int sound)
        {
            if (_playedThisFrame[sound])
                return null;
            _playedThisFrame[sound] = true;
            return Instance.AllSounds[sound];
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _playedThisFrame.Length; i++)
            {
                _playedThisFrame[i] = false;
            }
        }
    }
}