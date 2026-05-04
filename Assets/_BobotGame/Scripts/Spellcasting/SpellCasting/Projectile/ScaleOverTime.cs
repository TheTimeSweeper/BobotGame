using UnityEngine;

namespace SpellCasting.Projectiles
{
    public class ScaleOverTime : MonoBehaviour
    {
        [SerializeField]
        private float time = 0.3f;

        [SerializeField]
        private AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);

        [SerializeField]
        private Transform targetTransform;

        private float _lerpTim;
        private Vector3 _originalScale;

        void Reset()
        {
            targetTransform = transform;
        }

        void Start()
        {
            if (targetTransform == null)
            {
                targetTransform = transform;
            }
            //starting at 0 idk why
            _originalScale = targetTransform.localScale;
            targetTransform.localScale = _originalScale * curve.Evaluate(0);
        }

        void FixedUpdate()
        {
            _lerpTim += Time.deltaTime / time;

            targetTransform.localScale = _originalScale * curve.Evaluate(_lerpTim);
        }
    }
}