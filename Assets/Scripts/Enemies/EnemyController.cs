using System.Collections;
using TMPro;
using UnityEngine;


public class EnemyController: MonoBehaviour, IDamagable
{
    EnemyData enemyData;
    Canvas enemyCanvas;
    FlyTextController animatedText;
    bool isMoving;                          //Движение
    bool isAttacking;                       //Атака
    [SerializeField] float moveSpeed;

    [SerializeField] GameObject deathParticlesPrefab;
    
    void Start()
    {      
        enemyData = GetComponent<EnemyData>();
        enemyCanvas = GetComponentInChildren<Canvas>();
        
        animatedText = enemyCanvas.GetComponentInChildren<FlyTextController>();
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
            other.gameObject.GetComponent<CastleController>().ApplyDamage(enemyData.damage);
            EventManager.OnEnemDied();
            DeathAnimation();
        }
    }

    //Получение урона
    public void ApplyDamage(float damageValue)
    {
        enemyData.currentHealth -= damageValue;   
        animatedText.StartAnimate(damageValue);
        if (enemyData.currentHealth <= 0)
        {
            Vector3 destroyPos = transform.position;
            enemyCanvas.transform.SetParent(null, true);
            enemyCanvas.transform.position = 
                new Vector3(destroyPos.x, 
                enemyCanvas.transform.position.y, 
                enemyCanvas.transform.position.z);
                    
            ChangeMoveState(false);
            
            //EventManager.OnEnemDied();
            EventManager.OnEnemKilled(enemyData.cost);
            animatedText.StartLastAnimation();
            DeathAnimation();
        }
        
    }
    //Прибавление денег и уничтожение 
    void DeathAnimation()
    {
        //Партикл взрыва при смерти
        deathParticlesPrefab.transform.SetParent(null);
        ParticleSystem parts = deathParticlesPrefab.GetComponent<ParticleSystem>();
        deathParticlesPrefab.SetActive(true);
        float totalDuration = parts.main.duration + parts.main.startLifetime.constantMax;

        gameObject.SetActive(false);
    }


    
}
