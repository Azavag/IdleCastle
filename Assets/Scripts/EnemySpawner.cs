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
    float timer;

    [SerializeField] EnemyController enemyObject;

    bool isStartSpawn;
    void Start()
    {
        
        ResetTimer();
        
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && LimitSpawnCheck() && isStartSpawn)
        {
            SpawnEnemy();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = timeBetweenSpawn;
    }

    public void SetStartSpawn(bool state)
    {
        isStartSpawn = state;
        spawnedEnemyCount = 0;
    }
    void SpawnEnemy()
    {
        spawnedEnemyCount++;

        //ÕÀÐÄÊÎÄ
        float yPos = 1.5f;
        GeneratePos(firstBorderObject, secondBorderObject);
        EnemyController enemy = Instantiate(enemyObject, new Vector3(xPosition, yPos, zPosition),
            Quaternion.LookRotation(new Vector3(0, 0, -1)), this.transform);


    }

    bool LimitSpawnCheck()
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
