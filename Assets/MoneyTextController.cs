using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MoneyTextController : MonoBehaviour
{
    TextMeshProUGUI moneyText;
    float costText;
    float tPosition;
    bool isAnimate;
    GameObject parentCanvas;
    Vector3 startPosition;


    void Start()
    {
        parentCanvas = transform.parent.gameObject;
        moneyText = GetComponent<TextMeshProUGUI>();
        tPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimate)
        {
            startPosition = moneyText.transform.position;
            AnimateMoneyText();
        }
    }
    void AnimateMoneyText()
    {
        moneyText.text = "+" + costText.ToString("#.##") + "$";      
        
        tPosition += Time.deltaTime / 0.4f;                      //Чем меньше цифра, тем меньше время анимации
        Vector3 targetPosition = startPosition + new Vector3(0f, 0.1f, 0f);
        moneyText.transform.position = Vector3.Lerp(startPosition, targetPosition, tPosition);
              
        if (tPosition >= 1f)
        {
            tPosition = 0f;
            transform.position = targetPosition;
            isAnimate = false;
            Destroy(parentCanvas);
        }
    }

    public void StartAnimate(float cost)
    {
        costText = cost;
        isAnimate = true;
    }
    
        
}
