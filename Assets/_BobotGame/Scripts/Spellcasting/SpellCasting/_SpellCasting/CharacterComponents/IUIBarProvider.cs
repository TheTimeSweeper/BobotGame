namespace SpellCasting
{
    public interface IUIBarProvider
    {
        float GetUICurrentValue();
        float GetUIMaxValue();
        bool GetUIShouldShow();
    }
}