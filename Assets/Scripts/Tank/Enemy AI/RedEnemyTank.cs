using UnityEngine;

public class BasicEnemyTank : MonoBehaviour, IEnemyTankAI
{
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform tankManager;

    void Update()
    {
        
    }

    public void Attack()
    {
    }

    public void Chase()
    {
        throw new System.NotImplementedException();
    }

    public void Patrol()
    {
        throw new System.NotImplementedException();
    }
}
