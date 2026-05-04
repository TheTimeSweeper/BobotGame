using UnityEngine;

namespace SpellCasting
{
    [System.Serializable]
    public class RandomMaterialSet
    {
        public Material material;
        public float[] normalValues;
        public float[] tilingValues;
    }

    [CreateAssetMenu(menuName = "SpellCasting/ViewRandomizerInfo", fileName ="RandomView")]
    public class ViewRandomizerInfo : ScriptableObject
    {
        [SerializeField]
        private RandomMaterialSet[] materialSets;

        [SerializeField]
        private float yRatio = 1;

        [SerializeField]
        private Color[] colors = new Color[] { Color.white};

        public void SetStuff(Renderer renderer)
        {
            RandomMaterialSet set = materialSets.GetRandom();
            renderer.sharedMaterial = set.material;

            float randomScale = set.tilingValues.GetRandom();
            renderer.material.mainTextureScale = new Vector2(randomScale, randomScale * yRatio);

            renderer.material.SetFloat("_BumpScale", set.normalValues.GetRandom());

            renderer.material.color = colors.GetRandom();
        }
    }
}