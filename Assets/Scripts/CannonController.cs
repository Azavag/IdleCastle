using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    
    [SerializeField] Camera mainCamera;
    [SerializeField] BulletsPool bulletsPool;
    [Header("��������������� �����")]
    [SerializeField] float bulletSpeed;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float bulletDamage;
    [SerializeField] CannonScriptableObject[] cannons;
    [Header("��������� �����")]
    [SerializeField] int upgradeSteps = 10;
    [SerializeField] float bulletDamageUpgradeStep = 0.2f;


    CannonScriptableObject cannonType;
    Transform cannonBody;
    Transform firePoint;
    GameObject cannonModelObject;

    float ShootTimer;
    float minAngle = -45.0f;
    float maxAngle = 45.0f;
    float clampedAngleY;
    bool canShoot;

    int upgradesCounter;
    int cannonNumber;


    private void Start()
    {
        cannonNumber = 0;
        upgradesCounter = 0;
        timeBetweenShots = 0.5f;
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
        Destroy(cannonModelObject);                         //������� ������� ������
        upgradesCounter = 0;                               
        // --------�������� ����� �����---------
        cannonType = cannonScriptableObject;
        cannonModelObject = Instantiate(cannonType.cannonModel, transform.position,
           Quaternion.identity, transform);
        cannonModelObject.name = "CannonBody_" + cannonType.cannonName;
        cannonBody = cannonModelObject.transform;
        firePoint = cannonModelObject.transform.Find("FirePoint");
        // --------��������� ������������� ����� �����---------
        SetCannonStats(cannonType);
    }

    void SetCannonStats(CannonScriptableObject cannonType)
    {
        bulletDamage = cannonType.bulletDamage;
        timeBetweenShots = cannonType.timeBetweenShots;
        bulletSpeed = cannonType.bulletSpeed;
    }
    //��������� ������������� �����
    public void UpgradeStats()
    {
        bulletDamage += bulletDamageUpgradeStep;
       
        upgradesCounter++;
        //����� ����� ����� n ���������
        if (upgradesCounter == upgradeSteps)           
            ChangeCannonType(++cannonNumber);
    }
    //�� ������ ��������� � ����� 10 ���������
    public void ChangeCannonType(int number)
    {
        CreateCannonObject(cannons[number]);
    }

    //������� ������ �� ����
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
            // ������� ���� � ������ �� ��������� �������, ����������� � ��������
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
