using UnityEngine;

public class HeadLookAtPoint : MonoBehaviour
{
    [SerializeField] TankManager tankManager;
    [SerializeField] LayerMask levelMask;

    Camera _mainCamera;

    bool _isFollowingCursor;
    float _rotationDamping;

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

    private void LookAtPoint(Vector3 position)
    {
        Quaternion rotation = Quaternion.LookRotation(position - transform.position, Vector3.up);
        rotation.x = LOCKED_X_ROTATION;
        rotation.z = LOCKED_Z_ROTATION;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotationDamping * Time.deltaTime);
    }

    private void LookAtCursor()
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
