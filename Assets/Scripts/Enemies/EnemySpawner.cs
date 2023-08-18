using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Объекты на уровнне")]
    [SerializeField] int waveCount;
    [SerializeField] int startEnemiesCount;
    [SerializeField] int waveEnemiesCount;
    [SerializeField] bool isBoosWave;
    [SerializeField] int waveBossCount;

    List<EnemyController> enemiesList;
    int killedEnemies;
    int diedEnemies;
    
    [Header ("Правила спавна объектов")]
    [SerializeField] float minTimeBetweenSpawn;
    [SerializeField] float maXtimeBetweenSpawn;
    [SerializeField] GameObject firstBorderObject;
    [SerializeField] GameObject secondBorderObject;
    [SerializeField] int waveToUpgradeStats;
    [SerializeField] int waveToUpgradeCount;
    [SerializeField] int waveToBoss;
    float timeBetweenSpawn;
    [SerializeField] float timeBetweenSpawnBoss;
    float xSpawnPosition;
    float zSpawnPosition;
    float ySpawnPosition = 0;
    [Header("Правила улучшения монстров")]
    [SerializeField] float currentStats;
    float damage;
    [SerializeField] float speed;
    [SerializeField] float startStats = 1;
    [SerializeField] float statsUpgrade;
    [Header("Объекты")]
    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] EnemyController bossEnemyPrefab;
    [SerializeField] EnemyScriptableObject[] enemyTypeArr;
    [SerializeField] EnemyScriptableObject[] bossEnemyTypeArr;
    [Header("Система")]
    [SerializeField] GameManager gameManager;
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] CastleController castleController;
    Coroutine spawnRoutine = null;
    bool spawnState;

    private void Awake()
    {
        EventManager.EnemyDied += OnEnemyDied;
        EventManager.EnemyKilled += OnEnemyKilled;
    }
    void Start()
    {
        enemiesList = new List<EnemyController>();        
        killedEnemies = 0;
        diedEnemies = 0;

        timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maXtimeBetweenSpawn);
    }

    public void StartSpawn()
    {
        killedEnemies = 0;
        diedEnemies = 0;
        CheckWavesCountOnUpgrades();

        //Спавн обычных мобов
        for (int count = 0; count < waveEnemiesCount; count++)
        {
            EnemyController enemyObject = Instantiate(enemyPrefab, 
                GeneratePosition(firstBorderObject, secondBorderObject),
                Quaternion.LookRotation(new Vector3(0, 0, -1)), 
                transform);

            //-------- Установка типа и доп. характеристик монстров -------//
            SetStats(enemyTypeArr, enemyObject, moneyManager.MoneyMultiplier, currentStats, speed);

            enemyObject.gameObject.SetActive(false);
            enemiesList.Add(enemyObject);
        }
        //Спавн босса
        if (isBoosWave)
        {
            for (int count = 0; count < waveBossCount; count++)
            {       
                EnemyController enemyObject = Instantiate(bossEnemyPrefab,
                    GeneratePosition(firstBorderObject, secondBorderObject),
                    Quaternion.LookRotation(new Vector3(0, 0, -1)),
                    transform);
                //-------- Установка типа и доп. характеристик босса -------//
                SetStats(bossEnemyTypeArr, enemyObject, moneyManager.MoneyMultiplier*3, currentStats * 3, speed);

                enemyObject.gameObject.SetActive(false);
                enemiesList.Add(enemyObject);
            }
        }

        if (spawnState)
        {
            spawnRoutine = StartCoroutine(ReleaseEnemies());
            SetSpawnState(false);
        }
    }

    //Включение объектов
    IEnumerator ReleaseEnemies()
    {
        for (int count = 0; count < waveEnemiesCount; count++)
        {           
            enemiesList[count].gameObject.SetActive(true);
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
        if (isBoosWave)
        {
            yield return new WaitForSeconds(timeBetweenSpawnBoss);
            for (int count = 0; count < waveBossCount; count++)
            {
                enemiesList[waveEnemiesCount + count].gameObject.SetActive(true);
                yield return new WaitForSeconds(timeBetweenSpawnBoss);
            }
        }
    }
    //Выставление характеристик мобов от уровня
    void CheckWavesCountOnUpgrades()
    {
        currentStats = startStats + (statsUpgrade * (waveCount / waveToUpgradeStats)); 
        waveEnemiesCount = startEnemiesCount + (waveCount + 1) / waveToUpgradeCount;
        if ((waveCount + 1) % waveToBoss == 0)
        {
            isBoosWave = true;
            waveBossCount = (waveCount + 1) / waveToBoss;
        }
        else isBoosWave = false;
    }
    public void SetSpawnState(bool state)
    {
        spawnState = state;
    }

    void SetStats(EnemyScriptableObject[] enemiesArr, EnemyController enemy, float multiplier, float stats, float speed)
    {
        enemy.GetComponent<EnemyData>().SetMultiplier(multiplier);
        enemy.GetComponent<EnemyData>().SetStats(stats);
        enemy.GetComponent<EnemyData>().SetSpeed(speed);
        
        int randomIndex = Random.Range(0, enemiesArr.Length);
        enemy.GetComponent<EnemyData>().ChooseEnemyType(enemiesArr[randomIndex]);
    }

    //Неактивный метод
    EnemyScriptableObject GenerateType(EnemyScriptableObject[] array)
    {
        int randomIndex = Random.Range(0, array.Length);
        return array[randomIndex];
    }

    Vector3 GeneratePosition(GameObject firstBorderObject, GameObject secondBorderObject)
    {
        float firstBorderXPostion = firstBorderObject.transform.position.x;
        float firstBorderZPostion = firstBorderObject.transform.position.z;

        float secondBorderXPostion = secondBorderObject.transform.position.x;
        float secondBorderZPostion = secondBorderObject.transform.position.z;

        xSpawnPosition = Random.Range(firstBorderXPostion, secondBorderXPostion);
        zSpawnPosition = Random.Range(firstBorderZPostion, secondBorderZPostion);

        return new Vector3(xSpawnPosition, ySpawnPosition, zSpawnPosition);
    }
    //При смерти противника
    void OnEnemyDied()
    {
        diedEnemies++;
        WinProccess();
    }
    //Когда пушка убивает
    void OnEnemyKilled(float cost)
    {
        killedEnemies++;
        WinProccess();
    }
    void WinProccess()
    {
        if ((diedEnemies + killedEnemies) == enemiesList.Count && castleController.GetHealth() > 0)
        {
            waveCount++;           
            gameManager.OnWinRound();
        }
    }
    //Улучшение характеристик монстров В зависимости от волны
   

    public void StopAllEnemies()
    {
        SetSpawnState(false);
        StopCoroutine(spawnRoutine);
        if (enemiesList.Count > 0)
        {
            foreach (var enemy in enemiesList)
            {
                enemy.ChangeMoveState(false);
            }
        }
    }

    public void ClearAllEnemies()
    {
        if (enemiesList.Count > 0)
        {
            int countAlives = enemiesList.Count;
            for (int countEnemy = 0; countEnemy < countAlives; countEnemy++)
            {                
                Destroy(enemiesList[countEnemy].gameObject);
            }
            enemiesList.Clear();
        }
    }
    public int GetPassedWavesCount()
    {
        return waveCount;
    }
    public int GetKilledEnemiesCount()
    {
        return killedEnemies;
    }
    private void OnDestroy()
    {
        EventManager.EnemyDied -= OnEnemyDied;
        EventManager.EnemyKilled -= OnEnemyKilled;
    }

}
