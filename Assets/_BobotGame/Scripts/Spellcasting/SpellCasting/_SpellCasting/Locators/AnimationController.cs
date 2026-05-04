using UnityEngine;

namespace SpellCasting
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private InputBank inputBank;

        void Update()
        {
            animator.SetBool("Moving", inputBank.GlobalMoveDirection != Vector3.zero);
            animator.SetBool("Casting", inputBank.CurrentPrimaryInput != null);        
        }
    }
}