using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTankData", menuName = "Scriptable Objects/Tanks/Enemy Tank Data", order = 1)]
public class EnemyTankData : TankData
{
    #region Patrol Stats
        public float PatrolSpeed = 5f;
        public float PatrolRange = 5f;
    #endregion
}
