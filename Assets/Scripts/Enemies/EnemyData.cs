using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    EnemyScriptableObject enemyType;
    GameObject prefabModel;

    BoxCollider boxCollider;
    Rigidbody rb;
    public Vector3 colliderScale;
    public float moveSpeed { get; set; }
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float damage { get; set; }
    public float cost { get; set; }
    public float timeAttack { get; set; }

    float costMultiplier;
    float healthBonus;
    float damageBonus;


    private void Start()
    {
        colliderScale = enemyType.prefab.transform.localScale;
        boxCollider = gameObject.GetComponent<BoxCollider>();

        boxCollider.center = new Vector3(enemyType.collider.center.x * colliderScale.x, 
            enemyType.collider.center.y * colliderScale.y, 
            enemyType.collider.center.z * colliderScale.z);
 
        boxCollider.size = new Vector3(enemyType.collider.size.x * colliderScale.x,
            enemyType.collider.size.y * colliderScale.y,
            enemyType.collider.size.z * colliderScale.z);

        enemyType.collider.enabled = false;
       // rb.isKinematic = true;
        

        SetEnemyData();

    }
    public void ChooseEnemyType(EnemyScriptableObject type)
    {
        enemyType = type;
    }
    //Цена устаналвивается при спавне
   
    void SetEnemyData()
    {
        prefabModel = enemyType.prefab;
        Instantiate(prefabModel, new Vector3(transform.position.x, transform.position.y, transform.position.z),
            transform.rotation, transform);

        maxHealth = enemyType.health + healthBonus;
        currentHealth = maxHealth;
        damage = enemyType.damage + damageBonus;
        moveSpeed = enemyType.speed;
        cost = enemyType.moneyCost * costMultiplier;
        timeAttack = enemyType.timeBetweenAtack;
        this.name = enemyType.enemyName;
    }
    public void SetCostMultiplier(float multiplier)
    {
        costMultiplier += multiplier;
    }
    public void SetMaxhealthBonus(float bonus)
    {
        healthBonus += bonus;
    }
    public void SetAttackBonus(float bonus)
    {
        damageBonus += bonus;
    }




}
