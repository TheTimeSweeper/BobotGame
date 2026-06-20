using System;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ActiveStates.Characters
{
    public abstract class BasicTimedState : BodyState
    {
        [System.Serializable]
        public class TimedStateParams
        {
            /// <summary>
            /// total duration of the move
            /// </summary>
            [ShowMultiplyResult(addToMultipliers = "baseExtraEndDelayFraction")]
            public float baseDuration = 1;
            /// <summary>
            /// easy way to extend the end time of an attack without having to affect previous timings
            /// </summary>
            public float baseExtraEndDelayFraction = 0;
            /// <summary>
            /// 0-1 time relative to duration that the skill starts
            /// for example, set 0.5 and the "cast" will happen halfway through the skill
            /// </summary>
            [ShowMultiplyResult("baseDuration")]
            public float baseCastStartTimeFraction = 1;
            [ShowMultiplyResult("baseDuration")]
            public float baseCastEndTimeFraction = 1;
            [ShowMultiplyResult("baseDuration", addToMultipliers = "baseExtraEndDelayFraction")]
            public float baseOtherStateInterruptTimeFraction = 1;
            [ShowMultiplyResult("baseDuration", addToMultipliers = "baseExtraEndDelayFraction")]
            public float baseMovementInterruptTimeFraction = 1;
            public bool attackSpeedAffected = true;

            public TimedStateParams() { }
            public TimedStateParams(float baseDuration)
            {
                this.baseDuration = baseDuration;
            }
            public TimedStateParams(float? overrideBaseDuration, float? overrideBaseCastStartTime, float? overrideBaseCastEndTime)
            {
                if (overrideBaseDuration.HasValue)
                {
                    baseDuration = overrideBaseDuration.Value;
                }
                if (overrideBaseCastStartTime.HasValue)
                {
                    baseCastStartTimeFraction = overrideBaseCastStartTime.Value;
                }
                if (overrideBaseCastEndTime.HasValue)
                {
                    baseCastEndTimeFraction = overrideBaseCastEndTime.Value;
                }
            }
        }

        public virtual float? simpleOverrideBaseDuration => 1;
        public virtual float? simpleOverrideBaseCastStartTimeFraction => null;
        public virtual float? simpleOverrideBaseCastEndTimeFraction => null;

        protected virtual TimedStateParams timedStateParams { get; private set; }

        protected float duration;
        protected float castStartTime;
        protected float castEndTime;
        protected float stateEndTime;
        protected float movementInterruptTime;
        protected float otherStateInterruptTime;
        protected bool hasCasted;
        protected bool isCasting;
        protected bool hasExited;

        public override void OnEnter()
        {
            InitDurationValues();
            base.OnEnter();
        }

        protected virtual void InitDurationValues()
        {
            if(timedStateParams == null)
            {
                timedStateParams = new TimedStateParams(simpleOverrideBaseDuration, simpleOverrideBaseCastStartTimeFraction, simpleOverrideBaseCastEndTimeFraction);
            }
            duration = timedStateParams.baseDuration / (timedStateParams.attackSpeedAffected? characterBody.stats.AttackSpeed : 1);
            castStartTime = timedStateParams.baseCastStartTimeFraction * duration;
            castEndTime = timedStateParams.baseCastEndTimeFraction * duration;
            otherStateInterruptTime = timedStateParams.baseOtherStateInterruptTimeFraction * duration * ( 1 + timedStateParams.baseExtraEndDelayFraction);
            movementInterruptTime = timedStateParams.baseMovementInterruptTimeFraction * duration * (1 + timedStateParams.baseExtraEndDelayFraction);
            stateEndTime = duration * (1 + timedStateParams.baseExtraEndDelayFraction);
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
            isCasting = false;

            //to guarantee attack comes out if at high attack speed the fixedage skips past the endtime
            if (fireStarted && !fireEnded || fireStarted && fireEnded && !hasCasted)
            {
                isCasting = true;
                OnCastFixedUpdate();
                if (!hasCasted)
                {
                    OnCastEnter();
                    hasCasted = true;
                }
            }

            if (fireEnded && !hasExited)
            {
                hasExited = true;
                OnCastExit();
            }

            if (fixedAge > stateEndTime)
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
            if (isCasting)
            {
                OnCastUpdate();
            }
        }
    }
}