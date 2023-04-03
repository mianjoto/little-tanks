public class GreenEnemyTank : BaseEnemyStateMachine
{

    // Green tanks track player when in range, otherwise look randomly
    public override void TransitionStates()
    {
        if (IsPlayerInAttackRange)
            _currentState = EnemyState.Attack;
        else
        {
            _currentState = EnemyState.Patrol;
            Wait();
        }
    }

    public override void Attack()
    {
        LookAtPlayer();
        AttackIfWithinAngleThreshold(_enemyTankData.LookAtAngleThresholdInDegrees);
    }
}
