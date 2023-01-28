using UnityEngine;

public class TankShoot : MonoBehaviour
{
    [SerializeField] InputListener inputListener;
    [SerializeField] TankManager tankManager;
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Transform headOrientation;

    TankData _tankData;

    float _lastTimeShot = 0f;
    GameObject _bulletPrefab;
    
    void OnEnable()
    {
        inputListener.OnShoot += TryToShoot;
    }

    void OnDisable()
    {
        inputListener.OnShoot -= TryToShoot;
    }

    void Awake()
    {
       _tankData = tankManager.tankData;
       _bulletPrefab = _tankData.BulletPrefab;
    }

    void TryToShoot()
    {
        if (!CheckIfCanShoot())
            return;
        
        Shoot();
    }

    bool CheckIfCanShoot()
    {
        if (_lastTimeShot + _tankData.ShootCooldownInSeconds <= Time.time)
            return true;
        else
            return false;
    }
        
    void Shoot()
    {
        GameObject bullet = Instantiate(original: _bulletPrefab);
        SetBulletPositionAndRotation(bullet);
        bullet.GetComponent<Bullet>().tankData = _tankData;
        bullet.GetComponent<Bullet>().TankParent = tankManager.gameObject;
        _lastTimeShot = Time.time;
    }

    void SetBulletPositionAndRotation(GameObject bullet)
    {
        bullet.transform.position = bulletSpawnPoint.position;
        float angle = headOrientation.eulerAngles.y;
        bullet.transform.eulerAngles = new Vector3(bullet.transform.eulerAngles.x, angle, bullet.transform.eulerAngles.z);
    }

}
