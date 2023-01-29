using UnityEngine;

[RequireComponent(typeof(TankMovement))]
public class EnemyTankMovement : MonoBehaviour
{
    [SerializeField] TankMovement tankMovement;
    Transform _transform;
    float _moveSpeed;
    float _headRotateSpeed;

    void Awake() => _transform = transform;

    void MoveToPosition(Vector3 position)
    {
        var direction = position - _transform.position;
        var angle = Vector3.Angle(direction, _transform.forward);
        var cross = Vector3.Cross(direction, _transform.forward);
        if (cross.y < 0)
            angle = -angle;
        _transform.Rotate(Vector3.up, angle);
        tankMovement.MoveForward();
    }

    /// BUG: TEMPORARY
    /// BUG: TEMPORARY
    /// BUG: TEMPORARY
    /// BUG: TEMPORARY
    /// BUG: TEMPORARY

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            MoveToPosition(new Vector3(0, 0.65f, 0));
    }

}
