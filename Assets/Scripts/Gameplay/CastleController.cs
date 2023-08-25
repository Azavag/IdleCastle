using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class CastleController : MonoBehaviour, IDamagable
{
    [SerializeField] GameManager gameManager;
    [SerializeField] float maxHealth;  
    [SerializeField] float currentHealth;
    [SerializeField] float healthUpgrade;
    [Header("Слайдер")]
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] UnityEngine.UI.Image fillImage;

    private void Start()
    {
        maxHealth = Progress.Instance.playerInfo.maxHealth;
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
        currentHealth -= (float)Math.Round(damageValue,2);
        UpdateHealthSlider();
        if (currentHealth <= 0)
        {            
            gameManager.OnLoseRound();
        }
    }

    public void ChangeMaxHealth()
    {
        maxHealth += healthUpgrade;
        Progress.Instance.playerInfo.maxHealth = maxHealth;
        YandexSDK.Save();
        ResetHealth();
    }
    public float GetHealth()
    {
        return currentHealth;
    }
  

}
