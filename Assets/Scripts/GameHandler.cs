using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.IO;


[System.Serializable]
public class ScoreBoard
{//add date later
   public List<Score> ScoreList;

}

[System.Serializable]
public class Score
{
    public int score;
    public string date;
}
public class GameHandler : MonoBehaviour
{
   
    [Header("Flower Settings")]
    [SerializeField]
    float FlowerSpawnTime;
    float FlowerSpawnTimer;
    [SerializeField]
    Spawner FlowerSpawner;
    int FlowersSpawned = 0;
    [Header("Enemy settings")]
    [SerializeField]
    float EnemySpawnTime;
    float EnemySpawnTimer;
    [SerializeField]
    float EnemyMinSpawnTime;
    int EnemiesSpawned = 0;
    [SerializeField]   
    Spawner EnemySpawner;
    public static GameHandler Instance;
    public UnityEvent GameOver;
    bool IsGameOver = false;
    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI GameOverText;
    int Points = 0;
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
        scoreBoard.ScoreList = new List<Score>();
        Score score = new Score();
        score.score = 1000;
        score.date = System.DateTime.Today.ToString("dd / MM / yyyy");
        scoreBoard.ScoreList.Add(score);
        string jsonString = JsonUtility.ToJson(scoreBoard);
        StreamWriter writer = new StreamWriter(File.Create(filePath));
        writer.Write(jsonString);
        writer.Flush();
        writer.Close();
        return jsonString;
    }


    private void Start()
    {
        FilePath = Application.persistentDataPath + "/score.txt";
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
            FlowersSpawned++;
            FlowerSpawnTimer = Time.time + FlowerSpawnTime * FlowersSpawned * 0.95f;
        }
        if (EnemySpawnTimer < Time.time)
        {
            EnemySpawner.Spawn();
            EnemiesSpawned++;
            EnemySpawnTime *= 0.99f;
            EnemySpawnTime = Mathf.Clamp(EnemySpawnTimer,EnemyMinSpawnTime, EnemySpawnTimer);
            EnemySpawnTimer = Time.time + EnemySpawnTime;
        }
    }

    public void AddScore(int _points)
    {
        if (IsGameOver)
            return;
        Points += _points;
        ScoreText.text = "Score:" + Points;
    }

    public void StartGameOver()
    {
        if (IsGameOver)
            return;
        UpdateScoreBoard();
        GameOverText.text = "Game Over";
        GameOver.Invoke();
        IsGameOver = true;
        Invoke("RestartGame", 3f);
    }


    void RestartGame()
    {
        Transition.instance.Play();
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
        Score score = new Score();
        score.score = Points;
        score.date = System.DateTime.Today.ToString("dd / MM / yyyy");

        HighScore.ScoreList.Add(score);
        HighScore.ScoreList.Sort((a,b)=> a.score.CompareTo(b.score));
        while (HighScore.ScoreList.Count > 5)
        {
            HighScore.ScoreList.RemoveAt(0);
        }
        string jsonString = JsonUtility.ToJson(HighScore);
        StreamWriter writer = new StreamWriter(FilePath);
        writer.Write(jsonString);
        writer.Close();
    }

}
