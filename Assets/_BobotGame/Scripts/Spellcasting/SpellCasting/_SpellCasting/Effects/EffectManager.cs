using System;
using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    public class EffectManager : Singleton<EffectManager>
    {
        [SerializeField]
        public EffectPooled[] effectPrefabs;

        private Dictionary<EffectIndex, EffectPooled> _effectPrefabMap = new Dictionary<EffectIndex, EffectPooled>();
        private Dictionary<EffectIndex, EffectPool> _effectPools = new Dictionary<EffectIndex, EffectPool>();

        protected override void InitOnce()
        {
            //uh it's on the dontdestroy maingame isnt it
            //DontDestroyOnLoad(gameObject);
            for(int i = 0; i < effectPrefabs.Length; i++)
            {
                _effectPrefabMap.Add(effectPrefabs[i].effectIndex, effectPrefabs[i]);
                _effectPools.Add(effectPrefabs[i].effectIndex, new EffectPool());
            }
        }
        public static void SpawnEffect(EffectIndex index, Vector3 position, Transform transform = null, int genericParamter = -1)
            => SpawnEffect(index, position, Quaternion.identity, Vector3.one, transform, genericParamter);
        public static void SpawnEffect(EffectIndex index, Vector3 position, Quaternion rotation, Transform transform = null, int genericParamter = -1)
            => SpawnEffect(index, position, rotation, Vector3.one, transform, genericParamter);
        public static void SpawnEffect(EffectIndex index, Vector3 position, Quaternion rotation, Vector3 scale, Transform transform = null, int genericParamter = -1)
        {
            EffectPooled effect = Instance.GetEffectFromPool(index).StartEffect(genericParamter);

            effect.transform.position = position;
            effect.transform.rotation = rotation;
            effect.transform.localScale = scale;
            effect.transform.parent = transform != null? transform : Instance.transform;
        }

        public EffectPooled GetEffectFromPool(EffectIndex index)
        {
            if (_effectPools[index].Count > 0)
            {
                return _effectPools[index].RentItem();
            }
            else
            {
                return Instantiate(_effectPrefabMap[index], transform);
            }
        }

        internal static void ReturnEffect(EffectPooled effectPooled)
        {
            effectPooled.transform.parent = Instance.transform;
            Instance._effectPools[effectPooled.effectIndex].ReturnItem(effectPooled);
        }
    }
}