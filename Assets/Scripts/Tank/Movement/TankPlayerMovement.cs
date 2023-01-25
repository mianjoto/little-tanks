using UnityEngine;

public class TankPlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float RotateSpeed = 1f;

    Transform _transform;
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";
    bool _movingForward;
    bool _movingBackward;
    bool _rotatingLeft;
    bool _rotatingRight;

    void Awake() => _transform = transform;
    void Update() => ReadMovementInput();

    void FixedUpdate()
    {
        if (_movingForward)
            _transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        if (_movingBackward)
            _transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
        if (_rotatingLeft)
            _transform.Rotate(Vector3.up, RotateSpeed * -90 * Time.deltaTime);
        if (_rotatingRight)
            _transform.Rotate(Vector3.up, RotateSpeed * 90 * Time.deltaTime);
    }

    void ReadMovementInput()
    {
        var xAxisMovement = Input.GetAxisRaw(HORIZONTAL_AXIS);
        var yAxisMovement = Input.GetAxisRaw(VERTICAL_AXIS);

        if (xAxisMovement == 0)
            _rotatingRight = _rotatingLeft = false;
        if (yAxisMovement == 0)
            _movingForward = _movingBackward = false;
        
        if (xAxisMovement > 0)
            _rotatingRight = true;
        if (xAxisMovement < 0)
            _rotatingLeft = true;
        if (yAxisMovement > 0)
            _movingForward = true;
        if (yAxisMovement < 0)
            _movingBackward = true;
    }
}
