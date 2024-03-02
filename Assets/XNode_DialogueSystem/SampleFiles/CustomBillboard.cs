using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBillboard : MonoBehaviour
{
    public Transform cam;

    void Update()
    {
    
        Vector3 dir = transform.position - cam.position;
        dir.y = 0;
        dir = dir.normalized;
        transform.LookAt(transform.position + dir);
    }
}