using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bollboard : MonoBehaviour
{
    [SerializeField] Transform cam;
    
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
