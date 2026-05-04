
using SpellCasting;

namespace ActiveStates.Elements.Metal
{
    public class MetalSwipe : ElementThrowProjectile
    {
        protected override DamageTypeIndex GetDamageType()
        {
            return DamageTypeIndex.BIGSTUN;
        }
    }
}