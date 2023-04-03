using System;
using UnityEngine;

[RequireComponent(typeof(TankManager), typeof(TankShoot), typeof(TankMovement))]
public class BlueEnemyTank : BaseEnemyStateMachine
{
    // Blue tanks are stationary and lead their shots with high speed bullets
    void Start() => _currentState = EnemyState.Attack;

    public override void HandleStateLogic()
    {
        Attack();
    }
    
    public override void Attack()
    {
        LeadShotAndLookAtPlayer();
        base.Attack();
    }

    public override void TransitionStates() { }
    public override void Patrol() { }
}
