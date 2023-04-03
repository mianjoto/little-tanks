using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] TankManager tankManager;
    [SerializeField] Transform bodyTransform;
    Transform _transform;
    Rigidbody _rigidbody;
    float _moveSpeed;
    float _bodyRotateSpeed;

    void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();
        if (bodyTransform == null)
            bodyTransform = transform.GetChild(0);
        _moveSpeed = tankManager.TankData.MovementSpeed;
        _bodyRotateSpeed = tankManager.TankData.BodyRotateSpeed;
    }

    public void MoveTankForward() => _rigidbody.velocity = transform.forward * _moveSpeed;
    public void MoveTankBackward() => _rigidbody.velocity = -transform.forward * _moveSpeed;
    public void RotateTankClockwise() => _transform.Rotate(Vector3.up, _bodyRotateSpeed * 90 * Time.deltaTime);
    public void RotateTankCounterClockwise() => _transform.Rotate(Vector3.up, _bodyRotateSpeed * -90 * Time.deltaTime);
    public void RotateTankToPosition(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - _transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction).RestrictToYAxis();
        _transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _bodyRotateSpeed * Time.deltaTime);
    }

    public void RotateBodyToPosition(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - bodyTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction).RestrictToYAxis();
        bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, targetRotation, _bodyRotateSpeed * Time.deltaTime);
    }

    public void RotateBodyAlongHead()
    {
        bodyTransform.rotation = tankManager.TankHead.transform.rotation;
    }
}
