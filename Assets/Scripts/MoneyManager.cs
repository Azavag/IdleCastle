using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{    
    public float moneyCount;
    public float MoneyMultiplier { set; get; }
    [SerializeField] TextMeshProUGUI moneyCountText;

    private void Awake()
    {
        EventManager.EnemyDied += OnEnemyDied;
    }
    void Start()
    {
        moneyCount = 10000;
        MoneyMultiplier = 1;
        UpdateMoneyText();       
    }
   
    public bool TryToSpend(float price)
    {
        if (moneyCount < price)
            return false;

        ChangeMoneyCount(-price); 
        return true;
    }
    void ChangeMoneyCount(float diff)
    {       
        moneyCount += diff;
        UpdateMoneyText();
    }
    void UpdateMoneyText()
    {
        moneyCountText.text = moneyCount.ToString("#.##") + "$";
    }

    public void ChangeMoneyMultiplier(float diff)
    {
        MoneyMultiplier += diff;
    }
    void OnEnemyDied(float cost)
    {
        ChangeMoneyCount(cost);
    }

    public float GetMoneyCount()
    {
        return moneyCount;
    }
}
