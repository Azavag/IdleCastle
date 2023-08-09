using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public string enemyName;   
    public float speed;
    public float health;
    public float damage;
    public float moneyCost;

    public GameObject prefab;

}


