using UnityEngine;

public interface IEnemyBehavior
{
    Vector3 PlayerPosition { get; }

    void HandleStateLogic();
    void TransitionStates();
    void Patrol();
    void Chase();
    void Attack();
}
