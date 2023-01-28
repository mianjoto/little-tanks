using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    public TankData tankData;
    public byte Health;

    void Start()
    {
        transform.name = tankData.TankName;
        transform.tag = tankData.TankTag;
        Health = tankData.StartingHealth;
    }

    public void TakeDamage(byte damage)
    {
        Health -= damage;
        if (Health <= 0)
            Die();
    }

    void Die()
    {
        // TODO: Add particle effect
        Destroy(gameObject);
    }
}
