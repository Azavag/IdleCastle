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

    float costMultiplier;


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
    public void SetCostMultiplier(float multiplier)
    {
        costMultiplier = multiplier;
    }


    void SetEnemyData()
    {
        prefabModel = enemyType.prefab;
        Instantiate(prefabModel, transform.position - new Vector3(0, colliderScale / 2f, 0),
            transform.rotation, transform);

        maxHealth = enemyType.health;
        currentHealth = maxHealth;
        damage = enemyType.damage;
        moveSpeed = enemyType.speed;
        cost = enemyType.moneyCost * costMultiplier;
        this.name = enemyType.enemyName;
    }

   


}
