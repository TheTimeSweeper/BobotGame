using SpellCasting;

namespace ActiveStates.Elements.Earth
{
    public class EarthSummon : ElementSpawnEnemy
    {
        protected override CharacterBody minionPrefab => ((ElementTypeEnemyEarth)elementType).MinionPrefab;
        protected override int amount => 3;

        public override void OnEnter()
        {
            base.OnEnter();

            EffectManager.SpawnEffect(EffectIndex.SOUND, transform.position, null, 14);
        }
    }
}