using ActiveStates.Characters;
using SpellCasting;
using SpellCasting.Projectiles;
using UnityEngine;

[CreateAssetMenu(fileName = "SIBobotGameDev", menuName = "BobotGame/BobotGameStateInfo")]
public class BobotGameDevStateInfo : ActiveStateInfo
{
    [Header("BobotPunchCombo")]
    public GenericTimedState.TimedStateParams BPC_HitParams = new GenericTimedState.TimedStateParams();
    public BasicMeleeAttack.BasicMeleeParams BPC_MeleeParams1 = new BasicMeleeAttack.BasicMeleeParams();
    [Space]
    public GenericTimedState.TimedStateParams BPC_HitParams2 = new GenericTimedState.TimedStateParams();
    public BasicMeleeAttack.BasicMeleeParams BPC_MeleeParams2 = new BasicMeleeAttack.BasicMeleeParams();
    [Space]
    public GenericTimedState.TimedStateParams BPC_HitParams3 = new GenericTimedState.TimedStateParams();
    public BasicMeleeAttack.BasicMeleeParams BPC_MeleeParams3 = new BasicMeleeAttack.BasicMeleeParams();
    [Header("Dash")]
    public GenericTimedState.TimedStateParams DASH_params = new GenericTimedState.TimedStateParams();
    public float Dash_DashSpeed;
    public AnimationCurve Dash_DashSpeedCurve;
    [Header("Stance")]
    public float Crouch_Height;
    public float block_duration;
    [ShowMultiplyResult("block_duration")]
    public float block_duration_blocking_fraction;
    [Header("Kick")]
    public GenericTimedState.TimedStateParams Kick_params = new GenericTimedState.TimedStateParams();
    public BasicMeleeAttack.BasicMeleeParams Kick_meleeParams = new BasicMeleeAttack.BasicMeleeParams();
    [Header("Deadlift punch")]
    public float CPunch_chargeTime;
    public float CPunch_holdGiveupTime;
    [Space]
    public GenericTimedState.TimedStateParams CPunch_ReleaseParams = new GenericTimedState.TimedStateParams();
    public BasicMeleeAttack.BasicMeleeParams CPunch_meleeReleaseParams = new BasicMeleeAttack.BasicMeleeParams();
    public BasicMeleeAttack.BasicMeleeParams CPunch_meleeReleaseParamsMaxCharge = new BasicMeleeAttack.BasicMeleeParams();
    public float CPunch_InstantPunchCharge = 0.2f;
    public float CPunch_damageMin;
    public float CPunch_damageMax;
    public float CPunch_knockbackMin;
    public float CPunch_knockbackMax;
    [Header("Deadlift grapple")]
    public ProjectileController Grap_Prefab;
    public float Grap_Speed;
}
