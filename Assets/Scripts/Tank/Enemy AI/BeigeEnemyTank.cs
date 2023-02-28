using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankManager), typeof(TankShoot), typeof(TankMovement))]
public class BeigeEnemyTank : BaseEnemyStateMachine
{
    Coroutine _lookAtRandomPointCoroutine;
    Coroutine _waitCoroutine;

    bool isPatrolling;
    bool _isWaiting;
    float _lastTimeChangedState;

    float _minimumWaitTime;
    float _maximumWaitTime;
    float _minimumHeadRotationAngle;
    float _stateChangeCooldown;

    const float CHANCE_TO_WAIT = 0.8f;

    protected override void Awake()
    {
        base.Awake();
        _minimumWaitTime = _enemyTankData.MinimumWaitTimeInSeconds;
        _maximumWaitTime = _enemyTankData.MaximumWaitTimeInSeconds;
        _minimumHeadRotationAngle = _enemyTankData.MinimumRandomHeadRotationAngle;
        _stateChangeCooldown = _enemyTankData.StateChangeCooldownInSeconds;
    }

    void Start()
    {
        _currentState = EnemyState.Patrol;
        _lastTimeChangedState = -100;
    }

    public override void HandleStateLogic()
    {
        if (isBusy || isOnStateChangeCooldown) return;

        if (_currentState == EnemyState.Patrol)
            Patrol();
        else if (_currentState == EnemyState.Attack)
            base.Attack();

        TransitionStates();
    }

    // Beige tanks look somewhere random and shoot with a high likelihood to wait between each action
    public override void TransitionStates()
    {
        if (shouldWait) Wait();

        if (_currentState == EnemyState.Patrol)
            _currentState = EnemyState.Attack;
        else if (_currentState == EnemyState.Attack)
            _currentState = EnemyState.Patrol;
    }

    bool isBusy => isPatrolling || _isWaiting;
    bool shouldWait => Random.value < CHANCE_TO_WAIT;
    bool isOnStateChangeCooldown => Time.time < _lastTimeChangedState + _stateChangeCooldown;
    
    public override void Patrol()
    {
        if (_lookAtRandomPointCoroutine != null)
            StopCoroutine(_lookAtRandomPointCoroutine);

        _lookAtRandomPointCoroutine = StartCoroutine(LookAtRandomPoint());
    }

    IEnumerator LookAtRandomPoint()
    {
        isPatrolling = true;
        bool isLookingAtRandomPoint = false;

        Vector3 randomPoint = _tankHead.GetRandomRelativePoint(_minimumHeadRotationAngle);
        while (!isLookingAtRandomPoint)
        {
            _tankHead.LookAtPoint(randomPoint, relative: true);
            isLookingAtRandomPoint = _tankHead.IsLookingAtPoint(randomPoint, relative: true);
            yield return null;
        }

        isPatrolling = false;

        _lastTimeChangedState = Time.time;
    }

    void Wait()
    {
        if (_waitCoroutine != null)
            StopCoroutine(_waitCoroutine);

        _waitCoroutine = StartCoroutine(WaitForRandomTime());
    }

    IEnumerator WaitForRandomTime()
    {
        _isWaiting = true;

        float randomWaitTimeInSeconds = Random.Range(_minimumWaitTime, _maximumWaitTime);
        yield return new WaitForSeconds(randomWaitTimeInSeconds);
        _isWaiting = false;

        _lastTimeChangedState = Time.time;
    }
}
