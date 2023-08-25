using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Система")]
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;       
    [SerializeField] MoneyManager moneyManager;
    [Header("Цены улучшений")]
    [SerializeField] float cannonPriceStep;
    [SerializeField] float castlePriceStep;
    [SerializeField] float incomePriceStep;
    [SerializeField] float multiplierUpgradePercent;
    float moneyMultiplier = 1;
    float castlePrice = 1;
    float cannonPrice = 1;
    float incomePrice = 1;
    [Header("Текст цены улучшений")]
    [SerializeField] TextMeshProUGUI castleUpdragePriceText;
    [SerializeField] TextMeshProUGUI cannonUpdragePriceText;
    [SerializeField] TextMeshProUGUI moneyIncomeUpdragePriceText;
    SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        moneyMultiplier = Progress.Instance.playerInfo.multiplier;
        castlePrice = Progress.Instance.playerInfo.castlePrice;       
        cannonPrice = Progress.Instance.playerInfo.cannonPrice;
        incomePrice = Progress.Instance.playerInfo.incomePrice;
        UpdatePriceTexts();       
    }
    //По кнопке
    public void UpgradeCannon()
    {     
        if (moneyManager.TryToSpend(cannonPrice))
        {
            soundManager.Play("PositiveClick");
            cannonController.UpgradeStats();
            cannonPrice += cannonPriceStep;           
            UpdatePriceTexts();
            Progress.Instance.playerInfo.cannonPrice = cannonPrice;
            YandexSDK.Save();
        }   
        else soundManager.Play("NegativeClick");
    }
    //По кнопке
    public void UpgradeCastle()
    {
        if (moneyManager.TryToSpend(castlePrice))
        {
            soundManager.Play("PositiveClick");
            castleController.ChangeMaxHealth();
            castlePrice += castlePriceStep;
            UpdatePriceTexts();
            Progress.Instance.playerInfo.castlePrice = castlePrice;
            YandexSDK.Save();
        }
        else soundManager.Play("NegativeClick");
    }
    //По кнопке
    public void UpgradeMoneyIncome()
    {
        if (moneyManager.TryToSpend(incomePrice))
        {
            soundManager.Play("PositiveClick");
            moneyMultiplier += multiplierUpgradePercent/100f;
            moneyManager.SetMoneyMultiplier(moneyMultiplier);
            incomePrice += incomePriceStep;
            UpdatePriceTexts();           
            Progress.Instance.playerInfo.incomePrice = incomePrice;
            Progress.Instance.playerInfo.multiplier = moneyMultiplier;
            YandexSDK.Save();
        }
        else soundManager.Play("NegativeClick");

    }
    void UpdatePriceTexts()
    {
        castleUpdragePriceText.text = castlePrice.ToString("#.##") + "$";
        cannonUpdragePriceText.text = cannonPrice.ToString("#.##") + "$";
        moneyIncomeUpdragePriceText.text = incomePrice.ToString("#.##") + "$";
        
    }

}
