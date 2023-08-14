using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;
    [SerializeField] Button startButton;
    [SerializeField] Canvas menuCanvas;
    [SerializeField] TextMeshProUGUI passedWaveCountText;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePassedWaveCountText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnEndGame();
        }
    }

    public void OnStartGame()
    {
        enemySpawner.SetSpawnState(true);
        enemySpawner.StartSpawn();
        castleController.ResetHealth();
        cannonController.ChangeShootState(true);
        menuCanvas.gameObject.SetActive(false);
    }

    public void OnWinRound()
    {
        Debug.Log("Win");
        UpdatePassedWaveCountText();
        OnEndGame();
    }
    public void OnLoseRound()
    {
        Debug.Log("Loss");
        OnEndGame();
    }

    public void OnEndGame()
    {
        enemySpawner.SetSpawnState(false);
        enemySpawner.StopAllEnemies();
        cannonController.ChangeShootState(false);
        menuCanvas.gameObject.SetActive(true);
    }

    void UpdatePassedWaveCountText()
    {
        passedWaveCountText.text = "Пройдено волн: " + enemySpawner.GetPassedWavesCount().ToString();
    }
}
