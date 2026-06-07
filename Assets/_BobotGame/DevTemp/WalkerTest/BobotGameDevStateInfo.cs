using SpellCasting;
using UnityEngine;

[CreateAssetMenu(fileName = "SIBobotGameDev", menuName = "BobotGame/BobotGameStateInfo")]
public class BobotGameDevStateInfo : ActiveStateInfo
{
    [Header("BobotPUnchCombo")]
    public float BPC_Damage = 1;
    public float BPC_StartTimeFraction;
    public float BPC_EndTimeFraction;
    public float BPC_Duration;
    public float BPC_OtherStateInterruptTimeFraction;
    public float BPC_AnimationSpeed;
    public float BPC_positionShift;
    public float BPC_baseMovementInterruptTimeFraction;
    public float Dash_StartTimeFraction;
    public float Dash_Duration;
    public float Dash_AnimationSpeed;
    public float Dash_DashTime;
    public float Crouch_Height;
}
