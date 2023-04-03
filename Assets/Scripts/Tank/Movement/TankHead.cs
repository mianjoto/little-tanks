using UnityEngine;

public class TankHead : MonoBehaviour
{
    [SerializeField] TankManager tankManager;
    [SerializeField] LayerMask levelMask;

    Camera _mainCamera;

    float _rotationDamping;
    bool _isFollowingCursor;
    bool _rotatesHeadSmoothly;
    byte _lookAtAngleThreshold;
    float _headRotationSpeed;

    const float LOCKED_X_ROTATION = 0;
    const float LOCKED_Z_ROTATION = 0;

    void Awake()
    {
        _rotationDamping = tankManager.TankData.HeadRotationDamping;
        _isFollowingCursor = tankManager.TankData.HeadFollowsCursor;
        _rotatesHeadSmoothly = tankManager.TankData.RotatesHeadSmoothly;
        _lookAtAngleThreshold = tankManager.TankData.LookAtAngleThresholdInDegrees;
        _headRotationSpeed = tankManager.TankData.HeadRotationSpeed;
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
        Vector3 targetPosition = position;
        if (relative)
            targetPosition = transform.position + position;

        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

        rotation.x = LOCKED_X_ROTATION;
        rotation.z = LOCKED_Z_ROTATION;

        if (_rotatesHeadSmoothly)
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationDamping * Time.deltaTime);
        else
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _headRotationSpeed * Time.deltaTime);
    }

    public Vector3 GetRandomRelativePoint(float angleThresholdInDegrees = 0)
    {
        bool isValidRandomPoint = false;
        Vector2 randomPointInUnitCircle;
        Vector3 relativeRandomPoint = Vector3.zero;
        Vector3 forwardDirection = transform.forward;

        while (!isValidRandomPoint)
        {
            randomPointInUnitCircle = Random.insideUnitCircle;
            relativeRandomPoint = new Vector3(randomPointInUnitCircle.x, 0, randomPointInUnitCircle.y);
            isValidRandomPoint = !IsAngleBetweenVectorsBelowThreshold(transform.forward, relativeRandomPoint.normalized, angleThresholdInDegrees);
        }

        return relativeRandomPoint;
    }

    public bool IsAngleBetweenVectorsBelowThreshold(Vector3 point1, Vector3 point2, float angleThresholdInDegrees = 0)
    {
        float angle = Vector3.Angle(point1, point2);
        return angle < angleThresholdInDegrees;
    }

    public bool IsLookingAtPoint(Vector3 point, bool relative = false)
    {
        Vector3 direction = point - transform.position;
        if (relative)
            direction = point;

        return IsAngleBetweenVectorsBelowThreshold(transform.forward, direction.normalized, _lookAtAngleThreshold);
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
