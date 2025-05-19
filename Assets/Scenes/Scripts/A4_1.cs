using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  
public class A4_1 : MonoBehaviour
{

    public bool is_1 = true;



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Airwall")) 
        {
            is_1 = false;
            
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }
}
