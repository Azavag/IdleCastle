using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int waveNumber;
    [SerializeField] int waveEnemyCount, spawnedEnemyCount;
    
    [SerializeField] float timeBetweenSpawn;
    [SerializeField] GameObject firstBorderObject;
    [SerializeField] GameObject secondBorderObject;
    float xPosition;
    float zPosition;
    float yPostion = 3;
    float timer;

    [SerializeField] EnemyController enemyObject;
    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;
        if (timer < 0 && SpawnCheck())
        {
            SpawnEnemy();
            ResetTimer();
        }
  
    }


    void ResetTimer()
    {
        timer = timeBetweenSpawn;

    }
    void SpawnEnemy()
    {
        spawnedEnemyCount++;

        GeneratePos(firstBorderObject, secondBorderObject);
        EnemyController enemy = Instantiate(enemyObject, new Vector3(xPosition, yPostion, zPosition),
            Quaternion.LookRotation(new Vector3(0, 0, -1)), this.transform);

}

    bool SpawnCheck()
    {
        return spawnedEnemyCount < waveEnemyCount ? true : false;
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
