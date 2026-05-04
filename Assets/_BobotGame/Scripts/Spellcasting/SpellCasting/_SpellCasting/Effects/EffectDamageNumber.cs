using TMPro;
using UnityEngine;

namespace SpellCasting
{
    public class EffectDamageNumber : MonoBehaviour, IEffectWithIntParamter
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private float randomOffset;

        public void InitParameter(int parameter)
        {
            text.text = parameter.ToString();
        }

        void OnEnable()
        {
            Vector3 randomPosition = Quaternion.Euler(0, 0, Random.Range(0, 360f)) * (Vector3.up * randomOffset * Random.value);
            text.transform.localPosition = randomPosition;
        }
    }
}