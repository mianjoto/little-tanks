using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TankExtensions
{
    public static Quaternion RestrictToYAxis(this Quaternion rotation)
    {
        Vector3 euler = rotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;
        rotation.eulerAngles = euler;
        return rotation;
    }
}
