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
    protected Vector3 _playerPosition;
    #endregion

    #region Enemy State Properties
    protected EnemyState _currentState;
    protected float _playerDetectionRange;
    protected float _attackRange;
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
    public virtual void Chase() => _tankMovement.MoveForward();
    public virtual void Attack() => _tankShoot.Shoot();

    protected virtual void GetPlayerTransform()
    {
        if (_playerTransform != null)
            return;
        
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (_playerTransform == null)
            this.enabled = false;
    }

    public virtual Vector3 GetPlayerPositionInWorld()
    {
        if (_playerTransform == null)
            GetPlayerTransform();
        _playerPosition = _playerTransform.position;
        return _playerPosition;
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
    }
}

public enum EnemyState
{
    Patrol,
    Chase,
    Attack
}