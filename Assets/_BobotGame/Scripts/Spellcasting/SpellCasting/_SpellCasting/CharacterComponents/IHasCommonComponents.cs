namespace SpellCasting
{
    public interface IHasCommonComponents
    {
        CommonComponentsHolder CommonComponents { get; }
    }
    public interface ILabeled
    {
        string Label { get; }
    }
}