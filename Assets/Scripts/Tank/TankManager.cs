using UnityEngine;

public class TankManager : MonoBehaviour
{
    public TankData TankData;
    public TankHead TankHead;
    public byte Health;

    void Awake()
    {
        if (TankHead == null)
            TankHead = GetComponentInChildren<TankHead>();
    }

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
