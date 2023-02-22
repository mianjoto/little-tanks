using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankManager), typeof(TankShoot), typeof(TankMovement))]
public class BeigeEnemyTank : BaseEnemyStateMachine
{
    Coroutine _lookAtRandomPointCoroutine;
    Coroutine _waitCoroutine;

    const float MIN_DUMB_TIME = 2f;
    const float MAX_DUMB_TIME = 7f;
    const float CHANCE_TO_PATROL = 0.5f;
    const float CHANCE_TO_ATTACK = 0.7f;
    
    [SerializeField] bool isLookingAtRandomPoint; 

    void Start()
    {
        _currentState = EnemyState.Patrol;
        if (_lookAtRandomPointCoroutine != null)
            StopCoroutine(_lookAtRandomPointCoroutine);
    }

    // Beige tanks can only patrol (look around) and attack at random
    public override void HandleStateLogic()
    {
        if (_currentState == EnemyState.Patrol)
            Patrol();
        else if (_currentState == EnemyState.Attack)
            base.Attack();
        else if (_currentState == EnemyState.Wait)
            Wait();
        
        TransitionStates();
    }

    public override void TransitionStates()
    {
        if (WantsToPatrol)
            _currentState = EnemyState.Patrol;
        else if (WantsToAttack)
            _currentState = EnemyState.Attack;
        else
            _currentState = EnemyState.Wait;
    }

    [SerializeField] bool WantsToPatrol => Random.value < CHANCE_TO_PATROL;
    [SerializeField] bool WantsToAttack => Random.value < CHANCE_TO_ATTACK;
    [SerializeField] bool IsBusy => _lookAtRandomPointCoroutine != null || _waitCoroutine != null;

    IEnumerator LookAtRandomPoint()
    {
        Vector3 randomPoint = _tankHead.LookAtRandomRelativePoint();

        while (!isLookingAtRandomPoint)
        {
            _tankHead.LookAtPoint(randomPoint, relative: true);
            isLookingAtRandomPoint = _tankHead.IsLookingAtPoint(randomPoint);
            yield return null;
        }

        _currentState = EnemyState.Wait;
    }

    public override void Patrol()
    {
        if (_lookAtRandomPointCoroutine != null)
            StopCoroutine(_lookAtRandomPointCoroutine);

        _lookAtRandomPointCoroutine = StartCoroutine(LookAtRandomPoint());
        Debug.Log("Patrol coroutine == null" + _lookAtRandomPointCoroutine == null);
    }

    void Wait()
    {
        if (_waitCoroutine != null)
            StopCoroutine(_waitCoroutine);

        _waitCoroutine = StartCoroutine(WaitForRandomTime());
    }

    IEnumerator WaitForRandomTime()
    {
        float randomWaitTimeInSeconds = Random.Range(MIN_DUMB_TIME, MAX_DUMB_TIME);
        Debug.Log("Waiting for " + randomWaitTimeInSeconds + " seconds, current state is: " + _currentState + ".");
        yield return new WaitForSeconds(randomWaitTimeInSeconds);
        Debug.Log("Done waiting");
    }
}
