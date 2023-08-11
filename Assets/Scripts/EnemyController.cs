using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Input = UnityEngine.Input;

public class EnemyController : MonoBehaviour, IDamagable
{
    EnemyData enemyData;
    Rigidbody rb;
    Canvas enemyCanvas;
    bool isMoving;                          //Движение

    public float scale { set; get; }        //Размер коллайдера

    
    void Start()
    {      
        enemyData = GetComponent<EnemyData>();
        rb = GetComponent<Rigidbody>();
        enemyCanvas = GetComponentInChildren<Canvas>();
        rb.WakeUp();
        scale = enemyData.colliderScale;


        ChangeMoveState(true);
    }

    public void ChangeMoveState(bool state)
    {
        isMoving = state;
    }
    private void Update()
    {       
        if (isMoving)
            transform.Translate(enemyData.moveSpeed * Time.deltaTime * Vector3.forward);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Castle"))
        {
            CastleController castle = other.gameObject.GetComponent<CastleController>();
            //Атака
            //damagable.ApplyDamage(enemyData.damage);
            castle.ApplyDamage(enemyData.damage);

            isMoving = false;
            
        }

    }

    public void ApplyDamage(float damageValue)
    {
        enemyData.currentHealth -= damageValue;

        if (enemyData.currentHealth <= 0)
        {
            ChangeMoveState(false);
            DeathProcess();
            
        }
        
    }
    //Прибавление денег и уничтожение 
    void DeathProcess()
    {        
        enemyCanvas.transform.SetParent(null);
        enemyCanvas.gameObject.transform.position = transform.position;
        MoneyTextController mtc = enemyCanvas.GetComponentInChildren<MoneyTextController>();
        mtc.StartAnimate(enemyData.cost);



        Death();

    }

   

    void Death() 
    {
        
        EventManager.OnEnemyDied(enemyData.cost);        
        gameObject.SetActive(false);
    }
    
}
