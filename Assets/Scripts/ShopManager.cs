using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;       
    [SerializeField] MoneyManager moneyManager;
    [Header("���� ���������")]
    [SerializeField] float cannonPriceStep;
    [SerializeField] float castlePriceStep;
    [SerializeField] float incomePriceStep;

    [SerializeField] float multiplierUpgradePercent;
    float moneyMultiplier = 1;
    [SerializeField] float castleUpgradePrice;
    [SerializeField] float cannonUpradePrice;
    [SerializeField] float moneyIncomeUpgradePrice;
    [Header("����� ���� ���������")]
    [SerializeField] TextMeshProUGUI castleUpdragePriceText;
    [SerializeField] TextMeshProUGUI cannonUpdragePriceText;
    [SerializeField] TextMeshProUGUI moneyIncomeUpdragePriceText;


    void Start()
    {
        UpdatePriceTexts();
    }
    //�� ������
    public void UpgradeCannon()
    {     
        if (moneyManager.TryToSpend(cannonUpradePrice))
        {
            cannonController.UpgradeStats();
            cannonUpradePrice += cannonPriceStep;
            UpdatePriceTexts();
        }   
    }

    public void UpgradeCastle()
    {
        if (moneyManager.TryToSpend(castleUpgradePrice))
        {
            castleController.ChangeMaxHealth();
            castleUpgradePrice += cannonPriceStep;
            UpdatePriceTexts();
        }
    }

    public void UpgradeMoneyIncome()
    {
        if (moneyManager.TryToSpend(moneyIncomeUpgradePrice))
        {
            moneyMultiplier += multiplierUpgradePercent/100f;
            moneyManager.SetMoneyMultiplier(moneyMultiplier);
            moneyIncomeUpgradePrice += incomePriceStep;
            UpdatePriceTexts();
        }
        
    }
    void UpdatePriceTexts()
    {
        castleUpdragePriceText.text = castleUpgradePrice.ToString("#.##") + "$";
        cannonUpdragePriceText.text = cannonUpradePrice.ToString("#.##") + "$";
        moneyIncomeUpdragePriceText.text = moneyIncomeUpgradePrice.ToString("#.##") + "$";
    }

}
