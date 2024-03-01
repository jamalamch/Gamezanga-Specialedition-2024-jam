using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBillboard : MonoBehaviour
{
    public Transform cam;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    
        Vector3 dir = transform.position - cam.position;
        dir.y = 0;
        dir = dir.normalized;
        transform.LookAt(transform.position + dir);
        
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Land"){
            rb.isKinematic = true;
        }

    }
}