using UnityEngine;

namespace SpellCasting
{
    public abstract class Singleton<T>: MonoBehaviour where T : Singleton<T>
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                HandleAdditionalInstance();
                Instance.ReInit();
                return;
            }
            Instance = this as T;
            InitOnce();
        }

        protected virtual void InitOnce() { }
        public virtual void ReInit() { }
        protected virtual void HandleAdditionalInstance()
        {
            Destroy(gameObject);
        }
    }
}