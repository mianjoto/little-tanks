using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLookAtCursor : MonoBehaviour
{
    [SerializeField]  
    LayerMask levelMask;
    [SerializeField]
    float rotationDamping = 10;
 
    bool _isLookingAtCursor;
    Camera _mainCamera;
    float _lockedXRotation = 0;
    float _lockedZRotation = 0;

    void Start() => _mainCamera = Camera.main;
    void OnEnable() => _isLookingAtCursor = true;
    void OnDisable() => _isLookingAtCursor = false;

    private void Update()
    {
        if (!_isLookingAtCursor)
            return;
        LookAtCursor();
    }

    private void LookAtCursor()
    {
        Vector3 mousePosition = GetMousePosition();
        if (mousePosition == Vector3.zero)
            return;

        Quaternion rotation = Quaternion.LookRotation(mousePosition - transform.position, Vector3.up);
        rotation.x = _lockedXRotation;
        rotation.z = _lockedZRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationDamping * Time.deltaTime);
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, levelMask))
            return hit.point;
        else
            return Vector3.zero;
    }
}
