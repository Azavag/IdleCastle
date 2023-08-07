using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZone : MonoBehaviour
{

    private void Start()
    {
        
    }
    void Update()
    {
       
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.tag == "Enemy")
            Destroy(other.gameObject.transform.parent.gameObject);
    }
}
