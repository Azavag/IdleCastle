using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallPool : MonoBehaviour
{
    [SerializeField] CannonBallController cannonBallPrefab;
    [SerializeField] int cannonBallsAmount;
    Queue<CannonBallController> cannonballsQueue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateCannonBallsPool()
    {
        cannonballsQueue = new Queue<CannonBallController>();

        for (int i = 0; i < cannonBallsAmount; i++)
        {
            CannonBallController cannonBall = Instantiate(cannonBallPrefab, transform);
            
            cannonBall.gameObject.SetActive(false);
            cannonballsQueue.Enqueue(cannonBall);
        }
    }

    public CannonBallController SpawnFromPool(Vector3 spawnPosition)
    {
        CannonBallController objectToSpawn = cannonballsQueue.Dequeue();
        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.transform.position = spawnPosition;

        cannonballsQueue.Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}
