using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{    
    public float MoneyCount { set; get; }
    public float MoneyMultiplier { set; get; }
    [SerializeField] TextMeshProUGUI moneyCountText;

    private void Awake()
    {
        EventManager.EnemyDied += OnEnemyDied;
    }
    void Start()
    {
        MoneyCount = 0;
        MoneyMultiplier = 1;
        UpdateMoneyText();       
    }

    void OnEnemyDied(float cost)
    {
        ChangeMoneyCount(cost);
    }
    
    void ChangeMoneyCount(float diff)
    {
        MoneyCount += diff;
        UpdateMoneyText();
    }
    void UpdateMoneyText()
    {
        moneyCountText.text = MoneyCount.ToString("#.##") + "$";
    }

    public void ChangeMoneyMultiplier(float diff)
    {
        MoneyMultiplier += diff;
    }

}
