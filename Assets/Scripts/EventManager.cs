
using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<float> EnemyDied; 

    public static void OnEnemyDied(float cost)
    {
        EnemyDied?.Invoke(cost);
    }
}

