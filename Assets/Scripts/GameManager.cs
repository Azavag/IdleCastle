using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] CannonController cannonController;
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
        enemySpawner.SetStartSpawn(true);
        cannonController.ChangeShootState();
        menuCanvas.gameObject.SetActive(false);
    }

    public void OnEndGame()
    {
        enemySpawner.SetStartSpawn(false);
        menuCanvas.gameObject.SetActive(true);
    }

}
