using System;
using UnityEngine;

[RequireComponent(typeof(TankManager), typeof(TankShoot), typeof(TankMovement))]
public class BasicEnemyTank : BaseEnemyStateMachine
{  
    void Start() => _currentState = EnemyState.Patrol;

    public override void HandleStateLogic()
    {
        switch (_currentState)
        {
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            default:
                Patrol();
                break;
        }

        TransitionStates();
    }

    public override void TransitionStates()
    {
        if (IsPlayerInDetectionRange())
        {
            _tankHead.LookAtPoint(_playerPosition);
            _tankMovement.RotateToPosition(_playerPosition);
            if (IsPlayerInAttackRange())
                _currentState = EnemyState.Attack;
            else
                _currentState = EnemyState.Chase;
        }
        else
            _currentState = EnemyState.Patrol;
    }

    public override void Chase() => _tankMovement.MoveForward();
    public override void Patrol()
    {
        // TODO: Implement patrol logic with waypoints
        // Debug.Log($"{_transform.name} is patrolling...");
    }
}
