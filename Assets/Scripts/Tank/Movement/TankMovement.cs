using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] TankManager tankManager;
    Transform _transform;
    Rigidbody _rigidbody;
    float _moveSpeed;
    float _headRotateSpeed;

    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        _moveSpeed = tankManager.TankData.MovementSpeed;
        _headRotateSpeed = tankManager.TankData.BodyRotateSpeed;
    }

    public void MoveForward() => _rigidbody.velocity = transform.forward * _moveSpeed;
    public void MoveBackward() => _rigidbody.velocity = -transform.forward * _moveSpeed;
    public void RotateCounterClockwise() => _transform.Rotate(Vector3.up, _headRotateSpeed * -90 * Time.deltaTime);
    public void RotateClockwise() => _transform.Rotate(Vector3.up, _headRotateSpeed * 90 * Time.deltaTime);
    public void RotateToPosition(Vector3 position) => _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(position - _transform.position), Time.deltaTime * _headRotateSpeed);
}
