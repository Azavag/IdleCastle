using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class EnemyController : MonoBehaviour, IDamagable
{
    EnemyData enemyData;
    Rigidbody rb;
    //Slider healthBarSlider;
    TextMeshProUGUI moneyText;
   
    bool isMoving;                          //Движение
    public float scale { set; get; }        //Размер коллайдера

    
    void Start()
    {      
        enemyData = GetComponent<EnemyData>();
        rb = GetComponent<Rigidbody>();
       // healthBarSlider = GetComponentInChildren<Slider>();
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
        rb.WakeUp();
        scale = enemyData.colliderScale;
        //healthBarSlider.maxValue = enemyData.maxHealth;

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
            CastleController finalZone = other.gameObject.GetComponent<CastleController>();

            //Атака
            if (finalZone.TryGetComponent(out IDamagable damagable))
            {
                damagable.ApplyDamage(enemyData.damage);

            }
            isMoving = false;
            
        }

    }

    public void ApplyDamage(float damageValue)
    {
        enemyData.currentHealth -= damageValue;
        //ChangeHealtBarValue();
        if (enemyData.currentHealth <= 0)
        {
            ChangeMoveState(false);
            StartCoroutine(DeathProcess());
            
        }
        
    }
    //Прибавление денег и уничтожение 
    IEnumerator DeathProcess()
    {
        moneyText.text = "+" + enemyData.cost.ToString() + "$";
        yield return new WaitForSeconds(0f);

        Death();

    }

    void Death() 
    {
        EventManager.OnEnemyDied(enemyData.cost);        
        gameObject.SetActive(false);
    }

    void ChangeHealtBarValue()
    {
        //healthBarSlider.value = enemyData.currentHealth;
    }

    
}
