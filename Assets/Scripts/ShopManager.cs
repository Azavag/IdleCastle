using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] CannonController cannonController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpgradeDamage()
    {
        cannonController.ChangeDamage(10);
    }

    public void UpgradeBulletRate()
    {
        cannonController.ChangeRate(-0.1f);
    }

}
