using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CastleController : MonoBehaviour, IDamagable
{
    
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float maxHealth;
    float currentHealth;  

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
        UpdateText();
    }
    void UpdateText()
    {
        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
    public void ApplyDamage(float damageValue)
    {
        currentHealth -= damageValue;

        if (currentHealth <= 0)
        {
            Death();
        }
        UpdateText();
    }
    void Death()
    {
        Debug.Log("Поражение");
        gameObject.SetActive(false);

    }


    public void UpgradeHealth(float diff)
    {
        maxHealth += diff;
        ResetHealth();
    }
  

}
