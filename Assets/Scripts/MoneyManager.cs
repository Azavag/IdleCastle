using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{    
    [SerializeField] float moneyCount;
    [SerializeField] GameManager gameManager;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] float addMoneyAnimationTime;
    float waveMoneyCount;
    float tempWaveMoneyCount = 0;
    [SerializeField] TextMeshProUGUI moneyCountText;
    [SerializeField] TextMeshProUGUI waveMoneyCountText;
    public float MoneyMultiplier { set; get; }
    Color32 originalColor;
    [SerializeField] Color32 redColor;
    [SerializeField] Color32 addedColor;

    private void Awake()
    {
        EventManager.EnemyKilled += OnEnemyKilled;
    }
    void Start()
    {
        MoneyMultiplier = 1;
        originalColor = moneyCountText.color;
    }  
    public bool TryToSpend(float price)
    {
        if (moneyCount < price)
            return false;

        ChangeMoneyCount(-price); 
        return true;
    }
    public void ChangeMoneyCount(float diff)
    {
        if(diff>0)
            StartCoroutine(UpdateMoneyAnimation(moneyCountText, moneyCount, diff));
        moneyCount += diff;        
        UpdateMoneyText();
        
    }
    //Обнвление текста денег за раунд
    public void ChangeWaveMoneyCount(float diff)
    {
        StartCoroutine(UpdateMoneyAnimation(waveMoneyCountText, waveMoneyCount, diff));
        waveMoneyCount += diff;
        //waveMoneyCountText.text = "Заработано: " + waveMoneyCount.ToString("0.00") + "$";
    }

  

    public void ChangeMoneyMultiplier(float diff)
    {
        MoneyMultiplier = 1 + diff;
    }
      
    void OnEnemyKilled(float cost)
    {
        tempWaveMoneyCount += cost;
    }

    public void ResetWaveMoneyCount()
    {
        waveMoneyCount = 0;
        tempWaveMoneyCount = 0;
    }

    //Обновление текста денег
    public void UpdateMoneyText()
    {        
        moneyCountText.text = GetMoneyCount().ToString("0.00") + "$";
      
    }
    //Анимация обновления
    IEnumerator UpdateMoneyAnimation(TextMeshProUGUI text, float orginalNumber, float difference)
    {
        float animationNumber = orginalNumber;
        float speed = 1 / addMoneyAnimationTime;
        float timeElapsed = 0f;
        float tempMoney = 0;
        float stepMoney = tempMoney;
        originalColor = text.color;
        if (difference > 0)
            text.color = addedColor;
        else yield return null;

        while (timeElapsed < addMoneyAnimationTime)
        {
            timeElapsed += speed * Time.deltaTime;
            tempMoney = animationCurve.Evaluate(timeElapsed) * difference;

            if (stepMoney != tempMoney)
            {
                text.text = (animationNumber + tempMoney).ToString("0.00") + "$";
                stepMoney = tempMoney;
            }
            yield return null;
        }

        animationNumber += tempMoney;
        text.color = originalColor;
        
    }

    public float GetMoneyCount()
    {
        return moneyCount;
    }
    public float GetTempWaveMoneyCount()
    {
        return tempWaveMoneyCount;
    }
    public float GetWaveMoneyCount()
    {
        return waveMoneyCount;
    }

}
