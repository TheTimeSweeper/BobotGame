using UnityEngine;

namespace SpellCasting
{
    //JAM shit I need hitboxgroups for multiple hitboxes god dam it
    //well I did it kinda
    //woops misnamed should probably be HitBox but I am in no way changing that and replacing all the components on every prefab that has a hitbox
    public abstract class Hitbox : MonoBehaviour
    {
        private bool isDebug;
        private void Awake()
        {
            if (gameObject.activeSelf) { isDebug = true; }
            gameObject.SetActive(false);
        }
        private void OnValidate()
        {
            if (transform.lossyScale.x < 0 || transform.lossyScale.y < 0 || transform.lossyScale.z < 0)
            {
                Debug.LogError("Hitbox scale should not be negative", this);
            }
        }

        public abstract Collider[] DoOverlap();

        public void DebugShow()
        {
            if (!isDebug)
                return;
            gameObject.SetActive(true);
            frames = 3;
        }

        public int frames { get; set; }
        private void FixedUpdate()
        {
            frames--;
            if(frames <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}