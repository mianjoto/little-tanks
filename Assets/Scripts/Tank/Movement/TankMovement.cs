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

    public void MoveBodyForward() => _rigidbody.velocity = transform.forward * _moveSpeed;
    public void MoveBodyBackward() => _rigidbody.velocity = -transform.forward * _moveSpeed;
    public void RotateBodyClockwise() => _transform.Rotate(Vector3.up, _bodyRotateSpeed * 90 * Time.deltaTime);
    public void RotateBodyCounterClockwise() => _transform.Rotate(Vector3.up, _bodyRotateSpeed * -90 * Time.deltaTime);
    public void RotateBodyToPosition(Vector3 position)
    {
        Vector3 direction = position - _transform.position;
        float angleToPosition = Quaternion.LookRotation(direction).eulerAngles.y;
        _transform.Rotate(Vector3.up, _bodyRotateSpeed * angleToPosition * Time.deltaTime);
    } 
}
