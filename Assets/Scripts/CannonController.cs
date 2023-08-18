using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    
    [SerializeField] Camera mainCamera;
    [SerializeField] BulletsPool bulletsPool;
    List<BulletController> bullets = new List<BulletController>();
    [Header("Харакетиристики пушки")]
    [SerializeField] float bulletSpeed = 50f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] float bulletDamage = 1;
    [SerializeField] CannonScriptableObject[] cannons;
    [Header("Улучшение пушки")]
    [SerializeField] int upgradeSteps = 10;
    [SerializeField] float bulletDamageUpgradeStep = 0.1f;

    CannonScriptableObject cannonType;
    Transform cannonBody;
    Transform firePoint;
    GameObject cannonModelObject;

    float ShootTimer;
    float minAngle = -45.0f;
    float maxAngle = 45.0f;
    float clampedAngleY;
    bool canShoot;

    int upgradesCounter = 0;
    int cannonNumber = 0;


    private void Start()
    {
        CreateCannonObject(cannons[cannonNumber]);

        bulletsPool.CreateBulletsPool();
       
        canShoot = false;
        ResetTimer();
    }

    private void Update()
    {
        ShootTimer -= Time.deltaTime;

        RotateCannon();

        if (ShootTimer <= 0 && Input.GetMouseButton(0))
        {
            Shoot();
            ResetTimer();
        }
    }
    public void CreateCannonObject(CannonScriptableObject cannonScriptableObject)
    {
        Destroy(cannonModelObject);                         
        upgradesCounter = 0;                               
        // --------Создание новой пушки---------
        cannonType = cannonScriptableObject;
        cannonModelObject = Instantiate(cannonType.cannonModel, transform.position,
           Quaternion.identity, transform);
        cannonModelObject.name = "CannonBody_" + cannonType.cannonName;
        cannonBody = cannonModelObject.transform;
        firePoint = cannonModelObject.transform.Find("FirePoint");
    }

    //Улучшения харакетристик пушки
    public void UpgradeStats()
    {
        bulletDamage += bulletDamageUpgradeStep;       
        upgradesCounter++;
        //Смена пушки после n апгрейдов
        if (upgradesCounter == upgradeSteps)           
            ChangeCannonType(++cannonNumber);
    }
    //По кнопке улучшения и после 10 улучшений
    public void ChangeCannonType(int number)
    {
        CreateCannonObject(cannons[number]);
    }

    //Поворот пушки на угол
    private void RotateCannon()
    {
        if (canShoot)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                cannonBody.LookAt(hit.point);

                float rotationAngle = cannonBody.rotation.eulerAngles.y;
                if (rotationAngle > 180.0f)
                {
                    rotationAngle -= 360.0f;
                }
                clampedAngleY = Mathf.Clamp(rotationAngle, minAngle, maxAngle);
                cannonBody.rotation = Quaternion.Euler(new Vector3(0, clampedAngleY, 0));
            }
        }
        else return;
    }

    public void ChangeShootState(bool state)
    {
        canShoot = state;
        
        if (canShoot)
            bullets.Clear();
    }
    //Выстрел
    void Shoot()
    {
        if (canShoot)
        {
            // Создаем пулю и задаем ей начальную позицию, направление и скорость
            BulletController spawnedBullet = bulletsPool.SpawnFromPool(firePoint.position);
            bullets.Add(spawnedBullet);
            spawnedBullet.SetShootVector(firePoint.forward);
            spawnedBullet.SetShootSpeed(bulletSpeed);
            spawnedBullet.SetShootDamage(bulletDamage);
        }
        else return;

    }
    void ResetTimer()
    {
        ShootTimer = timeBetweenShots;
    }

    public void ResetAllBullets()
    {
        foreach (var bullet in bullets)
        {
            bullet.ResetPostion();
        }     
    }
    public void ResetCannonRotation()
    {
        cannonBody.rotation = Quaternion.identity;
    }
}
