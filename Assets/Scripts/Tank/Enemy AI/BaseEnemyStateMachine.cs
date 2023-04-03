using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TankManager), typeof(TankShoot), typeof(TankMovement))]
public abstract class BaseEnemyStateMachine : MonoBehaviour, IEnemyBehavior
{
    #region Required Components
    protected TankManager _tankManager;
    protected TankShoot _tankShoot;
    protected TankMovement _tankMovement;
    #endregion

    #region Cached Components
    protected Transform _transform;
    protected TankHead _tankHead;
    protected EnemyTankData _enemyTankData;
    #endregion

    #region Player Properties
    protected const string PLAYER_TAG = "Player";
    protected Transform _playerTransform;
    protected Rigidbody _playerRigidbody;
    protected Vector3 _playerPosition;
    #endregion

    #region Enemy State Properties
    protected EnemyState _currentState;
    protected float _playerDetectionRange;
    protected float _attackRange;
    protected float _leadShotFactorInUnits;
    protected float _minimumHeadRotationAngle;
    protected float _stateChangeCooldown;
    protected float _minimumWaitTime;
    protected float _maximumWaitTime;
    #endregion

    #region Enemy State Information
    protected float _lastTimeChangedState;
    protected bool _isPatrolling; // naive fix
    protected bool _isWaiting; // naive fix
    protected Coroutine _lookAtRandomPointCoroutine;
    protected Coroutine _waitCoroutine;
    #endregion

    #region Persistent State Properties
    protected bool _hasAtLeastOnePersistentStateProperty;
    protected bool _isAlwaysLookingAtPlayer;
    protected bool _isAlwaysRotatingTowardPlayer;
    protected bool _isAlwaysChasingPlayer;
    protected bool _isAlwaysAttackingAtPlayer;
    protected bool _shouldInterruptCurrentStateOnAttackRangeEnter;
    protected bool _shouldInterruptCurrentStateOnDetectRangeEnter;
    protected bool _shouldBodyRotateTowardHeadDirection;
    #endregion

    protected virtual void Awake()
    {
        // Assign required components
        _tankManager = GetComponent<TankManager>();
        _tankShoot = GetComponent<TankShoot>();
        _tankMovement = GetComponent<TankMovement>();
        _transform = transform;
        _tankHead = _tankManager.TankHead;
        _enemyTankData = (EnemyTankData)_tankManager.TankData;
        _playerDetectionRange = _enemyTankData.PlayerDetectionRange;
        _attackRange = _enemyTankData.AttackRange;
        _leadShotFactorInUnits = _enemyTankData.LeadShotFactorInUnits;
        _minimumHeadRotationAngle = _enemyTankData.MinimumRandomHeadRotationAngle;
        _stateChangeCooldown = _enemyTankData.StateChangeCooldownInSeconds;
        _minimumWaitTime = _enemyTankData.MinimumWaitTimeInSeconds;
        _maximumWaitTime = _enemyTankData.MaximumWaitTimeInSeconds;
        _isAlwaysLookingAtPlayer = _enemyTankData.IsAlwaysLookingAtPlayer;
        _isAlwaysRotatingTowardPlayer = _enemyTankData.IsAlwaysRotatingTowardPlayer;
        _isAlwaysChasingPlayer = _enemyTankData.IsAlwaysChasingPlayer;
        _isAlwaysAttackingAtPlayer = _enemyTankData.IsAlwaysAttackingAtPlayer;
        _shouldInterruptCurrentStateOnAttackRangeEnter = _enemyTankData.ShouldInterruptCurrentStateOnAttackRangeEnter;
        _shouldInterruptCurrentStateOnDetectRangeEnter = _enemyTankData.ShouldInterruptCurrentStateOnDetectRangeEnter;
        _shouldBodyRotateTowardHeadDirection = _enemyTankData.ShouldBodyRotateTowardHeadDirection;
        FindPlayerTransformInScene();
    }

    void Start()
    {
        _currentState = EnemyState.Patrol;
        _lastTimeChangedState = -100;

        if (_lookAtRandomPointCoroutine != null) StopCoroutine(_lookAtRandomPointCoroutine);
        if (_waitCoroutine != null) StopCoroutine(_waitCoroutine);
    }

    protected virtual void Update()
    {
        _playerPosition = PlayerPosition;
        HandleStateLogic();

        if (HasAtLeastOnePersistentStateProperty)
            HandlePersistentStateBehaviors();
    }

    #region Base State Machine
    public virtual void HandleStateLogic()
    {
        if (IsInterrupted)
        {
            HandleInterrupt();
            return;
        }
        
        if (IsBusy || IsOnStateChangeCooldown) return;

        if (_currentState == EnemyState.Patrol)
            Patrol();
        else if (_currentState == EnemyState.Chase)
            Chase();
        else if (_currentState == EnemyState.Attack)
            Attack();
        else if (_currentState == EnemyState.Wait)
            Wait();
        
        TransitionStates();
    }

    protected virtual void HandleInterrupt()
    {
        if (_shouldInterruptCurrentStateOnDetectRangeEnter && IsPlayerInDetectionRange)
            Chase();
        else if (_shouldInterruptCurrentStateOnAttackRangeEnter && IsPlayerInAttackRange)
            Attack();

        TransitionStates();
    }

    public abstract void TransitionStates();

    public virtual void Patrol()
    {
        if (_lookAtRandomPointCoroutine != null)
            StopCoroutine(_lookAtRandomPointCoroutine);

        _lookAtRandomPointCoroutine = StartCoroutine(LookAtRandomPoint());
    }

    public virtual void Chase()
    {
        _tankMovement.RotateBodyToPosition(_playerPosition);
        _tankMovement.MoveBodyForward();
    }

