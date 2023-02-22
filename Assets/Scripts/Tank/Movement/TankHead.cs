using UnityEngine;

public class TankHead : MonoBehaviour
{
    [SerializeField] TankManager tankManager;
    [SerializeField] LayerMask levelMask;

    Camera _mainCamera;

    bool _isFollowingCursor;
    float _rotationDamping;
    float _lookingAtThreshold = 0.1f;

    const float LOCKED_X_ROTATION = 0;
    const float LOCKED_Z_ROTATION = 0;

    void Awake()
    {
        _rotationDamping = tankManager.TankData.HeadRotationDamping;
        _isFollowingCursor = tankManager.TankData.HeadFollowsCursor;
    }

    void Start() => _mainCamera = Camera.main;

    private void Update()
    {
        if (!_isFollowingCursor)
            return;
        LookAtCursor();
    }

    public void LookAtPoint(Vector3 position, bool relative = false)
    {
        var targetPosition = position;
        if (relative)
            targetPosition = transform.position + position;
        
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
        
        rotation.x = LOCKED_X_ROTATION;
        rotation.z = LOCKED_Z_ROTATION;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationDamping * Time.deltaTime);
    }

    public Vector3 LookAtRandomRelativePoint()
    {
        Vector2 randomPointInUnitCircle = Random.insideUnitCircle;
        Vector3 randomPoint = new Vector3(randomPointInUnitCircle.x, 0, randomPointInUnitCircle.y);
        LookAtPoint(randomPoint, relative: true);
        return randomPoint;
    }

    public bool IsLookingAtPoint(Vector3 point)
    {
        Vector3 direction = point - transform.position;
        float lookAmount = Vector3.Dot(transform.forward, direction.normalized);
        return lookAmount > _lookingAtThreshold;
    }

    public void LookAtCursor()
    {
        Vector3 mousePosition = GetMousePosition();
        if (mousePosition == Vector3.zero)
            return;
        LookAtPoint(mousePosition);
    }
   
    private Vector3 GetMousePosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, levelMask))
            return hit.point;
        else
            return Vector3.zero;
    }
}
