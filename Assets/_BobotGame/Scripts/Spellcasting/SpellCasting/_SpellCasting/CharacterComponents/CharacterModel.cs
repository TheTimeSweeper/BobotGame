using System;
using System.Collections.Generic;
using UnityEngine;
namespace SpellCasting
{
    public class CharacterModel : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] elementColoredRenderers;

        [SerializeField]
        private ParticleSystem[] elementColoredParticles;

        [SerializeField]
        private Color noElementColor = Color.gray;

        [SerializeField]
        private CharacterDirection characterDirection;
        public CharacterDirection CharacterDirection { get => characterDirection; }

        [SerializeField]
        private ChildTransformLocator childLocator;
        public ChildTransformLocator ChildLocator { get => childLocator; }

        [SerializeField]
        private HitboxLocator hitboxLocator;
        public HitboxLocator HitboxLocator { get => hitboxLocator; }

        [SerializeField]
        public ParticleSystemLocator particleSystemLocator;

        private MaterialPropertyBlock[] _materialBlocks;
        private ParticleSystem.MainModule[] _particleMains;

        private List<Color> _currentColors = new List<Color>();

        private void Awake()
        {
            _materialBlocks = new MaterialPropertyBlock[elementColoredRenderers.Length];
            for (int i = 0; i < elementColoredRenderers.Length; i++) {
                _materialBlocks[i] = new MaterialPropertyBlock();
                elementColoredRenderers[i].GetPropertyBlock(_materialBlocks[i]);
            }

            _particleMains = new ParticleSystem.MainModule[elementColoredParticles.Length];
            for (int i = 0; i < elementColoredParticles.Length; i++)
            {
                _particleMains[i] = elementColoredParticles[i].main;
            }
            UpdateElementColors();
        }

        public void UpdateElementColors()
        {
            Color currentColor = _currentColors.Count > 0 ? _currentColors[0] : noElementColor;

            for (int i = 0; i < elementColoredRenderers.Length && i < _materialBlocks.Length; i++)
            {
                _materialBlocks[i].SetColor("_Color", currentColor);
                elementColoredRenderers[i].SetPropertyBlock(_materialBlocks[i]);
            }

            for (int i = 0; i < _particleMains.Length; i++)
            {
                _particleMains[i].startColor = currentColor;
            }
        }
        public void SetElementColor(Color newColor)
        {
            _currentColors.Insert(0, newColor);
            UpdateElementColors();
        }
        public void RemoveElementColor(Color removingColor)
        {
            for (int i = 0; i < _currentColors.Count; i++)
            {
                if(_currentColors[i] == removingColor)
                {
                    _currentColors.RemoveAt(i);
                    break;
                }
            }
            UpdateElementColors();
        }
    }
}