using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyData : MonoBehaviour
{
    EnemyScriptableObject enemyType;
    [SerializeField] EnemyScriptableObject[] enemyTypeArr;
    [SerializeField] GameObject prefab;
    MeshRenderer m_meshRenderer;
    MeshFilter m_meshFilter;
    public float m_Speed { get; set; }

    private void Start()
    {
        m_meshRenderer = GetComponentInChildren<MeshRenderer>();
        m_meshFilter = GetComponentInChildren<MeshFilter>();

        ChooseEnemyType();
        SetEnemyType();
    }

    void SetEnemyType()
    {
        //m_meshFilter.mesh = enemyType.meshModel;
        //m_meshRenderer.material = enemyType.material;
        prefab = enemyType.prefab;
        Instantiate(prefab, transform.position + new Vector3(0,-1.5f,0), transform.rotation, this.transform);
        m_Speed = enemyType.speed;
        this.name = enemyType.enemyName;
    }
    private void ChooseEnemyType()
    {
        int randomIndex = Random.Range(0, enemyTypeArr.Length);
        enemyType = enemyTypeArr[randomIndex];
    }

}
