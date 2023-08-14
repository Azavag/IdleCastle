using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    EnemyScriptableObject enemyType;
    [SerializeField] GameObject prefabModel;
    [SerializeField] 


    BoxCollider boxCollider;
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
        boxCollider = GetComponent<BoxCollider>();
        colliderScale = boxCollider.size.y;

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
        Instantiate(prefabModel, transform.position - new Vector3(0, colliderScale / 2f, 0),
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
    public void SetMaxhealtBonus(float bonus)
    {
        healthBonus += bonus;
    }
    public void SetAttackBonus(float bonus)
    {
        damageBonus += bonus;
    }




}
