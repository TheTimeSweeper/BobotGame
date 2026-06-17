using SpellCasting;
using SpellCasting.Projectiles;
using System;

namespace ActiveStates.Bobots
{
    public class DeadliftGrapple : ActiveState, IHasStateInfo<BobotGameDevStateInfo>, IStateFromInput
    {
        public ActiveStateInfo AssignedStateInfo { get; set; }
        public Type StateInfoType => typeof(BobotGameDevStateInfo);
        public BobotGameDevStateInfo StateInfo => AssignedStateInfo as BobotGameDevStateInfo;

        public InputState input { get; set; }

        private ProjectileController projectile;

        public override void OnEnter()
        {
            base.OnEnter();
            var projectileInfo = new FireProjectileData
            {
                AimDirection = inputBank.AimOut,
                OwnerObject = gameObject,
                OwnerBody = characterBody,
                StartPosition = characterBody.corePosition
            };
            projectile = UnityEngine.Object.Instantiate<ProjectileController>(StateInfo.Grap_Prefab);
            projectile.Init(projectileInfo);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (input != null && !input.Down)
            {
                EndState();
            }
        }

        public override void OnExit(bool machineDed = false)
        {
            base.OnExit(machineDed);
            if (projectile)
            {
                UnityEngine.Object.Destroy(projectile.gameObject);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.STATE_MED;
        }
    }
}
