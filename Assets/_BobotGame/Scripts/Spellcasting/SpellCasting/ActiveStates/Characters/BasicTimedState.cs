using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ActiveStates.Characters
{
    public abstract class BasicTimedState : BodyState
    {
        //total duration of the move
        protected abstract float baseDuration { get; }
        //0-1 time relative to duration that the skill starts
        //for example, set 0.5 and the "cast" will happen halfway through the skill
        protected virtual float baseCastStartTimeFraction => 1;
        protected virtual float baseCastEndTimeFraction => 1;
        protected virtual float baseOtherStateInterruptTimeFraction => 1;
        protected virtual float baseMovementInterruptTimeFraction => 1;
        protected virtual bool attackSpeedAffected => true;

        protected float duration;
        protected float castStartTime;
        protected float castEndTime;
        protected float movementInterruptTime;
        protected float otherStateInterruptTime;
        protected bool hasFired;
        protected bool isFiring;
        protected bool hasExited;

        public override void OnEnter()
        {
            InitDurationValues();
            base.OnEnter();
        }

        protected virtual void InitDurationValues()
        {
            duration = baseDuration / (attackSpeedAffected? characterBody.stats.AttackSpeed : 1);
            castStartTime = baseCastStartTimeFraction * duration;
            castEndTime = baseCastEndTimeFraction * duration;
            otherStateInterruptTime = baseOtherStateInterruptTimeFraction * duration;
            movementInterruptTime = baseMovementInterruptTimeFraction * duration;
        }

        protected virtual void OnCastEnter() { }
        protected virtual void OnCastFixedUpdate() { }
        protected virtual void OnCastUpdate() { }
        protected virtual void OnCastExit() { }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            bool fireStarted = fixedAge >= castStartTime;
            bool fireEnded = fixedAge >= castEndTime;
            isFiring = false;

            //to guarantee attack comes out if at high attack speed the fixedage skips past the endtime
            if (fireStarted && !fireEnded || fireStarted && fireEnded && !hasFired)
            {
                isFiring = true;
                OnCastFixedUpdate();
                if (!hasFired)
                {
                    OnCastEnter();
                    hasFired = true;
                }
            }

            if (fireEnded && !hasExited)
            {
                hasExited = true;
                OnCastExit();
            }

            if (fixedAge > duration)
            {
                SetNextState();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (fixedAge >= movementInterruptTime)
            {
                return InterruptPriority.MOVEMENT;
            }
            if (fixedAge >= otherStateInterruptTime)
            {
                return InterruptPriority.STATE_ANY;
            }
            return InterruptPriority.STATE_LOW;
        }

        protected virtual void SetNextState()
        {
            Machine.SetStateToDefault();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isFiring)
            {
                OnCastUpdate();
            }
        }
    }
}