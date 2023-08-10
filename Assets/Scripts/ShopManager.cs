using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] CannonController cannonController;
    [SerializeField] CastleController castleController;

    [SerializeField] float healthUpgrade;
    [SerializeField] float bulletRateUpgradeStep;
    [SerializeField] float bulletSpeedUpgradeStep;
    [SerializeField] float bulletDamageUpgradeStep;

    [SerializeField] float moneyMultiplierUpgrade;

    [SerializeField] MoneyManager moneyManager;
    float UpgradePercent = 50;

    // Start is called before the first frame update
    void Start()
    {
        SetUpgradesStep();
    }

    public void UpgradeCannon()
    {
        cannonController.ChangeBulletsDamage(bulletDamageUpgradeStep);
        cannonController.ChangeBulletsRate(-bulletRateUpgradeStep);
        cannonController.ChangeBulletsSpeed(bulletSpeedUpgradeStep);
    }

    public void SetUpgradesStep()
    {
        bulletDamageUpgradeStep = 0.1f * ((1 + UpgradePercent / 100) * cannonController.cannonType.bulletDamage -
           cannonController.cannonType.bulletDamage);

        bulletSpeedUpgradeStep = 0.1f * ((1 + UpgradePercent / 100) * cannonController.cannonType.bulletSpeed -
            cannonController.cannonType.bulletSpeed);

        bulletRateUpgradeStep = 0.1f * ((UpgradePercent / 100) * cannonController.cannonType.timeBetweenShots);
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
