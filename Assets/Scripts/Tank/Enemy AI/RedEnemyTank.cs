using System;
using UnityEngine;

[RequireComponent(typeof(TankManager), typeof(TankShoot), typeof(TankMovement))]
public class RedEnemyTank : BaseEnemyStateMachine
{
    void Start() => _currentState = EnemyState.Attack;

    public override void HandleStateLogic()
    {
        Attack();
    }
    
    public override void Attack()
    {    
        _tankHead.LookAtPoint(_playerPosition);
        _tankMovement.RotateToPosition(_playerPosition);
        base.Chase();
        base.Attack();
    }

    public override void TransitionStates() { }
    public override void Patrol() { }
}
