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
    //����� �����
    void UpdateHealthText()
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
        UpdateHealthText();
    }
    void Death()
    {      
        gameObject.SetActive(false);
        gameManager.OnEndGame();
    }


    public void ChangeMaxHealth(float diff)
    {
        maxHealth += diff;
        ResetHealth();
    }
  

}