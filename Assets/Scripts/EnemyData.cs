using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyData : MonoBehaviour
{
    EnemyScriptableObject enemyType;
    [SerializeField] EnemyScriptableObject[] enemyTypeArr;
    [SerializeField] GameObject prefab;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    BoxCollider boxCollider;
    public float colliderScale { get; set; }
    public float moveSpeed { get; set; }
    public float maxHealth { get; set; }
    public float currentHealth { get; set; }
    public float damage { get; set; }
    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshFilter = GetComponentInChildren<MeshFilter>();
        boxCollider = GetComponent<BoxCollider>();
        colliderScale = boxCollider.size.y;
        ChooseEnemyType();
        SetEnemyType();
    }

    void SetEnemyType()
    {
        prefab = enemyType.prefab;
        Instantiate(prefab, transform.position - new Vector3(0, colliderScale/2f, 0), transform.rotation, transform);


        maxHealth = enemyType.health;
        currentHealth = maxHealth;
        damage = enemyType.damage;
        moveSpeed = enemyType.speed;
        this.name = enemyType.enemyName;
    }
    private void ChooseEnemyType()
    {
        int randomIndex = Random.Range(0, enemyTypeArr.Length);
        enemyType = enemyTypeArr[randomIndex];
    }

}
