public class GreenEnemyTank : BaseEnemyStateMachine
{
    const float ANGLE_AIM_THRESHOLD_IN_DEGREES = 30;

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
        AttackIfWithinAngleThreshold(ANGLE_AIM_THRESHOLD_IN_DEGREES);
    }
}
