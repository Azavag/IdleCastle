using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("Объекты на уровнне")]
    [SerializeField] int waveCount = 0;
    [SerializeField] int waveEnemiesCount;

    List<EnemyController> aliveEnemies;
    int killedEnemies;
    
    [Header ("Правила спавна объектов")]
    [SerializeField] float minTimeBetweenSpawn;
    [SerializeField] float maXtimeBetweenSpawn;
    [SerializeField] GameObject firstBorderObject;
    [SerializeField] GameObject secondBorderObject;
    float timeBetweenSpawn;
    float xSpawnPosition;
    float zSpawnPosition;
    float ySpawnPosition = 2f;
    [Header("Правила улучшения монстров")]
    float maxHealthBonus = 0;
    float damageBonus = 0;
    int waveEnemiesCountUpgrade = 1;
    [Header("Объекты")]
    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] EnemyController bossEnemyPrefab;
    [SerializeField] EnemyScriptableObject[] enemyTypeArr;
    [SerializeField] EnemyScriptableObject[] bossEnemyTypeArr;
    [Header("Система")]
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

        timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maXtimeBetweenSpawn);
    }

    public void StartSpawn()
    {
        killedEnemies = 0;
           
        for (int count = 0; count < waveEnemiesCount; count++)
        {
            EnemyController enemyObject = Instantiate(enemyPrefab, 
                GeneratePosition(firstBorderObject, secondBorderObject),
                Quaternion.LookRotation(new Vector3(0, 0, -1)), 
                transform);

            //-------- Установка типа и доп. характеристик монстров -------//
            SetBonuses(enemyTypeArr, enemyObject, moneyManager.MoneyMultiplier, maxHealthBonus, damageBonus);


            enemyObject.gameObject.SetActive(false);
            aliveEnemies.Add(enemyObject);
        }

        if ((waveCount+1) % 10 == 0)
        {
            Vector3 bossPosition =  (firstBorderObject.transform.position + secondBorderObject.transform.position) / 2f;
            EnemyController enemyObject = Instantiate(bossEnemyPrefab, 
                new Vector3(bossPosition.x, 10f, bossPosition.z),
                Quaternion.LookRotation(new Vector3(0, 0, -1)), 
                transform);
            //-------- Установка типа и доп. характеристик босса -------//
            SetBonuses(bossEnemyTypeArr, enemyObject, moneyManager.MoneyMultiplier, maxHealthBonus * 3, damageBonus * 3);

            enemyObject.gameObject.SetActive(false);
            aliveEnemies.Add(enemyObject);
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
    void SetBonuses(EnemyScriptableObject[] enemiesArr, EnemyController enemy, float moneyMultiplier, float hltBonus, float dmgBonus)
    {
        enemy.GetComponent<EnemyData>().SetCostMultiplier(moneyMultiplier);
        enemy.GetComponent<EnemyData>().SetMaxhealthBonus(hltBonus);
        enemy.GetComponent<EnemyData>().SetAttackBonus(dmgBonus);
        
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
    void OnEnemyDied(float cost)
    {
        killedEnemies++;

        if (killedEnemies == aliveEnemies.Count)
        {
            waveCount++;
            CheckWavesCountOnUpgrades();
            gameManager.OnWinRound();
        }
        
    }
    //Улучшение характеристик монстров В зависимости от волны
    void CheckWavesCountOnUpgrades()
    {
        if(waveCount % 5 == 0)
            maxHealthBonus++;
        if (waveCount % 10 == 0)
        {
            damageBonus++;
            waveEnemiesCount += waveEnemiesCountUpgrade;
        }
    }

    public void StopAllEnemies()
    {
        SetSpawnState(false);
        StopCoroutine(spawnRoutine);

        if (aliveEnemies.Count > 0)
        {
            int countAlives = aliveEnemies.Count;
            for (int countEnemy = 0; countEnemy < countAlives; countEnemy++)
            {
                aliveEnemies[countEnemy].ChangeMoveState(false);
                Destroy(aliveEnemies[countEnemy].gameObject);
            }
            aliveEnemies.Clear();
        }
    }
    public int GetPassedWavesCount()
    {
        return waveCount;
    }
    private void OnDestroy()
    {
        EventManager.EnemyDied -= OnEnemyDied;
    }

}
