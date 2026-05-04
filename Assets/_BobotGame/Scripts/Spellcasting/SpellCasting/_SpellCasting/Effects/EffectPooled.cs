using SpellCasting.Projectiles;
using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    //jam I should not be doing this
    public class EffectPooled : MonoBehaviour
    {
        [SerializeField]
        public EffectIndex effectIndex;

        [SerializeField]
        private ParticleSystem[] systems;

        [SerializeField]
        private float effectTime;

        //jam another thing to generalize
        [SerializeField]
        private List<MonoBehaviour> effectsWithIntParameter;

        private float _tim;

        void Update()
        {
            _tim += Time.deltaTime;
            if(_tim > effectTime)
            {
                EffectManager.ReturnEffect(this);
                _tim = 0;
            }
        }

        public EffectPooled StartEffect(int genericParameter)
        {
            gameObject.SetActive(true);
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].Play();
            }

            for (int i = 0; i < effectsWithIntParameter.Count; i++)
            {
                IEffectWithIntParamter initializedComponent = effectsWithIntParameter[i] as IEffectWithIntParamter;
                if (initializedComponent != null)
                {
                    initializedComponent.InitParameter(genericParameter);
                }
                effectsWithIntParameter[i].enabled = true;
            }

            return this;
        }

        public EffectPooled EndEffect()
        {
            gameObject.SetActive(false);
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].Stop();
            }
            return this;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            AssignSubComponents();
        }

        [ContextMenu("Assign SubComponents")]
        private void AssignSubComponents()
        {
            UnityEditor.Undo.RecordObject(this, "subcomponents");
            IEffectWithIntParamter[] attachedSubComponents = GetComponents<IEffectWithIntParamter>();
            for (int i = 0; i < attachedSubComponents.Length; ++i)
            {
                if (!effectsWithIntParameter.Contains((MonoBehaviour)attachedSubComponents[i]))
                {
                    effectsWithIntParameter.Add((MonoBehaviour)attachedSubComponents[i]);
                }
            }
        }
#endif
    }
}