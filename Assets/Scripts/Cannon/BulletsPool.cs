using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    [SerializeField] BulletController bulletPrefab;
    [SerializeField] int bulletsAmount;
    Queue<BulletController> bulletsQueue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateBulletsPool()
    {
        bulletsQueue = new Queue<BulletController>();

        for (int i = 0; i < bulletsAmount; i++)
        {
            BulletController bullet = Instantiate(bulletPrefab, transform);
            
            bullet.gameObject.SetActive(false);
            bulletsQueue.Enqueue(bullet);
        }
    }

    public BulletController SpawnFromPool(Vector3 spawnPosition)
    {
        BulletController objectToSpawn = bulletsQueue.Dequeue();
        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.position = spawnPosition;

        bulletsQueue.Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    
}
