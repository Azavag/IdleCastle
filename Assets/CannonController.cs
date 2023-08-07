using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Transform cannonHead;

    public Transform firePoint;
    [SerializeField] float bulletSpeed = 10;
    [SerializeField] float timeBetweenShots = 2;
    float timer;
    float minAngle = -45.0f;
    float maxAngle = 45.0f;
    [SerializeField] Camera mainCamera;

    [SerializeField] GameObject cannonBallPrefab;   
    [SerializeField] int cannonBallsAmount;
    public Queue<GameObject> cannonballsQueue;

    private void Start()
    {



        CreateCannonBallsPool();
        ResetTimer();
        Vector2 cannonPostion = Camera.main.WorldToScreenPoint(cannonHead.position);
    }

    void CreateCannonBallsPool()
    {
        cannonballsQueue = new Queue<GameObject>();

        for (int i = 0; i < cannonBallsAmount; i++)
        {
            GameObject cannonBall = Instantiate(cannonBallPrefab);
            cannonBall.SetActive(false);
            cannonballsQueue.Enqueue(cannonBall);
        }
    }

    GameObject SpawnFromPool(Vector3 spawnPosition)
    {
        GameObject objectToSpawn = cannonballsQueue.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = spawnPosition;

        cannonballsQueue.Enqueue(objectToSpawn);
        return objectToSpawn;
    }
    
    private void Update()
    {
        timer -= Time.deltaTime;

        RotateCannon();

        if (Input.GetMouseButton(0) && timer <= 0)
        {

            Shoot();
            ResetTimer();
        }
    }
    
    void RotateCannon()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(hit.point);

            float rotationAngle = transform.rotation.eulerAngles.y;
            if (rotationAngle > 180.0f)
            {
                rotationAngle -= 360.0f;
            }
            float clampedAngleY = Mathf.Clamp(rotationAngle, minAngle, maxAngle);
            transform.rotation = Quaternion.Euler(new Vector3(0, clampedAngleY, 0));
        }
    }

    void Shoot()
    {
        // Создаем пулю и задаем ей начальную позицию и направление

        //GameObject bullet = Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        GameObject spawnedCannonball = SpawnFromPool(firePoint.position);
        Rigidbody rb = spawnedCannonball.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

    }

    void ResetTimer()
    {
        timer = timeBetweenShots;
    }
}
