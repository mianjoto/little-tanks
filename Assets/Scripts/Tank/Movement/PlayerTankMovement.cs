using UnityEngine;

[RequireComponent(typeof(TankMovement))]
public class PlayerTankMovement : MonoBehaviour
{
    [SerializeField] TankMovement tankMovement;
    Transform _transform;
    float _moveSpeed;
    float _headRotateSpeed;

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
            tankMovement.MoveForward();
        if (_movingBackward)
            tankMovement.MoveBackward();
        if (_rotatingLeft)
            tankMovement.RotateCounterClockwise();
        if (_rotatingRight)
            tankMovement.RotateClockwise();
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
