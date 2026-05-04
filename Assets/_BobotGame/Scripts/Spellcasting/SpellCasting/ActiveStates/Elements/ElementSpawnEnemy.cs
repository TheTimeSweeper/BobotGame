using SpellCasting;
using UnityEngine;

namespace ActiveStates.Elements
{
    public abstract class ElementSpawnEnemy : BaseElementMassState
    {
        protected abstract CharacterBody minionPrefab { get; }
        protected virtual int amount => 1;

        //jam 
        public override void OnEnter()
        {
            base.OnEnter();

            for (int i = 0; i < elementMass.SubMasses.Count; i++)
            {
                CharacterBody enemigo = Object.Instantiate(minionPrefab, elementMass.SubMasses[i].transform.position, elementMass.SubMasses[i].transform.rotation);
                enemigo.CommonComponents.TeamComponent.TeamIndex = teamComponent.TeamIndex;
            }

            FizzleMass();
        }

        protected virtual void FizzleMass()
        {
            elementMass.Fizzle();
        }

    }
}