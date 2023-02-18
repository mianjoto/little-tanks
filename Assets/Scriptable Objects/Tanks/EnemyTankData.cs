using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTankData", menuName = "Scriptable Objects/Tanks/Enemy Tank Data", order = 1)]
public class EnemyTankData : TankData
{
    #region Enemy State Stats
        public float PlayerDetectionRange = 10f;
        public float AttackRange = 5f;
        public float LeadShotFactorInUnits = 3f;
        public bool IsAlwaysLookingAtPlayer = false;
        public bool IsAlwaysRotatingTowardPlayer = false;
        public bool IsAlwaysChasingPlayer = false;
        public bool IsAlwaysAttackingAtPlayer = false;
    #endregion
}
