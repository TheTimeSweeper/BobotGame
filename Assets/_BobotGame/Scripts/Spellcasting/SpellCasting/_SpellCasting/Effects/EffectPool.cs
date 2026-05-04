using System.Collections.Generic;
namespace SpellCasting
{
    public class EffectPool
    {
        public int Count => _effects.Count;
        private Queue<EffectPooled> _effects= new Queue<EffectPooled>();

        public EffectPooled RentItem()
        {
            return _effects.Dequeue();
        }
        public void ReturnItem(EffectPooled item)
        {
            _effects.Enqueue(item.EndEffect());
        }
    }
}