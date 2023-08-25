using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyTextController : MonoBehaviour
{
    TextMeshProUGUI damageText;
    float _damage;
    float tPosition;
    float animationTime = 0.45f;
    bool isAnimate;
    bool isLastAnimation;
    float startYPosition;
    GameObject parentCanvas;

    void Start()
    {
        damageText = GetComponent<TextMeshProUGUI>();
        parentCanvas = transform.parent.gameObject;
        tPosition = 0;
    }


    void Update()
    {
        if (isAnimate)
        {          
            if(_damage % 1 == 0)
                damageText.text = _damage.ToString("f0");
            else damageText.text = _damage.ToString("f2");

            tPosition += Time.deltaTime / animationTime;
            Vector3 targetPosition = transform.position + new Vector3(0f, 0.2f, 0f);
            transform.position = Vector3.Lerp(transform.position, targetPosition, tPosition);

            if (tPosition >= animationTime)
            {
                isAnimate = false;
                tPosition = 0f;
                transform.position = targetPosition;

                gameObject.SetActive(false);
                transform.position = new Vector3(transform.position.x, startYPosition, transform.position.z);
                if (isLastAnimation)
                    Destroy(parentCanvas);
            }
        }
    }

    public void StartAnimate(float textValue)
    {
        gameObject.SetActive(true);
        startYPosition = transform.position.y;
        _damage = textValue;
        damageText.enabled = true;
        isAnimate = true;      
    }

    public void StartLastAnimation()
    {
        isLastAnimation = true;
    }          
}
