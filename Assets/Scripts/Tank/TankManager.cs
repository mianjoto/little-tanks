using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManager : MonoBehaviour
{
    public TankData TankData;
    public byte Health;

    void Start()
    {
        transform.name = TankData.TankName;
        transform.tag = TankData.TankTag;
        Health = TankData.StartingHealth;
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
