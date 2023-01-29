using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMovement), typeof(TankShoot))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] InputListener inputListener;
    TankMovement _tankMovement;
    TankShoot _tankShoot;

    bool _isListeningForInput;

    void Awake()
    {
        _tankMovement = GetComponent<TankMovement>();
        _tankShoot = GetComponent<TankShoot>();
    }

    void OnEnable() => ListenForInput();
    void OnDisable() => StopListeningForInput();

    public void ToggleListeningForInput()
    {
        if (_isListeningForInput)
            StopListeningForInput();
        else
            ListenForInput();
    }

    void ListenForInput()
    {
        _isListeningForInput = true;
        inputListener.OnMovingForward += _tankMovement.MoveForward;
        inputListener.OnMovingBackward += _tankMovement.MoveBackward;
        inputListener.OnRotatingClockwise += _tankMovement.RotateClockwise;
        inputListener.OnRotatingCounterClockwise += _tankMovement.RotateCounterClockwise;
        inputListener.OnShootKey += _tankShoot.Shoot;
    }

    void StopListeningForInput()
    {
        _isListeningForInput = false;
        inputListener.OnMovingForward -= _tankMovement.MoveForward;
        inputListener.OnMovingBackward -= _tankMovement.MoveBackward;
        inputListener.OnRotatingClockwise -= _tankMovement.RotateClockwise;
        inputListener.OnRotatingCounterClockwise -= _tankMovement.RotateCounterClockwise;
        inputListener.OnShootKey -= _tankShoot.Shoot;
    }
}
