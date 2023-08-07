using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public string enemyName;   
    public float speed;
    public int health;
    public int damage;

    //public Material material;
    //public Mesh meshModel;
    public GameObject prefab;

}


