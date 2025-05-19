using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target_1 : MonoBehaviour
{
    public GameObject target;
    public float speed = 1.0f; // 移动速度
    public airWall_1 airWall;
    public GameObject a4;
    public GameObject a4_1;
    public Airwall_3 airwall_3;
    public bool is_3 = true;
    public GameObject line;
    

    void with()
    {

         transform.SetParent(a4.transform); 
    }
    void loose()
    {
        transform.SetParent(null);
    }
    void Move() 
    {
        target.transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }

    void Start()
    {
        //Invoke("with", 15);
        //Invoke("loose", 18);
    }


    void Update()
    {
        if (airWall.isTriggered)
        {
            Move();
        }
    
        
    }
}