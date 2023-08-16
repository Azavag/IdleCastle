using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header ("Система")]
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;
    [SerializeField] MoneyManager moneyManager;
    [Header("UI элементы")]
    [SerializeField] Button startButton;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] GameObject endWavePanel;
    [SerializeField] GameObject upgradeButtons;
    [Header("Text элементы")]
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GameObject waveCountObject;
    TextMeshProUGUI waveCountText;
    [SerializeField] GameObject moneyCountObject;
    [SerializeField] TextMeshProUGUI waveEnemiesCountText;

    int wavesCount;
    string failureText = "Поражение";
    string successText = "Победа";

    void Start()
    {
        StartLaunch();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnEndGame();
        }
    }

    //Пеереход на экран раунда
    public void GoToGameCanvas()
    {
        endWavePanel.SetActive(false);
        startButton.gameObject.SetActive(true);
        upgradeButtons.SetActive(true);
        waveCountObject.SetActive(true);

        moneyManager.ChangeMoneyCount(moneyManager.GetWaveMoneyCount());
        moneyManager.ResetWaveMoneyCount();
        castleController.ResetHealth();
    }
    //Начало раунда
    public void StartGame()
    {
        enemySpawner.SetSpawnState(true);
        enemySpawner.StartSpawn();      
        cannonController.ChangeShootState(true);
       
        startButton.gameObject.SetActive(false);
        upgradeButtons.SetActive(false);
        waveCountObject.SetActive(false);
        moneyCountObject.SetActive(false);
    }
    //При победе
    public void OnWinRound()
    {
        resultText.text = successText;
        UpdateWaveCountText();
        StartCoroutine(OnEndGame());
    }
    //При поражении
    public void OnLoseRound()
    {
        resultText.text = failureText;
        StartCoroutine(OnEndGame());
    }
    //Конец раунда
    IEnumerator OnEndGame()
    {      
        enemySpawner.StopAllEnemies();       
        cannonController.ChangeShootState(false);
        yield return new WaitForSeconds(0.6f);

        
        UpdateWaveEnemiesCountText();
        endWavePanel.SetActive(true);
        moneyManager.ChangeWaveMoneyCount(moneyManager.GetTempWaveMoneyCount());
        moneyCountObject.SetActive(true);
        cannonController.ResetCannonRotation();
        enemySpawner.ClearAllEnemies();
        
    }

    public void DoubleWaveMoneyCount()
    {
        moneyManager.ChangeWaveMoneyCount(moneyManager.GetTempWaveMoneyCount());
    }
    //Обновление текста по уровням
    void UpdateWaveCountText()
    {
        wavesCount = enemySpawner.GetPassedWavesCount();
        waveCountText.text = "Пройдено уровней: " + wavesCount.ToString();
    }
    //Обновление текста по убийства на уровне
    void UpdateWaveEnemiesCountText()
    {
        waveEnemiesCountText.text = enemySpawner.GetKilledEnemiesCount().ToString();    
    }
    //На запуске игры
    void StartLaunch()
    {
        mainCanvas.gameObject.SetActive(true);      
        startButton.gameObject.SetActive(true);
        upgradeButtons.SetActive(true);
        endWavePanel.SetActive(false);

        waveCountObject.SetActive(true);
        moneyCountObject.SetActive(true);
        waveCountText = waveCountObject.GetComponentInChildren<TextMeshProUGUI>();

        resultText.text = "";
        UpdateWaveCountText();
        moneyManager.UpdateMoneyText();
    }


}
