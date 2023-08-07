using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class EnemyController : MonoBehaviour
{
    EnemyData enemyData;
    Rigidbody m_Rigidbody;
   
    bool m_isMoving;
    Vector3 moveDirectionVector;        //Направление движения

    //Добавить EnemyData для данных
    // Через контроллер только движение и другие действия

    void Start()
    {      
        m_Rigidbody = GetComponent<Rigidbody>();
        enemyData = GetComponent<EnemyData>();
        moveDirectionVector = new Vector3(0, 0, -1);

        StartMoving();
    }
   

    void StartMoving()
    {
        m_isMoving = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            m_isMoving = !m_isMoving;
    }

    private void FixedUpdate()
    {
        if (m_isMoving)
            m_Rigidbody.MovePosition(transform.position + moveDirectionVector * Time.deltaTime * enemyData.m_Speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cannon")
        {
            Destroy(gameObject);
            collision.gameObject.SetActive(false);
        }
    }

}
