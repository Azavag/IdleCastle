using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalZone : MonoBehaviour, IDamagable
{
    [SerializeField] int maxHealth;
    int currentHealth;  

    private void Start()
    {
        ResetHealth();
    }
    void Update()
    {
       
    }

    void ResetHealth()
    {        
        currentHealth = maxHealth;
    }
    public void ApplyDamage(int damageValue)
    {
        currentHealth -= damageValue;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("Поражение");
        gameObject.SetActive(false);
        
    }

}
