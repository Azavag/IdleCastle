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
    public float colliderScale { get; set; }
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
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.size = enemyType.collider.size;
        boxCollider.center = enemyType.collider.center;
        enemyType.collider.enabled = false;
       // rb.isKinematic = true;
        //colliderScale = boxCollider.size.y;

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
