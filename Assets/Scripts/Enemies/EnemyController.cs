using System.Collections;
using TMPro;
using UnityEngine;


public class EnemyController: MonoBehaviour, IDamagable
{
    EnemyData enemyData;
    Rigidbody rb;
    Canvas enemyCanvas;
    bool isMoving;                          //Движение
    bool isAttacking;                       //Атака
    float attackTimer;   
    public float scale { set; get; }        //Размер коллайдера

    [SerializeField] GameObject deathParticlesPrefab;
    
    void Start()
    {      
        enemyData = GetComponent<EnemyData>();
        rb = GetComponent<Rigidbody>();
        enemyCanvas = GetComponentInChildren<Canvas>();
        rb.WakeUp();
        scale = enemyData.colliderScale;

        ResetAttackTimer();
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
            
            //castle.ApplyDamage(enemyData.damage);

            isMoving = false;
            isAttacking = true;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Castle"))
        {
            if (isAttacking)
                attackTimer -= Time.deltaTime;
            CastleController castle = other.gameObject.GetComponent<CastleController>();
            //Атака
            if (attackTimer <= 0)
            {
                castle.ApplyDamage(enemyData.damage);
                ResetAttackTimer();
            }        
        }
    }
    //Получение урона
    public void ApplyDamage(float damageValue)
    {
        enemyData.currentHealth -= damageValue;

        if (enemyData.currentHealth <= 0)
        {
            GameObject partsClone = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            ParticleSystem parts = partsClone.GetComponent<ParticleSystem>();
            float totalDuration = parts.main.duration + parts.main.startLifetime.constantMax;
            Destroy(partsClone, totalDuration);
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

    void ResetAttackTimer()
    {
        attackTimer = enemyData.timeAttack;
    }
   

    void Death() 
    {
        
        EventManager.OnEnemyDied(enemyData.cost);        
        gameObject.SetActive(false);
    }
    
}
