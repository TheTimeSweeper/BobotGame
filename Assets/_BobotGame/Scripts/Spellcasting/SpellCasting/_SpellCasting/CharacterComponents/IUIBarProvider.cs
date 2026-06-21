namespace SpellCasting
{
    public interface IUIBarProvider
    {
        float GetUICurrentValue();
        float GetUIMaxValue();
        bool GetUIShouldShow();
    }
    
    public interface IUIBehindBarProvider
    {
        float GetUIBehindCurrentValue();
        float GetUIBehindMaxValue();
    }
}