using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CastleController : MonoBehaviour, IDamagable
{
    
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float maxHealth;
    [SerializeField] GameManager gameManager;
    float currentHealth;  

    private void Start()
    {
        ResetHealth();
    }
    void Update()
    {
       
    }

    public void ResetHealth()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        UpdateHealthText();
    }
    //Через эвент
    void UpdateHealthText()
    {
        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
    public void ApplyDamage(float damageValue)
    {
        currentHealth -= damageValue;
        UpdateHealthText();
        if (currentHealth <= 0)
        {
            Death();
        }
        
    }
    void Death()
    {      
        gameObject.SetActive(false);
        gameManager.OnLoseRound();
    }


    public void ChangeMaxHealth(float diff)
    {
        maxHealth += diff;
        ResetHealth();
    }
  

}
