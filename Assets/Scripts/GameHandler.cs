using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    int Score = 0;
    UnityEvent GameOver;
    [Header("Flower Settings")]
    [SerializeField]
    float FlowerSpawnTime;
    float FlowerSpawnTimer;
    Spawner FlowerSpawner;
    [Header("Enemy settings")]
    [SerializeField]
    float EnemySpawnTime;
    float EnemySpawnTimer;
    Spawner EnemySpawner;
    static GameHandler Instance;




    private void Awake()
    {
        if (Instance != null)
        { Destroy(this); }
        else { Instance = this; }
    }

    private void Start()
    {
        FlowerSpawnTimer = Time.time + FlowerSpawnTime;
        EnemySpawnTimer = Time.time + EnemySpawnTimer;
    }

    private void Update()
    {
        if (FlowerSpawnTimer < Time.time)
        {
            FlowerSpawner.Spawn();
            FlowerSpawnTimer = Time.time + FlowerSpawnTime;
        }
        if (EnemySpawnTimer < Time.time)
        {
            EnemySpawner.Spawn();
            EnemySpawnTimer = Time.time + EnemySpawnTime;
        }
    }

    void AddScore(int _points)
    {
        Score += _points;
    }

    void StartGameOver()
    {
        GameOver.Invoke();
    }

}
