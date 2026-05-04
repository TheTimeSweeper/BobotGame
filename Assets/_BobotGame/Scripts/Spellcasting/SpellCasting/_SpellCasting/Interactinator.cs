using UnityEngine;

namespace SpellCasting
{

    public class Interactinator : MonoBehaviour
    {
        [SerializeField]
        private InteractionHandler[] interactionListeners;

        [SerializeField]
        protected TeamIndex SearchTeam = TeamIndex.PLAYER;

        [SerializeField]
        private float SearchInterval = 0.2f;
        [SerializeField]
        private float SearchRange = 10;

        [Space]
        [SerializeField]
        private GameObject interactableIndicator;

        private bool interacted;

        private float _timer = 0.2f;

        private CharacterBody Body;

        private void FixedUpdate()
        {
            _timer -= Time.deltaTime;

            if (_timer < 0)
            {
                _timer = SearchInterval;
                CheckInteractable();
            }

            if (Body != null)
            {
                for (int i = 0; i < interactionListeners.Length; i++)
                {
                    interacted |= interactionListeners[i].OnBodyDetected(Body, Body.CommonComponents.InputBank.E.JustPressed(this));
                }

                if (interactableIndicator != null)
                {
                    interactableIndicator.gameObject.SetActive(Body != null && !interacted);
                }
            }
        }

        private void CheckInteractable()
        {
            Body = CharacterBodyTracker.FindBodyBySqrDistance(SearchTeam, transform.position, SearchRange * SearchRange);
        }
    }
}