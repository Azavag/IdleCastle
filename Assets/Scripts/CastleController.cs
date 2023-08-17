using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class CastleController : MonoBehaviour, IDamagable
{
    
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] float maxHealth;
    [SerializeField] GameManager gameManager;
    [SerializeField] float currentHealth;
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] UnityEngine.UI.Image fillImage;
    private void Start()
    {
        ResetHealth();
    }


    public void ResetHealth()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
        UpdateHealthSlider();
    }
    
    void UpdateHealthSlider()
    {        
        slider.value = currentHealth;
        fillImage.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void ApplyDamage(float damageValue)
    {
        currentHealth -= damageValue;
        UpdateHealthSlider();
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            gameManager.OnLoseRound();
        }       
    }

    public void ChangeMaxHealth(float diff)
    {
        maxHealth += diff;
        ResetHealth();
    }
    public float GetHealth()
    {
        return currentHealth;
    }
  

}
