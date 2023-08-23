
using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<float> EnemyKilled;
    public static event Action EnemyDied;
    public static event Action ShotMaked;
    public static void OnEnemKilled(float cost)
    {
        EnemyKilled?.Invoke(cost);
    }
    public static void OnEnemDied()
    {
        EnemyDied?.Invoke();
    }
    public static void OnShotMaked()
    {
        ShotMaked?.Invoke();
    }
}

