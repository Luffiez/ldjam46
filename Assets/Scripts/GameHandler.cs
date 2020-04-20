using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.IO;


[System.Serializable]
public class ScoreBoard
{//add date later
   public List<int> Scores;

}

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
    ScoreBoard HighScore;
    string FilePath;

    private void Awake()
    {
        if (Instance != null)
        { Destroy(this); }
        else { Instance = this; }
    }

    string CreateNewScoreFile(string filePath)
    {
        ScoreBoard scoreBoard = new ScoreBoard();
        scoreBoard.Scores = new List<int>();
        scoreBoard.Scores.Add(1000);
        string jsonString = JsonUtility.ToJson(scoreBoard);
        StreamWriter writer = new StreamWriter(File.Create(filePath));
        writer.Write(jsonString);
        writer.Flush();
        writer.Close();
        return jsonString;
    }


    private void Start()
    {
        FilePath = Application.dataPath + "/score.txt";
        FlowerSpawner.Spawn();
        FlowerSpawnTimer = Time.time + FlowerSpawnTime;
        EnemySpawnTimer = Time.time + EnemySpawnTimer;
        GameOverText.text = "";
        string jsonString;

        if (!File.Exists(FilePath))
        {
            jsonString = CreateNewScoreFile(FilePath);
        
        }
        else
        {
            StreamReader reader = new StreamReader(FilePath);
            jsonString = reader.ReadLine();
            reader.Close();
        }

        HighScore = JsonUtility.FromJson<ScoreBoard>(jsonString);
        if (HighScore == null)
        {
            jsonString = CreateNewScoreFile(FilePath);
            HighScore = JsonUtility.FromJson<ScoreBoard>(jsonString);
        }
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
        UpdateScoreBoard();
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

    void UpdateScoreBoard()
    {
        HighScore.Scores.Add(Score);
        HighScore.Scores.Sort();
        while (HighScore.Scores.Count > 5)
        {
            HighScore.Scores.RemoveAt(0);
        }
        string jsonString = JsonUtility.ToJson(HighScore);
        StreamWriter writer = new StreamWriter(FilePath);
        writer.Write(jsonString);
        writer.Close();
    }

}
