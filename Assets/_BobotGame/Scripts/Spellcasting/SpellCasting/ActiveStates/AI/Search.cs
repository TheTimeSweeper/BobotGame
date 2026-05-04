using UnityEngine;

namespace ActiveStates.AI
{
    public class Search : AIState
    {
        private float _searchTim = 1;

        public override void OnEnter()
        {
            base.OnEnter();

            RandomizeSearch();
        }

        private void RandomizeSearch()
        {
            _searchTim = Random.Range(Brain.searchIntervalMin, Brain.searchIntervalMax);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            _searchTim -= Time.fixedDeltaTime;
            if (_searchTim <= 0)
            {
                Brain.CurrentTargetBody = Brain.SearchForTarget();
                if(Brain.CurrentTargetBody != null)
                {
                    Machine.SetState(new ChaseToCombat { Brain = Brain, ChaseTime = Brain.chaseTimeMinimunm });
                    return;
                }

                RandomizeSearch();
            }
        }
    }
}
