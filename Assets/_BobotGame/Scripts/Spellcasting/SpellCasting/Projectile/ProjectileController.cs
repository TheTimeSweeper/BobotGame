using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField]
        private List<MonoBehaviour> subComponentsBehaviours;

        [SerializeField]
        private List<MonoBehaviour> dormantComponents;

        private FireProjectileData ProjectileInfo { get; set; }

#if UNITY_EDITOR
        void Reset()
        {
            AssignSubComponents();
        }

        void OnValidate()
        {
            AssignSubComponents();
        }

        [ContextMenu("Assign SubComponents")]
        private void AssignSubComponents()
        {
            UnityEditor.Undo.RecordObject(this, "subcomponents");
            IProjectileSubComponent[] attachedSubComponents = GetComponents<IProjectileSubComponent>();
            for (int i = 0; i < attachedSubComponents.Length; ++i)
            {
                if (!subComponentsBehaviours.Contains((MonoBehaviour)attachedSubComponents[i]))
                {
                    subComponentsBehaviours.Add((MonoBehaviour)attachedSubComponents[i]);
                }
            }

            IProjectileDormant[] attachedDormantComponents = GetComponents<IProjectileDormant>();
            for (int i = 0; i < attachedDormantComponents.Length; ++i)
            {
                if(!dormantComponents.Contains((MonoBehaviour)attachedDormantComponents[i]))
                dormantComponents.Add((MonoBehaviour)attachedDormantComponents[i]);
            }
        }
#endif 

        void Awake()
        {
            for (int i = 0; i < dormantComponents.Count; i++)
            {
                dormantComponents[i].enabled = false;
            }
        }

        public void Init(FireProjectileData projectileInfo)
        {
            ProjectileInfo = projectileInfo;

            for (int i = 0; i < subComponentsBehaviours.Count; i++)
            {
                Object subComponent = subComponentsBehaviours[i];
                ((IProjectileSubComponent)subComponent).ProjectileData = ProjectileInfo;
            }

            for (int i = 0; i < dormantComponents.Count; i++)
            {
                IProjectileDormant initializedComponent = dormantComponents[i] as IProjectileDormant;
                if (initializedComponent != null)
                {
                    initializedComponent.ProjectileWake();
                }
                dormantComponents[i].enabled = true;
            }
        }
    }
}