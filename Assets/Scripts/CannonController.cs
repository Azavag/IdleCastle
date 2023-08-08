using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform cannonBody;
    public Transform firePoint;

    [SerializeField] float bulletSpeed;
    [SerializeField] float timeBetweenShots;
    [SerializeField] int bulletDamage;
    float timer;
    float minAngle = -45.0f;
    float maxAngle = 45.0f;
    float clampedAngleY;
    float shootXVector;
    bool canShoot;
    [SerializeField] Camera mainCamera;

    [SerializeField] CannonBallPool cannonBallPool;


    private void Start()
    {
        cannonBallPool.CreateCannonBallsPool();
        canShoot = false;

        Input.mousePosition.Set(0,0,0);

        ResetTimer();
    }

    
    private void Update()
    {
        timer -= Time.deltaTime;

        RotateCannon();

        if (timer <= 0 && Input.GetMouseButton(0))
        {
            Shoot();
            ResetTimer();
        }
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

    public void ChangeShootState()
    {
        canShoot = true;
    }

    void Shoot()
    {
        if (canShoot)
        {
            // Создаем пулю и задаем ей начальную позицию, направление и скорость
            CannonBallController spawnedCannonball = cannonBallPool.SpawnFromPool(firePoint.position);
            spawnedCannonball.SetShootVector(firePoint.forward);
            spawnedCannonball.SetShootSpeed(bulletSpeed);
            spawnedCannonball.SetShootDamage(bulletDamage);
        }
        else return;

    }
    void ResetTimer()
    {
        timer = timeBetweenShots;
    }

    public void ChangeDamage(int diff)
    {
        bulletDamage += diff ;
    }
    public void ChangeRate(float diff)
    {
        timeBetweenShots += diff;
    }
}
