using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankManager), typeof(TankShoot), typeof(TankMovement))]
public class BeigeEnemyTank : BaseEnemyStateMachine
{
    const float CHANCE_TO_WAIT = 0.8f;

    // Beige tanks look somewhere random and shoot with a high likelihood to wait between each action
    public override void TransitionStates()
    {
        if (shouldWait) Wait();

        if (_currentState == EnemyState.Patrol)
            _currentState = EnemyState.Attack;
        else if (_currentState == EnemyState.Attack)
            _currentState = EnemyState.Patrol;
    }

    bool shouldWait => Random.value < CHANCE_TO_WAIT;
}
