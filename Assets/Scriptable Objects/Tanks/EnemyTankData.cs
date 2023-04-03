using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTankData", menuName = "Scriptable Objects/Tanks/Enemy Tank Data", order = 1)]
public class EnemyTankData : TankData
{
    #region Enemy State Stats
    public float StateChangeCooldownInSeconds = 1;
    public float MinimumWaitTimeInSeconds = 1f;
    public float MaximumWaitTimeInSeconds = 3f;
    public float MinimumRandomHeadRotationAngle = 60f;
    public float PlayerDetectionRange = 10f;
    public float AttackRange = 5f;
    public float LeadShotFactorInUnits = 3f;
    public bool ShouldInterruptCurrentStateOnDetectRangeEnter = false;
    public bool ShouldInterruptCurrentStateOnAttackRangeEnter = false;
    public bool IsAlwaysLookingAtPlayer = false;
    public bool IsAlwaysRotatingTowardPlayer = false;
    public bool IsAlwaysChasingPlayer = false;
    public bool IsAlwaysAttackingAtPlayer = false;
    public bool ShouldBodyRotateTowardHeadDirection = false;
    #endregion
}
