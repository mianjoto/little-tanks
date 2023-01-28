using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public TankData tankData;
    
    float _timeSinceSpawned = 0f;

    void Update()
    {
        if (_timeSinceSpawned > tankData.BulletLifeTimeInSeconds)
            BulletDestroy();
        else
            _timeSinceSpawned += Time.deltaTime;
    }


    void BulletDestroy()
    {
        // TODO: Add particle effect
        Destroy(gameObject);
    }

    void FixedUpdate() => Move();
    
        
    // Since the bullet is a capsule that is rotated, its axes are also rotated.
    // Because of this, modifying transform.forward acts as transform.down.
    // We re-definine forward to be up, which will make the bullet move in the correct direction.
    void Move() => transform.position += transform.up * tankData.BulletSpeedInUnitsPerSecond * Time.deltaTime;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tank"))
        {
            collision.gameObject.GetComponent<TankManager>().TakeDamage(tankData.BulletDamage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
            Ricochet();
    }

    void Ricochet()
    {
        // TODO: Implement ricochet logic 
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * tankData.BulletSpeedInUnitsPerSecond);
    }
}
