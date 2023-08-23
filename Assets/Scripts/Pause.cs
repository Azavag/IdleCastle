using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    CannonController cannonController;
    bool isPause;
    void Start()
    {
        pauseMenu.SetActive(false);
        cannonController = FindAnyObjectByType<CannonController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            SwitchPauseState();
    }
    public void SwitchPauseState()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
            cannonController.ChangeShootState(false);
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            cannonController.ChangeShootState(true);
            pauseMenu.SetActive(false);
        }
    }

}
