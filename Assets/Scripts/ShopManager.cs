using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;
    [SerializeField] float healthUpgrade;
    [SerializeField] float moneyMultiplierUpgradeStep;
    float moneyMultiplier = 0;
    [SerializeField] MoneyManager moneyManager;

    [SerializeField] float castleUpdragePrice;
    [SerializeField] float cannonUpdragePrice;
    [SerializeField] float moneyIncomeUpdragePrice;

    [SerializeField] TextMeshProUGUI castleUpdragePriceText;
    [SerializeField] TextMeshProUGUI cannonUpdragePriceText;
    [SerializeField] TextMeshProUGUI moneyIncomeUpdragePriceText;

    [SerializeField] float upgradePricePercent = 20;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePriceTexts();
    }
    //По кнопке
    public void UpgradeCannon()
    {     
        if (moneyManager.TryToSpend(cannonUpdragePrice))
        {
            cannonController.UpgradeStats();
            cannonUpdragePrice *= 1 + upgradePricePercent / 100;
            UpdatePriceTexts();
        }
        
    }

    public void UpgradeCastle()
    {
        if (moneyManager.TryToSpend(castleUpdragePrice))
        {
            castleController.ChangeMaxHealth(healthUpgrade);
            castleUpdragePrice *= 1 + upgradePricePercent / 100;
            UpdatePriceTexts();
        }
    }

    public void UpgradeMoneyIncome()
    {
        if (moneyManager.TryToSpend(moneyIncomeUpdragePrice))
        {
            moneyMultiplier += moneyMultiplierUpgradeStep/100f;
            moneyManager.ChangeMoneyMultiplier(moneyMultiplier);
            moneyIncomeUpdragePrice *= 1 + upgradePricePercent / 100f;
            UpdatePriceTexts();
        }
        
    }

    void UpdatePriceTexts()
    {
        castleUpdragePriceText.text = castleUpdragePrice.ToString("#.##") + "$";
        cannonUpdragePriceText.text = cannonUpdragePrice.ToString("#.##") + "$";
        moneyIncomeUpdragePriceText.text = moneyIncomeUpdragePrice.ToString("#.##") + "$";
    }

}
