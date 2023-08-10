using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiseCannonsController : MonoBehaviour
{
    [SerializeField] CannonScriptableObject[] cannons;
    [SerializeField] CannonController cannonController;
    int cannonNumber = 0; 
    private void Start()
    {
        cannonController.CreateCannonObject(cannons[cannonNumber]);
    }
    //По кнопке улучшения
    public void ChangeCannonType()
    {
        cannonNumber++;
        cannonController.CreateCannonObject(cannons[cannonNumber]);
    }
}
