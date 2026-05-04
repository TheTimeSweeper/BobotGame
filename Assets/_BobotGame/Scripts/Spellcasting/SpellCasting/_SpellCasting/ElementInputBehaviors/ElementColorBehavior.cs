namespace SpellCasting
{
    public class ElementColorBehavior : ElementInputBehavior
    {
        public ElementColorBehavior(CommonComponentsHolder commonComponents_) : base(commonComponents_) { }

        private ElementType _lastElement;

        public override void Update()
        {
            if (commonComponents.CharacterModel == null)
                return;

            if (_lastElement != currentCastingElement)
            {
                if (_lastElement != null)
                {
                    commonComponents.CharacterModel.RemoveElementColor(_lastElement.ElementColor);
                }

                _lastElement = currentCastingElement;

                if (currentCastingElement != null)
                {
                    commonComponents.CharacterModel.SetElementColor(currentCastingElement.ElementColor);
                }
            }
        }
    }
}