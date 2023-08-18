using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public string enemyName;   
    float speed;
    float health;
    float damage;
    float moneyCost;
    float timeBetweenAtack;

    public GameObject prefab;
    public BoxCollider collider;
}