    public virtual void Attack() => _tankShoot.Shoot();

    public virtual void Wait()
    {
        if (_waitCoroutine != null)
            StopCoroutine(_waitCoroutine);

        _waitCoroutine = StartCoroutine(WaitForRandomTime());
    }

    private void HandlePersistentStateBehaviors()
    {
        if (_isAlwaysAttackingAtPlayer)
            Attack();
        else if (_isAlwaysChasingPlayer)
            Chase();
        else if (_isAlwaysLookingAtPlayer)
            LookAtPlayer();
        else if (_isAlwaysRotatingTowardPlayer)
            _tankMovement.RotateBodyToPosition(_playerPosition);
        else if (_shouldBodyRotateTowardHeadDirection)
        {
            var direction = _transform.position - _playerPosition;
            _tankMovement.RotateBodyToPosition(direction);
            print($"direction={direction}");
        }
    }
    #endregion

    #region Getters
    protected virtual void FindPlayerTransformInScene()
    {
        if (_playerTransform != null)
            return;

        var player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        _playerTransform = player.transform;
        _playerRigidbody = player.GetComponent<Rigidbody>();

        if (_playerTransform == null || _playerRigidbody == null)
            this.enabled = false;
    }

    public virtual Vector3 PlayerPosition
    {
        get
        {
            if (_playerTransform == null)
                FindPlayerTransformInScene();
            _playerPosition = _playerTransform.position;
            return _playerPosition;
        }
    }

    protected bool IsPlayerInAttackRange
    {
        get
        {
            if (_transform == null || _playerPosition == null)
                return false;
            return Vector3.Distance(_transform.position, _playerPosition) < _attackRange;
        }
    }

    protected bool IsPlayerInDetectionRange
    {
        get
        {
            if (_transform == null || _playerPosition == null)
                return false;
            return Vector3.Distance(_transform.position, _playerPosition) < _playerDetectionRange;
        }
    }

    protected bool IsOnStateChangeCooldown => Time.time < _lastTimeChangedState + _stateChangeCooldown;
    protected bool IsBusy => _isPatrolling || _isWaiting;
    protected bool IsInterrupted => _shouldInterruptCurrentStateOnDetectRangeEnter
                                    && IsPlayerInDetectionRange
                                    || _shouldInterruptCurrentStateOnAttackRangeEnter
                                    && IsPlayerInAttackRange;
    public bool HasAtLeastOnePersistentStateProperty
    {
        get
        {
            return _isAlwaysLookingAtPlayer
                || _isAlwaysRotatingTowardPlayer
                || _isAlwaysChasingPlayer
                || _isAlwaysAttackingAtPlayer
                || _shouldBodyRotateTowardHeadDirection;
        }
    }

    #endregion

    #region Look at Player
    public virtual void LookAtPlayer()
    {
        if (_playerPosition == null)
            return;

        _tankHead.LookAtPoint(_playerPosition);
    }

    IEnumerator LookAtRandomPoint()
    {
        _isPatrolling = true;
        bool isLookingAtRandomPoint = false;

        Vector3 randomPoint = _tankHead.GetRandomRelativePoint(_minimumHeadRotationAngle);
        while (!isLookingAtRandomPoint)
        {
            _tankHead.LookAtPoint(randomPoint, relative: true);
            isLookingAtRandomPoint = _tankHead.IsLookingAtPoint(randomPoint, relative: true);
            yield return null;
        }

        yield return StartCoroutine(WaitForRandomTime());
        
        _isPatrolling = false;
        _lastTimeChangedState = Time.time;
    }

    #endregion

    #region Attacking
    protected Vector3 GetPredictedPlayerPosition()
    {
        Vector3 playerVelocity = _playerRigidbody.velocity;
        return _playerPosition + (playerVelocity * _leadShotFactorInUnits);
    }

    protected virtual void LookAtPredictedPlayerPosition()
    {
        if (_leadShotFactorInUnits == 0)
            return;

        var predictedPlayerPosition = GetPredictedPlayerPosition();
        if (predictedPlayerPosition != null)
            _tankHead.LookAtPoint(predictedPlayerPosition);
    }

    protected virtual void LeadShotAndLookAtPlayer()
    {
        if (!IsPlayerInAttackRange)
            LookAtPredictedPlayerPosition();
        else
        {
            _tankHead.LookAtPoint(_playerPosition);
        }
    }

    protected virtual void AttackIfWithinAngleThreshold(float angleThreshold)
    {
        Vector3 playerDirection = _playerPosition - transform.position;
        bool withinAngleThreshold = _tankHead.IsAngleBetweenVectorsBelowThreshold(_tankHead.transform.forward,
            playerDirection.normalized,
            angleThreshold);

        if (withinAngleThreshold)
            _tankShoot.Shoot();
    }

    #endregion
    #region Wait
    protected IEnumerator WaitForRandomTime()
    {
        _isWaiting = true;
        float waitTimeInSeconds = Random.Range(_minimumWaitTime, _maximumWaitTime);
        yield return new WaitForSeconds(waitTimeInSeconds);
        _isWaiting = false;
    }

    #endregion
    protected virtual void OnDrawGizmosSelected()
    {
        if (_transform == null || _enemyTankData == null)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_transform.position, _playerDetectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_transform.position, _attackRange);

        Gizmos.color = Color.green;
        var predictedPlayerPosition = GetPredictedPlayerPosition();
        Gizmos.DrawWireSphere(predictedPlayerPosition, 0.5f);

        if (_tankHead != null)
        {
            Gizmos.DrawRay(transform.position, _tankHead.transform.forward*15);
        }
    }
}

public enum EnemyState
{
    Patrol,
    Chase,
    Attack,
    Wait
}