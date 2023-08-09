using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{    
    public float MoneyCount { set; get; }
    [SerializeField] TextMeshProUGUI moneyCountText;
    
    // Start is called before the first frame update
    void Start()
    {
        MoneyCount = 0;
        UpdateMoneyText();
        EventManager.EnemyDied += OnEnemyDied;
    }

    void OnEnemyDied(float cost)
    {
        MoneyCount += cost;
        UpdateMoneyText();
    }
    private void UpdateMoneyText()
    {
        moneyCountText.text = MoneyCount.ToString();
    }

}
