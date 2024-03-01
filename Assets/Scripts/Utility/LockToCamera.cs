using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToCamera : MonoBehaviour
{
    public static Vector3 CameraNormalInvers;

    void Update()
    {
        transform.forward = CameraNormalInvers;
    }
}
