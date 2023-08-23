using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


[System.Serializable]
public class PlayerInfo
{
    public int rounds = 0;          //++
    public int damage = 1;          //++
    public int cannonUpgrades = 0;  //++
    public int maxHealth = 5;       //++
    public float cannonPrice = 1;   //++
    public float castlePrice = 1;   //++
    public float incomePrice = 1;   //++
    public  float money = 0;        //++
    public float multiplier = 1;    //++
    public bool isMute = false;     //++
}


public class Progress : MonoBehaviour
{
    public PlayerInfo playerInfo;
    public static Progress Instance;
    YandexSDK yandexSDK;
   
    private void Awake()
    {
        yandexSDK = FindObjectOfType<YandexSDK>();
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;

            yandexSDK.Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}



