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
    public float BPC_DashTime;
    public float BPC_AnimationSpeed;
}
