using UnityEngine;

namespace SpellCasting
{
    public class ViewRandomizer :MonoBehaviour
    {
        [SerializeField]
        private Renderer randomizedRenderer;


        [SerializeField]
        private ViewRandomizerInfo randomInfo;

        void Reset()
        {
            randomizedRenderer = GetComponent<Renderer>();
        }

        void Awake()
        {
            randomInfo.SetStuff(randomizedRenderer);   
        }
    }
}