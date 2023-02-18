using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const string PLAYER_TAG = "Player";
    const string ENEMY_TAG = "Enemy";
    const string BULLET_TAG = "Bullet";
    const string WALL_TAG = "Wall";
    
    public TankData tankData;
    Rigidbody _rigidbody;

    public GameObject TankParent;
    float _timeSinceSpawned;
    float _bulletSpeedInUnitsPerSecond;
    byte _bulletDamage;
    byte _maxRicochets;
    byte _currentRicochets;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _timeSinceSpawned = 0f;
        _bulletSpeedInUnitsPerSecond = tankData.BulletSpeedInUnitsPerSecond;
        _bulletDamage = tankData.BulletDamage;
        _maxRicochets = tankData.NumberOfRicochets;
        _currentRicochets = 0;
    }

    void Update()
    {
        if (_timeSinceSpawned > tankData.BulletLifeTimeInSeconds)
            DestroyBullet(gameObject);
        else
            _timeSinceSpawned += Time.deltaTime;
    }


    void DestroyBullet(GameObject bullet)
    {
        // TODO: Add particle effect
        Destroy(bullet.gameObject);
    }

    void FixedUpdate() => Move();
    
        
    // Since the bullet is a capsule that is rotated, its axes are also rotated.
    // Because of this, modifying transform.forward acts as transform.down.
    // We re-definine forward to be up, which will make the bullet move in the correct direction.
    void Move() => _rigidbody.velocity = transform.up * tankData.BulletSpeedInUnitsPerSecond;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ENEMY_TAG) || collision.gameObject.CompareTag(PLAYER_TAG))
        {
            collision.gameObject.GetComponent<TankManager>().TakeDamage(tankData.BulletDamage);
            DestroyBullet(gameObject);
        }

        if (collision.gameObject.CompareTag(WALL_TAG))
            Ricochet(collision);

        if (collision.gameObject.CompareTag(BULLET_TAG))
        {
            if (collision.gameObject.GetComponent<Bullet>().TankParent == TankParent)
                return;
                
            // NOTE: Potentially unsafe
            DestroyBullet(collision.gameObject);
            DestroyBullet(gameObject);
        }
    }

    void Ricochet(Collision collision)
    {
        if (_currentRicochets >= _maxRicochets)
        {
            DestroyBullet(gameObject);
            return;
        }

        Vector3 normal = collision.impulse.normalized;
        Vector3 direction = transform.up;
        Vector3 reflection = Vector3.Reflect(direction, normal);
        transform.up = reflection;

        _currentRicochets++;
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawRay(transform.position, transform.up * tankData.BulletSpeedInUnitsPerSecond);
    // }
}
