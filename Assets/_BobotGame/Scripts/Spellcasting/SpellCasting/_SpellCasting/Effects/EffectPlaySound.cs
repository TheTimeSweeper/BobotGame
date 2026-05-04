using UnityEngine;

namespace SpellCasting
{
    public class EffectPlaySound : MonoBehaviour, IEffectWithIntParamter
    {
        [SerializeField]
        private AudioSource audioSource;


        [SerializeField]
        private float pitchMin;
        [SerializeField] private float pitchMax;

        public void InitParameter(int parameter)
        {
            audioSource.pitch = Random.Range(pitchMin, pitchMax);

            audioSource.clip = SoundManager.GetSoundClip(parameter);
            if (audioSource.clip == null)
                return;
            audioSource.Play();
        }
    }
}