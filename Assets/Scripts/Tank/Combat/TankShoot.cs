using UnityEngine;

[RequireComponent(typeof(TankManager))]
public class TankShoot : MonoBehaviour
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] Transform headTransform;
    TankManager _tankManager;

    TankData _tankData;

    float _lastTimeShot = 0f;
    GameObject _bulletPrefab;

    void Awake()
    {
        _tankManager = GetComponent<TankManager>();
       _tankData = _tankManager.TankData;
       _bulletPrefab = _tankData.BulletPrefab;
    }

    public void Shoot()
    {
        if (!CheckIfCanShoot())
        {
            // TODO: Add a cooldown indication
            Debug.Log($"{transform.name} cannot shoot, on cooldown!");
            return;
        }
        
        InstantiateBullet();
    }

    bool CheckIfCanShoot()
    {
        if (_lastTimeShot + _tankData.ShootCooldownInSeconds <= Time.time)
            return true;
        else
            return false;
    }
        
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
