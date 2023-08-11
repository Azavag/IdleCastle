using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public CannonScriptableObject cannonType;
    [SerializeField] Camera mainCamera;
    [SerializeField] BulletsPool bulletsPool;

    [SerializeField] float bulletSpeed;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float bulletDamage;
    [SerializeField] CannonScriptableObject[] cannons;

    Transform cannonBody;
    Transform firePoint;
    GameObject cannonModelObject;

    float ShootTimer;
    float minAngle = -45.0f;
    float maxAngle = 45.0f;
    float clampedAngleY;
    bool canShoot;

    [SerializeField] float bulletRateUpgradeStep;
    [SerializeField] float bulletSpeedUpgradeStep;
    [SerializeField] float bulletDamageUpgradeStep;
    float upgradePercent;
    int upgradeSteps;
    int upgradesCounter;
    int cannonNumber;
    float upgradePrice;


    private void Start()
    {
        cannonNumber = 0;
        upgradePercent = 10;
        upgradeSteps = 10;
        upgradesCounter = 0;
        upgradePrice = 0;
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
        Destroy(cannonModelObject);                         //Удаляем прошлую модель
        upgradesCounter = 0;                                //Сбрасываем счётчик улучшений
        // --------Создание новой пушки---------
        cannonType = cannonScriptableObject;
        cannonModelObject = Instantiate(cannonType.cannonModel, transform.position,
           Quaternion.identity, transform);
        cannonModelObject.name = "CannonBody_" + cannonType.cannonName;
        cannonBody = cannonModelObject.transform;
        firePoint = cannonModelObject.transform.Find("FirePoint");
        // --------Получение характеристик новой пушки---------
        SetCannonStats(cannonType);
        SetUpgradesStep();
    }

    void SetCannonStats(CannonScriptableObject cannonType)
    {
        bulletDamage = cannonType.bulletDamage;
        timeBetweenShots = cannonType.timeBetweenShots;
        bulletSpeed = cannonType.bulletSpeed;

        upgradePrice = cannonType.price;

    }
    //Улучшения харакетристик пушки
    public void UpgradeStats()
    {
        bulletDamage += bulletDamageUpgradeStep;
        timeBetweenShots -= bulletRateUpgradeStep;
        bulletSpeed += bulletSpeedUpgradeStep;

        upgradesCounter++;
        if (upgradesCounter == upgradeSteps)
            ChangeCannonType(++cannonNumber);
    }
    //По кнопке улучшения и после 10 улучшений
    public void ChangeCannonType(int number)
    {
        CreateCannonObject(cannons[number]);
    }
    //Значение каждого шага для улучшения
    public void SetUpgradesStep()
    {
        bulletDamageUpgradeStep = ((upgradePercent / 100) * cannonType.bulletDamage) / upgradeSteps;

        bulletSpeedUpgradeStep = ((upgradePercent / 100) * cannonType.bulletSpeed) / upgradeSteps;

        bulletRateUpgradeStep = ((upgradePercent / 100) * cannonType.timeBetweenShots) / upgradeSteps;
    }
    //Поворот пукшки на угол
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
    }

    void Shoot()
    {
        if (canShoot)
        {
            // Создаем пулю и задаем ей начальную позицию, направление и скорость
            BulletController spawneBullet = bulletsPool.SpawnFromPool(firePoint.position);
            spawneBullet.SetShootVector(firePoint.forward);
            spawneBullet.SetShootSpeed(bulletSpeed);
            spawneBullet.SetShootDamage(bulletDamage);
        }
        else return;

    }
    void ResetTimer()
    {
        ShootTimer = timeBetweenShots;
    }
 
    public float GetUpgradePrice()
    {
        return upgradePrice;
    }
}
