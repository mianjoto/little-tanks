using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] TankManager tankManager;
    Transform _transform;
    Rigidbody _rigidbody;
    float _moveSpeed;
    float _headRotateSpeed;
    float _bodyRotateSpeed;

    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = tankManager.TankData.MovementSpeed;
        if (tankManager.TankData.RotatesHeadSmoothly)
            _headRotateSpeed = tankManager.TankData.HeadRotationDamping;
        else
            _headRotateSpeed = tankManager.TankData.HeadRotationSpeed;
        _bodyRotateSpeed = tankManager.TankData.BodyRotateSpeed;
    }

    public void MoveForward() => _rigidbody.velocity = transform.forward * _moveSpeed;
    public void MoveBackward() => _rigidbody.velocity = -transform.forward * _moveSpeed;
    public void RotateCounterClockwise() => _transform.Rotate(Vector3.up, _headRotateSpeed * -90 * Time.deltaTime);
    public void RotateClockwise() => _transform.Rotate(Vector3.up, _headRotateSpeed * 90 * Time.deltaTime);
    public void RotateBodyToPosition(Vector3 position) => _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(_transform.position - position), Time.deltaTime * _bodyRotateSpeed);
}
