using ActiveStates.Characters;
using SpellCasting;
using SpellCasting.Projectiles;
using UnityEngine;

[CreateAssetMenu(fileName = "SIBobotGameDev", menuName = "BobotGame/BobotGameStateInfo")]
public class BobotGameDevStateInfo : ActiveStateInfo
{
    [Header("BobotPunchCombo")]
    public BasicMeleeAttack.BasicMeleeParams BPC_HitParams = new BasicMeleeAttack.BasicMeleeParams();
    public BasicMeleeAttack.BasicMeleeParams BPC_HitParams2 = new BasicMeleeAttack.BasicMeleeParams();
    public BasicMeleeAttack.BasicMeleeParams BPC_HitParams3 = new BasicMeleeAttack.BasicMeleeParams();

    public float BPC_AnimationSpeed;
    [Header("Dash")]

    public BasicTimedState.TimedStateParams DASH_params = new BasicTimedState.TimedStateParams();
    public float Dash_AnimationSpeed;
    public float Dash_DashSpeed;
    public AnimationCurve Dash_DashSpeedCurve;
    [Header("Stance")]
    public float Crouch_Height;
    public float block_duration;
    public float block_AnimationMultiplier;
    [Header("Kick")]
    public BasicMeleeAttack.BasicMeleeParams Kick_params = new BasicMeleeAttack.BasicMeleeParams();
    public float Kick_AnimationSpeed;
    [Header("Deadlift punch")]
    public float CPunch_chargeTime;
    public float CPunch_holdGiveupTime;
    public float CPunch_StartTimeFraction;
    public float CPunch_EndTimeFraction;
    public float CPunch_Duration;
    public float CPunch_damageMin;
    public float CPunch_damageMax;
    public float CPunch_knockbackMin;
    public float CPunch_knockbackMax;
    [Header("Deadlift grapple")]
    public ProjectileController Grap_Prefab;
    public float Grap_Speed;
}
