using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class EnemyController : MonoBehaviour, IDamagable
{
    EnemyData enemyData;
    Rigidbody rb;
    Slider healthBarSlider;
   
    bool isMoving;                          //Движение
    public float scale { set; get; }        //Размер коллайдера

    
    void Start()
    {      
        enemyData = GetComponent<EnemyData>();
        rb = GetComponent<Rigidbody>();
        healthBarSlider = GetComponentInChildren<Slider>();
        rb.WakeUp();
        scale = enemyData.colliderScale;
        healthBarSlider.maxValue = enemyData.maxHealth;

        StartMoving();
    }
   

    void StartMoving()
    {
        isMoving = true;
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
        ChangeHealtBarValue();
        if (enemyData.currentHealth <= 0)
        {
            Death();
        }
        
    }

    void Death() 
    {
        Destroy(gameObject);
        Debug.Log(name + "Умер");
    }

    void ChangeHealtBarValue()
    {
        healthBarSlider.value = enemyData.currentHealth;
    }
}
