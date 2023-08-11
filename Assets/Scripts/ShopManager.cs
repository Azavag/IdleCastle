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


    // Start is called before the first frame update
    void Start()
    {
        
    }
    //По кнопке
    public void UpgradeCannon()
    {
        cannonController.UpgradeStats();
        moneyManager.ChangeMoneyCount(-cannonController.GetUpgradePrice());
    }

    public void UpgradeCastle()
    {
        castleController.ChangeMaxHealth(healthUpgrade);
    }

    public void UpgradeMoneyIncome()
    {
        moneyManager.ChangeMoneyMultiplier(moneyMultiplierUpgrade);
    }

}
