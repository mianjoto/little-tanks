using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMovement), typeof(TankShoot))]
public class PlayerManager : MonoBehaviour
{
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


    void ListenForInput()
    {
        _isListeningForInput = true;
        InputListener.OnMovingForward += _tankMovement.MoveTankForward;
        InputListener.OnMovingBackward += _tankMovement.MoveTankBackward;
        InputListener.OnRotatingClockwise += _tankMovement.RotateTankClockwise;
        InputListener.OnRotatingCounterClockwise += _tankMovement.RotateTankCounterClockwise;
        InputListener.OnShootKey += _tankShoot.Shoot;
    }

    void StopListeningForInput()
    {
        _isListeningForInput = false;
        InputListener.OnMovingForward -= _tankMovement.MoveTankForward;
        InputListener.OnMovingBackward -= _tankMovement.MoveTankBackward;
        InputListener.OnRotatingClockwise -= _tankMovement.RotateTankClockwise;
        InputListener.OnRotatingCounterClockwise -= _tankMovement.RotateTankCounterClockwise;
        InputListener.OnShootKey -= _tankShoot.Shoot;
    }
    
    public void ToggleListeningForInput()
    {
        if (_isListeningForInput)
            StopListeningForInput();
        else
            ListenForInput();
    }
}
