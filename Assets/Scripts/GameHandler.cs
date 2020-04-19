using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameHandler : MonoBehaviour
{
   
    [Header("Flower Settings")]
    [SerializeField]
    float FlowerSpawnTime;
    float FlowerSpawnTimer;
    [SerializeField]
    Spawner FlowerSpawner;
    [Header("Enemy settings")]
    [SerializeField]
    float EnemySpawnTime;
    float EnemySpawnTimer;
    [SerializeField]   
    Spawner EnemySpawner;
    static GameHandler Instance;
    UnityEvent GameOver;
    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI GameOverText;
    int Score = 0;
    [SerializeField]
    TextMeshProUGUI ScoreText;




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
        GameOverText.text = "";
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
        ScoreText.text = "Score:" + Score;
    }

    void StartGameOver()
    {
        GameOverText.text = "Game Over";
        GameOver.Invoke();
    }

}
