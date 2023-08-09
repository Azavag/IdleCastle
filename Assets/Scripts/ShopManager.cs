using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;

    [SerializeField] float healthUpgrade;
    [SerializeField] float bulletRateUpgrade;
    [SerializeField] float bulletSpeedUpgrade;
    [SerializeField] float bulletDamageUpgrade;

    [SerializeField] MoneyManager moneyManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpgradeCannon()
    {
        cannonController.ChangeBulletsDamage(bulletDamageUpgrade);
        cannonController.ChangeBulletsRate(-bulletRateUpgrade);
        cannonController.ChangeBulletsSpeed(bulletSpeedUpgrade);
    }

    public void UpgradeCastle()
    {
        castleController.ChangeMaxHealth(healthUpgrade);
    }

    void ChangeMoneyCount(float dif)
    {
        moneyManager.MoneyCount += dif;
    }

    

}
