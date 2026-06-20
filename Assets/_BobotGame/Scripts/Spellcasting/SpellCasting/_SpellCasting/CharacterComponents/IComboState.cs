namespace SpellCasting
{
    public interface IComboState
    {
        int ComboHits { get; }
        int CurrentComboHit { get; set; }
    }
}