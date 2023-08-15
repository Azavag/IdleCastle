using System.Collections;
using TMPro;
using UnityEngine;


public class EnemyController: MonoBehaviour, IDamagable
{
    EnemyData enemyData;
    Canvas enemyCanvas;
    bool isMoving;                          //Движение
    bool isAttacking;                       //Атака
    float attackTimer;   
    public float scale { set; get; }        //Размер коллайдера

    [SerializeField] GameObject deathParticlesPrefab;
    
    void Start()
    {      
        enemyData = GetComponent<EnemyData>();
        enemyCanvas = GetComponentInChildren<Canvas>();
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
        if (other.gameObject.GetComponent<CastleController>())
        {
            isMoving = false;
            isAttacking = true;
           
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<CastleController>())
        {
            if (isAttacking)
                attackTimer -= Time.deltaTime;
            
            //Атака
            if (attackTimer <= 0)
            {
                other.gameObject.GetComponent<CastleController>().ApplyDamage(enemyData.damage);
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
            //GameObject partsClone = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);

            deathParticlesPrefab.transform.SetParent(null);           
            ParticleSystem parts = deathParticlesPrefab.GetComponent<ParticleSystem>();
            deathParticlesPrefab.SetActive(true);
            float totalDuration = parts.main.duration + parts.main.startLifetime.constantMax;


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
