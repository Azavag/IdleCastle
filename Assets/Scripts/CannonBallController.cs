using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    Vector3 shootVector;
    float bulletSpeed;
    int damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shootVector != null && bulletSpeed != 0)
            transform.Translate(shootVector * bulletSpeed);
    }

    public void SetShootVector(Vector3 vec)
    {
        shootVector = vec;
    }
    public void SetShootSpeed(float speed)
    {
        bulletSpeed = speed;
    }
    public void SetShootDamage(int dmg)
    {
        damage = dmg;
    }

    //При прикосновении пули
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if(enemy.TryGetComponent(out IDamagable damagable))
            {
                damagable.ApplyDamage(damage);
                
            }
            
            gameObject.SetActive(false);

            
        }

    }

}
