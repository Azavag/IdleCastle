using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    [Header ("Система")]
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] AdvManager advManager;
    [Header("UI элементы")]
    [SerializeField] Button startButton;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] GameObject endWavePanel;
    [SerializeField] GameObject upgradeButtons;
    [SerializeField] GameObject sideButton;
    [SerializeField] GameObject sideMenu;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject leaderboardObject;
    [SerializeField] GameObject rewardedButton;
    [Header("Text элементы")]
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] GameObject waveCountObject;
    TextMeshProUGUI waveCountText;
    [SerializeField] GameObject moneyCountObject;
    [SerializeField] TextMeshProUGUI waveEnemiesCountText;
    [SerializeField] TextMeshProUGUI waveNumberText;

    int wavesCount;
    string failureText;
    string successText;
    string roundText;       
       
    bool isStartRound;
    [Header("Анимация")]
    bool isEndAnimation;
    float tColour = 0;
    Color targetColour;
    Color startColourNoOpacity;
    float enterDuration = 0.8f;
    float exitDuration = 0.8f;

    bool isRewarded;
    void Start()
    {
        if (Language.isRusLang)
        {
            failureText = "Поражение";
            successText = "Победа";
            roundText = "уровень";
        }
        else
        {
            failureText = "Failure";
            successText = "Success";
            roundText = "level";
        }

            StartLaunch();
        targetColour = waveNumberText.color;
        startColourNoOpacity = targetColour; 
        startColourNoOpacity.a = 0f;

    }
    private void Update()
    {
        if(isStartRound)
            StartFadeWaveNumber();
        if (isEndAnimation)
            EndFadeWaveNumber();
    }

    //Пеереход на экран раунда
    IEnumerator GoToGameCanvas()
    {       
        moneyManager.ChangeMoneyCount(moneyManager.GetWaveMoneyCount());
        moneyManager.ResetWaveMoneyCount();
        yield return new WaitForSeconds(1f);

        endWavePanel.SetActive(false);
        startButton.gameObject.SetActive(true);
        sideButton.SetActive(true);
        upgradeButtons.SetActive(true);
        //waveCountObject.SetActive(true);
        rewardedButton.SetActive(true);
        castleController.ResetHealth();
        advManager.ShowAdv();
    }
    //По кнопке
    public void PressContinue()
    {
        StartCoroutine(GoToGameCanvas());
    }
    //Начало раунда(По кнопке)
    public void StartRound()
    {
        isStartRound = true;
        enemySpawner.SetSpawnState(true);
        enemySpawner.StartSpawn();      
        cannonController.ChangeShootState(true);
        sideMenu.SetActive(false);
        startButton.gameObject.SetActive(false);
        upgradeButtons.SetActive(false);
        //waveCountObject.SetActive(false);
        moneyCountObject.SetActive(false);
        sideButton.SetActive(false);
        pauseButton.SetActive(true);
    }
    //При победе
    public void OnWinRound()
    {
        FindObjectOfType<SoundManager>().Play("WinRound");
        resultText.text = successText;
        UpdateWaveCountText();
        YandexSDK.SetToLeaderboard();
        StartCoroutine(ShowFinalCanvas());
    }
    public void OnLoseRound()
    {
        FindObjectOfType<SoundManager>().Play("LoseRound");
        resultText.text = failureText;
        StartCoroutine(ShowFinalCanvas());
    }

    //Конец раунда
    IEnumerator ShowFinalCanvas()
    {      
        enemySpawner.StopAllEnemies();       
        cannonController.ChangeShootState(false);
        yield return new WaitForSeconds(0.6f);

        cannonController.ResetAllBullets();
        UpdateWaveEnemiesCountText();
        endWavePanel.SetActive(true);
        moneyManager.ChangeWaveMoneyCount(moneyManager.GetTempWaveMoneyCount());
        moneyCountObject.SetActive(true);
        cannonController.ResetCannonRotation();
        enemySpawner.ClearAllEnemies();
        
        pauseButton.SetActive(false);

    }
    //По кнопке
    public void ShowRewardedADV()
    {
        YandexSDK.ShowRewardedADV();
    }
    public void SetRewardingState()
    {
        isRewarded = true;
    } 
    //В jslib ShowRewardedADV -> DoubleWaveMoneyCount
    public void DoubleWaveMoneyCount()
    {
        if (isRewarded)
        {
            moneyManager.ChangeWaveMoneyCount(moneyManager.GetTempWaveMoneyCount());
            isRewarded = false;
        }
    }

    //В jslib
    public void HideRewardedButton()
    {
        rewardedButton.SetActive(false);
    }
    //Обновление текста по уровням
    void UpdateWaveCountText()
    {
        wavesCount = enemySpawner.GetPassedWavesCount();
    }
    //Обновление текста по убийствам на уровне
    void UpdateWaveEnemiesCountText()
    {
        waveEnemiesCountText.text = enemySpawner.GetKilledEnemiesCount().ToString();    
    }  
    void StartFadeWaveNumber()
    {
        waveNumberText.text = enemySpawner.GetPassedWavesCount()+1 + " " + roundText;
        tColour += Time.deltaTime / enterDuration;       
        waveNumberText.gameObject.SetActive(true);
        waveNumberText.color = Color.Lerp(startColourNoOpacity, targetColour, tColour);
        // if tColour is 1, colour must be at desired colour so mark entered as true for this entry
        if (tColour >= enterDuration)
        {
            tColour = 0f;
            isStartRound = false;
            isEndAnimation = true;
            EndFadeWaveNumber();
        }         
    }
    void EndFadeWaveNumber()
    {
        tColour += Time.deltaTime / exitDuration;
        waveNumberText.color = Color.Lerp(targetColour, startColourNoOpacity, tColour);
        if (tColour >= exitDuration)
        {
            tColour = 0f;            
            waveNumberText.gameObject.SetActive(false);
            isEndAnimation = false;
        }
    }

    //На запуске игры
    void StartLaunch()
    {
        mainCanvas.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
        upgradeButtons.SetActive(true);
        endWavePanel.SetActive(false);
        sideButton.SetActive(true);
        pauseButton.SetActive(false);
        //waveCountObject.SetActive(true);
        moneyCountObject.SetActive(true);
        waveCountText = waveCountObject.GetComponentInChildren<TextMeshProUGUI>();
        leaderboardObject.SetActive(false);
        resultText.text = "";
        UpdateWaveCountText();
        moneyManager.UpdateMoneyText();
    }

}
