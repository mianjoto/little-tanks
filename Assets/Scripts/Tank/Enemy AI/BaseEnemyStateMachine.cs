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
    #endregion
    
    protected virtual void Awake()
    {
        // Assign required components
        _tankManager = GetComponent<TankManager>();
        _tankShoot = GetComponent<TankShoot>();
        _tankMovement = GetComponent<TankMovement>();
        _transform = transform;
        _tankHead = _tankManager.TankHead;
        _enemyTankData = (EnemyTankData) _tankManager.TankData;
        _playerDetectionRange = _enemyTankData.PlayerDetectionRange;
        _attackRange = _enemyTankData.AttackRange;
        _leadShotFactorInUnits = _enemyTankData.LeadShotFactorInUnits;
        GetPlayerTransform();
    }

    protected virtual void Update()
    {
        _playerPosition = GetPlayerPositionInWorld();        
        HandleStateLogic();
    }

    public abstract void HandleStateLogic();
    public abstract void TransitionStates();
    public abstract void Patrol();
    public virtual void Chase()
    {
        _tankMovement.RotateToPosition(_playerPosition);
        _tankMovement.MoveForward();
    }
    public virtual void Attack() => _tankShoot.Shoot();

    protected virtual void GetPlayerTransform()
    {
        if (_playerTransform != null)
            return;
        
        var player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        _playerTransform = player.transform;
        _playerRigidbody = player.GetComponent<Rigidbody>();

        if (_playerTransform == null || _playerRigidbody == null)
            this.enabled = false;
    }

    public virtual Vector3 GetPlayerPositionInWorld()
    {
        if (_playerTransform == null)
            GetPlayerTransform();
        _playerPosition = _playerTransform.position;
        return _playerPosition;
    }
    
    public virtual void LookAtPlayer()
    {
        if (_playerPosition == null)
            return;
        
        _tankHead.LookAtPoint(_playerPosition);
    }

    public virtual void LookAtPredictedPlayerPosition()
    {
        if (_leadShotFactorInUnits == 0)
            return;
        
        var predictedPlayerPosition = GetPredictedPlayerPosition();
        if (predictedPlayerPosition != null)
            _tankHead.LookAtPoint(predictedPlayerPosition);
    }

    public virtual void LeadShotAndLookAtPlayer()
    {
        if (!IsPlayerInAttackRange())
            LookAtPredictedPlayerPosition();
        else
        {
            _tankHead.LookAtPoint(_playerPosition);
        }
    }

    private Vector3 GetPredictedPlayerPosition()
    {
        Vector3 playerVelocity = _playerRigidbody.velocity;
        return _playerPosition + (playerVelocity * _leadShotFactorInUnits);
    }

    protected bool IsPlayerInDetectionRange()
    {
        if (_transform == null || _playerPosition == null)
        {
            Debug.Log($"Transform or player position is null in {transform.name}");
            return false;
        }
        return Vector3.Distance(_transform.position, _playerPosition) < _playerDetectionRange;
    }

    protected bool IsPlayerInAttackRange()
    {
        if (_transform == null || _playerPosition == null)
        {
            Debug.Log($"Transform or player position is null in {transform.name}");
            return false;
        }
        return Vector3.Distance(_transform.position, _playerPosition) < _attackRange;
    }

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
    }
}

public enum EnemyState
{
    Patrol,
    Chase,
    Attack,
    Wait
}