using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] TankManager tankManager;
    Transform _transform;
    float _moveSpeed;
    float _headRotateSpeed;

    void Awake()
    {
        _transform = transform;
        _moveSpeed = tankManager.tankData.MovementSpeed;
        _headRotateSpeed = tankManager.tankData.BodyRotateSpeed;
    }

    public void MoveForward() => _transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
    public void MoveBackward() => _transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);
    public void RotateCounterClockwise() => _transform.Rotate(Vector3.up, _headRotateSpeed * -90 * Time.deltaTime);
    public void RotateClockwise() => _transform.Rotate(Vector3.up, _headRotateSpeed * 90 * Time.deltaTime);
}
