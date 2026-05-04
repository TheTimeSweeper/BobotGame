using ActiveStates;
using UnityEngine;

namespace SpellCasting.AI
{
    public class DemonstratorBrain : AIBrain
    {
        [SerializeField]
        private CharacterBody targetBody;

        private float tim = 0.1f;

        private CharacterBody _nearPlayerBody;

        protected override void FixedUpdate()
        {
            tim -= Time.fixedDeltaTime;
            if(tim <= 0)
            {
                tim += 0.2f;
                _nearPlayerBody = CharacterBodyTracker.FindBodyBySqrDistance(TeamIndex.PLAYER, transform.position, 25*25);
            }

            if (_nearPlayerBody == null)
            {
                aiStateMachine.SetState(new IdleState());
                return;
            }

            base.FixedUpdate();
        }

        public override CharacterBody SearchForTarget()
        {
            return targetBody;
        }
    }
}
