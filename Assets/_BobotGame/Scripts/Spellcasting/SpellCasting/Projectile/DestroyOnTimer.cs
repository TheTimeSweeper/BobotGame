using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class DestroyOnTimer : MonoBehaviour
    {
        [SerializeField]
        private float lifeTime = 1;

        private float _tim;
        void FixedUpdate()
        {
            _tim += Time.deltaTime;
            if (_tim > lifeTime)
            {
                Destroy(gameObject); 
            }
        }
    }
}