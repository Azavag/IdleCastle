using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int waveNumber;
    [SerializeField] int roundEnemiesCount;

    List<EnemyController> aliveEnemies;
    int killedEnemies;
    [SerializeField] float timeBetweenSpawn;
    [SerializeField] GameObject firstBorderObject;
    [SerializeField] GameObject secondBorderObject;
    float xPosition;
    float zPosition; 

    [SerializeField] EnemyController enemyObject;
    [SerializeField] EnemyScriptableObject[] enemyTypeArr;
    [SerializeField] GameManager gameManager;
    [SerializeField] MoneyManager moneyManager;
    Coroutine spawnRoutine = null;
    bool spawnState;

    private void Awake()
    {
        EventManager.EnemyDied += OnEnemyDied;
    }
    void Start()
    {
        aliveEnemies = new List<EnemyController>();        
        killedEnemies = 0;
    }

    void Update()
    {
       
    }

    public void StartSpawn()
    {
        killedEnemies = 0;
        if (spawnState)
        {

            if (aliveEnemies.Count > 0)
            {
                foreach (var enemy in aliveEnemies)
                {
                    Destroy(enemy.gameObject);
                }
                aliveEnemies.Clear();
            }

            SpawnEnemies(roundEnemiesCount);
            spawnRoutine = StartCoroutine(ReleaseEnemies());

            SetSpawnState(false);
        }
    }
    //Спавн всех объектов
    void SpawnEnemies(int enemiesCount)
    {       
        for (int count = 0; count < enemiesCount; count++)
        {
            float yPos = 1.5f;

            GeneratePos(firstBorderObject, secondBorderObject);

            EnemyController enemy = Instantiate(enemyObject, new Vector3(xPosition, yPos, zPosition),
                Quaternion.LookRotation(new Vector3(0, 0, -1)), this.transform);

            enemy.GetComponent<EnemyData>().SetCostMultiplier(moneyManager.MoneyMultiplier);
            enemy.GetComponent<EnemyData>().ChooseEnemyType(GenerateType());
            enemy.gameObject.SetActive(false);
            aliveEnemies.Add(enemy);
        }      
    }
    //Включение объекта
    IEnumerator ReleaseEnemies()
    {
        for (int count = 0; count < aliveEnemies.Count; count++)
        {           
            aliveEnemies[count].gameObject.SetActive(true);
            yield return new WaitForSeconds(timeBetweenSpawn);

        }       
    }

    public void SetSpawnState(bool state)
    {
        spawnState = state;
    }

    EnemyScriptableObject GenerateType()
    {
        int randomIndex = Random.Range(0, enemyTypeArr.Length);
        return enemyTypeArr[randomIndex];
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

    void OnEnemyDied(float cost)
    {
        killedEnemies++;

        if (killedEnemies == roundEnemiesCount)
        {          
            gameManager.OnEndGame();
        }
    }

    public void StopAllEnemies()
    {
        SetSpawnState(false);
        StopCoroutine(spawnRoutine);
        foreach (var enemy in aliveEnemies)
        {
            enemy.ChangeMoveState(false);            
        }
        
    }

    private void OnDestroy()
    {
        EventManager.EnemyDied -= OnEnemyDied;
    }
}
