using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Vector3 shootVector;
    float bulletSpeed;
    float bulletDamage;
    float rangeLimitX = 200f, rangeLimitZ = 250f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shootVector != null && bulletSpeed != 0)
            transform.Translate(shootVector * bulletSpeed);

        if(!CheckLimitPosition())
            ResetBulletPostion();
    }

    public void SetShootVector(Vector3 vec)
    {
        shootVector = vec;
    }
    public void SetShootSpeed(float speed)
    {
        bulletSpeed = speed;
    }
    public void SetShootDamage(float dmg)
    {
        bulletDamage = dmg;
    }

    //При прикосновении пули
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            if(enemy.TryGetComponent(out IDamagable damagable))
            {
                damagable.ApplyDamage(bulletDamage);
                
            }
            
            
            ResetBulletPostion();
        }

    }

    void ResetBulletPostion()
    {
        gameObject.SetActive(false);
        SetShootSpeed(0);
        SetShootVector(new Vector3(0, 0, 0));
        gameObject.transform.position = gameObject.transform.parent.position;
    }

    bool CheckLimitPosition()
    {
        return (Mathf.Abs(transform.position.x) < rangeLimitX ||
            Mathf.Abs(transform.position.z) < rangeLimitZ);
    }

}
