using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    int Score = 0;
    UnityEvent GameOver;
    [SerializeField]
    Spawner FlowerSpawner;
    [SerializeField]
    Spawner EnemySpawner;

    static GameHandler Instance;

    private void Awake()
    {
        if (Instance != null)
        { Destroy(this); }
        else { Instance = this; }
    }

    void AddScore(int _points)
    {
        Score += _points;
    }



}
