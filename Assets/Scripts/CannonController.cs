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

    Transform cannonBody;
    Transform firePoint;
    GameObject cannonModel;

    float ShootTimer;
    float minAngle = -45.0f;
    float maxAngle = 45.0f;
    float clampedAngleY;
    bool canShoot;
    


    private void Start()
    {
       
        bulletsPool.CreateBulletsPool();
       
        canShoot = false;
        ResetTimer();
    }

    public void CreateCannonObject(CannonScriptableObject cannonScriptableObject)
    {
        Destroy(cannonModel);
        cannonType = cannonScriptableObject;
        cannonModel = Instantiate(cannonType.cannonModel, transform.position,
           Quaternion.identity, transform);
        cannonModel.name = "CannonBody_" + cannonType.cannonName;
        cannonBody = cannonModel.transform;
        firePoint = cannonModel.transform.Find("FirePoint");

        SetCannonStats(cannonType);
    }

    void SetCannonStats(CannonScriptableObject cannonType)
    {
        bulletDamage = this.cannonType.bulletDamage;
        timeBetweenShots = this.cannonType.timeBetweenShots;
        bulletSpeed = this.cannonType.bulletSpeed;

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

    public void ChangeBulletsDamage(float diff)
    {
        bulletDamage += diff;
    }
    public void ChangeBulletsRate(float diff)
    {
        timeBetweenShots += diff;
    }
    public void ChangeBulletsSpeed(float diff)
    {
        bulletSpeed += diff;
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

   

   
}
