using UnityEngine;

[RequireComponent(typeof(TankManager))]
public class TankShoot : MonoBehaviour
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Transform headTransform;
    TankManager _tankManager;
    TankAmmoReserve _tankAmmoReserve;
    AudioClip _shotAudioClip;
    AudioOptions _shotAudioOptions;

    TankData _tankData;

    float _lastTimeShot = 0f;
    GameObject _bulletPrefab;

    void Awake()
    {
        _tankManager = GetComponent<TankManager>();
       _tankData = _tankManager.TankData;
       _bulletPrefab = _tankData.BulletPrefab;
        _tankAmmoReserve = new TankAmmoReserve(_tankData);

        _shotAudioClip = _tankData.ShotAudioClip;
        _shotAudioOptions = _tankData.ShotAudioOptions;
    }

    public void Shoot() 
    {
        if (!HasAmmoAndCanShoot)
            return;
        
        InstantiateBullet();
        AudioManager.Instance.PlaySound(_shotAudioClip, _shotAudioOptions);
    }

    bool HasAmmoAndCanShoot => _tankAmmoReserve.HasAmmo && CanShootAfterShootDelay;
    bool CanShootAfterShootDelay => Time.time > _lastTimeShot + _tankData.ShootDelayInSeconds;

    void InstantiateBullet()
    {
        GameObject bullet = Instantiate(original: _bulletPrefab);
        SetBulletPositionAndRotation(bullet);
        bullet.GetComponent<Bullet>().tankData = _tankData;
        bullet.GetComponent<Bullet>().TankParent = _tankManager.gameObject;
        _lastTimeShot = Time.time;
    }

    void SetBulletPositionAndRotation(GameObject bullet)
    {
        bullet.transform.position = bulletSpawnPoint.position;
        float angle = headTransform.eulerAngles.y;
        bullet.transform.eulerAngles = new Vector3(bullet.transform.eulerAngles.x, angle, bullet.transform.eulerAngles.z);
    }

}
