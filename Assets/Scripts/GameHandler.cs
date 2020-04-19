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
    public static GameHandler Instance;
    public UnityEvent GameOver;
    bool IsGameOver = false;
    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI GameOverText;
    int Score = 0;
    [SerializeField]
    TextMeshProUGUI ScoreText;
    [SerializeField]
    TextMeshProUGUI AmmoText;



    private void Awake()
    {
        if (Instance != null)
        { Destroy(this); }
        else { Instance = this; }
    }

    private void Start()
    {
        FlowerSpawner.Spawn();
        FlowerSpawnTimer = Time.time + FlowerSpawnTime;
        EnemySpawnTimer = Time.time + EnemySpawnTimer;
        GameOverText.text = "";
    }

    private void Update()
    {
        if (IsGameOver)
            return;
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

    public void AddScore(int _points)
    {
        if (IsGameOver)
            return;
        Score += _points;
        ScoreText.text = "Score:" + Score;
    }

    public void StartGameOver()
    {
        if (IsGameOver)
            return;
        GameOverText.text = "Game Over";
        GameOver.Invoke();
        IsGameOver = true;
    }

    public void SetAmmoText(int _ammo)
    {
        if (_ammo > 0)
        {
            AmmoText.text = "Ammo:" + _ammo;
        }
        else
        {
            AmmoText.text = "Emtpy!";
        }
    }

}
