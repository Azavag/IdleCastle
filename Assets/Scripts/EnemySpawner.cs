using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int waveNumber;
    [SerializeField] int waveEnemyCount;
    int spawnedEnemyCount;
    
    [SerializeField] float timeBetweenSpawn;
    [SerializeField] GameObject firstBorderObject;
    [SerializeField] GameObject secondBorderObject;
    float xPosition;
    float zPosition; 

    [SerializeField] EnemyController enemyObject;

    bool spawnState;
    void Start()
    {
        

        
    }

    void Update()
    {
        if (spawnState)
        {
            StartCoroutine(SpawnEnemies(waveEnemyCount));
            SetSpawnState(false);
        }
    }


    IEnumerator SpawnEnemies(int enemiesCount)
    {
        for (int count = 0; count < enemiesCount; count++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        
    }

    public void SetSpawnState(bool state)
    {
        spawnState = state;
        spawnedEnemyCount = 0;
    }
    void SpawnEnemy()
    {
        spawnedEnemyCount++;

        //!!! позиция по высоте
        float yPos = 1.5f;

        GeneratePos(firstBorderObject, secondBorderObject);
        EnemyController enemy = Instantiate(enemyObject, new Vector3(xPosition, yPos, zPosition),
            Quaternion.LookRotation(new Vector3(0, 0, -1)), this.transform);


    }

    void GeneratePos(GameObject firstBorderObject, GameObject secondBorderObject)
    {
        float firstBorderXPostion = firstBorderObject.transform.position.x;
        float firstBorderZPostion = firstBorderObject.transform.position.z;

        float secondBorderXPostion = secondBorderObject.transform.position.x;
        float secondBorderZPostion = secondBorderObject.transform.position.z;

        xPosition = Random.Range(firstBorderXPostion, secondBorderXPostion);
        zPosition = Random.Range(firstBorderZPostion, secondBorderZPostion);

    }

  


}
