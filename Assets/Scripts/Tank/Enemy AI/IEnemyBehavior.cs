using UnityEngine;

public interface IEnemyBehavior
{
    Vector3 GetPlayerPositionInWorld();
    void HandleStateLogic();
    void TransitionStates();
    void Patrol();
    void Chase();
    void Attack();
}
