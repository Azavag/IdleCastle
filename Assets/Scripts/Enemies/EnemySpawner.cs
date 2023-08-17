using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Header("������� �� �������")]
    [SerializeField] int waveCount = 0;
    [SerializeField] int waveEnemiesCount;

    List<EnemyController> enemiesList;
    int killedEnemies;
    int diedEnemies;
    
    [Header ("������� ������ ��������")]
    [SerializeField] float minTimeBetweenSpawn;
    [SerializeField] float maXtimeBetweenSpawn;
    [SerializeField] GameObject firstBorderObject;
    [SerializeField] GameObject secondBorderObject;
    float timeBetweenSpawn;
    float xSpawnPosition;
    float zSpawnPosition;
    float ySpawnPosition = 0;
    [Header("������� ��������� ��������")]
    float maxHealthBonus = 0;
    float damageBonus = 0;
    int waveEnemiesCountUpgrade = 1;
    [Header("�������")]
    [SerializeField] EnemyController enemyPrefab;
    [SerializeField] EnemyController bossEnemyPrefab;
    [SerializeField] EnemyScriptableObject[] enemyTypeArr;
    [SerializeField] EnemyScriptableObject[] bossEnemyTypeArr;
    [Header("�������")]
    [SerializeField] GameManager gameManager;
    [SerializeField] MoneyManager moneyManager;

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
        //����� ������� �����
        for (int count = 0; count < waveEnemiesCount; count++)
        {
            EnemyController enemyObject = Instantiate(enemyPrefab, 
                GeneratePosition(firstBorderObject, secondBorderObject),
                Quaternion.LookRotation(new Vector3(0, 0, -1)), 
                transform);

            //-------- ��������� ���� � ���. ������������� �������� -------//
            SetBonuses(enemyTypeArr, enemyObject, moneyManager.MoneyMultiplier, maxHealthBonus, damageBonus);

            enemyObject.gameObject.SetActive(false);
            enemiesList.Add(enemyObject);
        }
        //����� �����
        if ((waveCount+1) % 10 == 0)
        {
            Vector3 bossPosition =  (firstBorderObject.transform.position + secondBorderObject.transform.position) / 2f;
            EnemyController enemyObject = Instantiate(bossEnemyPrefab, 
                new Vector3(bossPosition.x, ySpawnPosition, bossPosition.z),
                Quaternion.LookRotation(new Vector3(0, 0, -1)), 
                transform);
            //-------- ��������� ���� � ���. ������������� ����� -------//
            SetBonuses(bossEnemyTypeArr, enemyObject, moneyManager.MoneyMultiplier, maxHealthBonus * 3, damageBonus * 3);

            enemyObject.gameObject.SetActive(false);
            enemiesList.Add(enemyObject);
        }

        if (spawnState)
        {
            spawnRoutine = StartCoroutine(ReleaseEnemies());
            SetSpawnState(false);
        }
    }

    //��������� ��������
    IEnumerator ReleaseEnemies()
    {
        for (int count = 0; count < enemiesList.Count; count++)
        {           
            enemiesList[count].gameObject.SetActive(true);
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

    //���������� �����
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
    //��� ������ ����������
    void OnEnemyDied()
    {
         diedEnemies++;

        if ((diedEnemies + killedEnemies) == enemiesList.Count)
        {                    
            gameManager.OnWinRound();
        }       
    }
    //����� ����� �������
    void OnEnemyKilled(float cost)
    {
        killedEnemies++;

        if ((diedEnemies + killedEnemies) == enemiesList.Count)
        {
            waveCount++;
            CheckWavesCountOnUpgrades();
            gameManager.OnWinRound();
        }
    }
    //��������� ������������� �������� � ����������� �� �����
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
