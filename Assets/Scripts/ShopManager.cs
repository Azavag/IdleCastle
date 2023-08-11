using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;
    [SerializeField] float healthUpgrade;
    [SerializeField] float moneyMultiplierUpgrade;
    [SerializeField] MoneyManager moneyManager;

    [SerializeField] float castleUpdragePrice;
    [SerializeField] float cannonUpdragePrice;
    [SerializeField] float moneyIncomeUpdragePrice;

    [SerializeField] TextMeshProUGUI castleUpdragePriceText;
    [SerializeField] TextMeshProUGUI cannonUpdragePriceText;
    [SerializeField] TextMeshProUGUI moneyIncomeUpdragePriceText;

    [SerializeField] float upgradeCannonPricePercent = 20;

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
            cannonUpdragePrice *= 1 + upgradeCannonPricePercent / 100;
            UpdatePriceTexts();
        }
        
    }

    public void UpgradeCastle()
    {
        if (moneyManager.TryToSpend(castleUpdragePrice))
        {
            castleController.ChangeMaxHealth(healthUpgrade);
            castleUpdragePrice *= 1 + upgradeCannonPricePercent / 100;
            UpdatePriceTexts();
        }
    }

    public void UpgradeMoneyIncome()
    {
        if (moneyManager.TryToSpend(moneyIncomeUpdragePrice))
        {
            moneyManager.ChangeMoneyMultiplier(moneyMultiplierUpgrade);
            moneyIncomeUpdragePrice *= 1 + upgradeCannonPricePercent / 100;
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
