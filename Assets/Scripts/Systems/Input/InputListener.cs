using System;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public bool ListenForInput = true;

    #region Keybinds
    [SerializeField] KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode moveForwardKey = KeyCode.W;
    [SerializeField] KeyCode moveBackwardKey = KeyCode.S;
    [SerializeField] KeyCode rotateCounterClockwiseKey = KeyCode.A;
    [SerializeField] KeyCode rotateClockwiseKey = KeyCode.D;
    #endregion

    #region Actions
    public static Action OnShootKey;
    public static Action OnMovingForward;
    public static Action OnMovingBackward;
    public static Action OnRotatingCounterClockwise;
    public static Action OnRotatingClockwise;
    #endregion

    public float HorizontalInput => Input.GetAxis("Horizontal");
    public float VerticalInput => Input.GetAxis("Vertical");

    #region Singleton Handler
    public static InputListener Instance { get; private set; }
    void Awake() => Instance = this;
    #endregion
    
    void Update()
    {
        if (!ListenForInput)
            return;
        
        HandleListenForInput();
    }

    private void HandleListenForInput()
    {
        if (Input.GetKeyDown(shootKey))
            OnShootKey?.Invoke();
        if (Input.GetKey(moveForwardKey))
            OnMovingForward?.Invoke();
        if (Input.GetKey(moveBackwardKey))
            OnMovingBackward?.Invoke();
        if (Input.GetKey(rotateCounterClockwiseKey))
            OnRotatingCounterClockwise?.Invoke();
        if (Input.GetKey(rotateClockwiseKey))
            OnRotatingClockwise?.Invoke();
    }
}
