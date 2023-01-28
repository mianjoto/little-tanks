using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "Scriptable Objects/Tanks/Tank Data", order = 1)]
public class TankData : ScriptableObject
{
    #region Tank Stats
        public string TankName = "Tank";
        public string TankTag = "Tank";
        public byte StartingHealth = 3;
    #endregion

    #region Shooting Stats
        public GameObject BulletPrefab;
        public float ShootCooldownInSeconds = 1f;
        public float BulletSpeedInUnitsPerSecond = 1f;
        public float BulletLifeTimeInSeconds = 5;
        public byte BulletDamage = 1;
        public byte NumberOfRicochets = 1;
    #endregion
}
