using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;
    [SerializeField] Button startButton;
    [SerializeField] Canvas menuCanvas;

    bool isStart;
    // Start is called before the first frame update
    void Start()
    {
       
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

    public void OnEndGame()
    {
        enemySpawner.SetSpawnState(false);
        enemySpawner.StopAllEnemies();
        cannonController.ChangeShootState(false);
        menuCanvas.gameObject.SetActive(true);
    }

}
