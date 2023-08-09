using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    EnemyScriptableObject enemyType;
    [SerializeField] GameObject prefabModel;


    BoxCollider boxCollider;
    public float colliderScale { get; set; }
    public float moveSpeed { get; set; }
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float damage { get; set; }
    public float cost { get; set; }

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        colliderScale = boxCollider.size.y;

        SetEnemyData();

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
        cost = enemyType.moneyCost;
        this.name = enemyType.enemyName;
    }

    public void ChooseEnemyType(EnemyScriptableObject type)
    {
        enemyType = type;
    }


}
