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
    public float BulletSpeedInUnitsPerSecond = 1f;
    public byte MaxBulletsInReserve = 4;
    public float ShootDelayInSeconds = 0.1f;
    public float ReloadAfterShootingDelayInSeconds = 1f;
    public float PerBulletReloadTimeInSeconds = 1.5f;
    public float BulletLifeTimeInSeconds = 5;
    public byte BulletDamage = 1;
    public byte NumberOfRicochets = 1;
    #endregion

    #region Movement Stats
    public float MovementSpeed = 1f;
    public float BodyRotateSpeed = 1f;
    #endregion

    #region Head Rotation Stats
    public bool HeadFollowsCursor = false;
    public bool RotatesHeadSmoothly = true;
    public float HeadRotationDamping = 10f;
    public float HeadRotationSpeed = 10;
    public byte LookAtAngleThresholdInDegrees = 20;
    #endregion
}
