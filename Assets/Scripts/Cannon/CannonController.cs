using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    
    [SerializeField] Camera mainCamera;
    [SerializeField] BulletsPool bulletsPool;
    List<BulletController> bullets = new List<BulletController>();
    [Header("��������������� �����")]
    [SerializeField] float bulletSpeed = 70f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] float bulletDamage = 1;
    [SerializeField] CannonScriptableObject[] cannons;
    [Header("��������� �����")]
    [SerializeField] int upgradeSteps = 10;
    [SerializeField] float bulletDamageUpgradeStep = 1;

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
        bulletDamage = Progress.Instance.playerInfo.damage;
        upgradesCounter = Progress.Instance.playerInfo.cannonUpgrades;
        cannonNumber = Progress.Instance.playerInfo.cannonNumber;
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
        // --------�������� ����� �����---------
        cannonType = cannonScriptableObject;
        cannonModelObject = Instantiate(cannonType.cannonModel, transform.position,
           Quaternion.identity, transform);
        cannonModelObject.name = "CannonBody_" + cannonType.cannonName;
        cannonBody = cannonModelObject.transform;
        firePoint = cannonModelObject.transform.Find("FirePoint");
    }

    //��������� ������������� �����
    public void UpgradeStats()
    {
        bulletDamage += bulletDamageUpgradeStep;       
        upgradesCounter++;
        Progress.Instance.playerInfo.damage = bulletDamage;
        Progress.Instance.playerInfo.cannonUpgrades = upgradesCounter;
        //����� ����� ����� n ���������
        if (upgradesCounter == upgradeSteps)
        {
            if (cannonNumber < cannons.Length - 1)
            {
                FindObjectOfType<SoundManager>().Play("CannonUpgrade");
                ChangeCannonType(++cannonNumber);
                upgradesCounter = 0;
                Progress.Instance.playerInfo.cannonNumber = cannonNumber;
                Progress.Instance.playerInfo.cannonUpgrades = upgradesCounter;
            }
        }
    }
    //�� ������ ��������� � ����� 10 ���������
    public void ChangeCannonType(int number)
    {
        CreateCannonObject(cannons[number]);
    }

    //������� ����� �� ����
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
    //�������
    void Shoot()
    {
        if (canShoot)
        {
            FindObjectOfType<SoundManager>().Play("Fire");
            // ������� ���� � ������ �� ��������� �������, ����������� � ��������
            BulletController spawnedBullet = bulletsPool.SpawnFromPool(firePoint.position);
            bullets.Add(spawnedBullet);
            spawnedBullet.SetShootVector(firePoint.forward);
            spawnedBullet.SetShootSpeed(bulletSpeed);
            spawnedBullet.SetShootDamage(bulletDamage);
            EventManager.OnShotMaked();
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
