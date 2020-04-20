using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class HighScoreReader : MonoBehaviour
{
    [SerializeField]
    GameObject TextPrefab;
    [SerializeField]
    Transform PanelTransform;
    string FilePath;
    void Start()
    {
        FilePath = Application.persistentDataPath + "/score.txt";
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

        ScoreBoard HighScore = JsonUtility.FromJson<ScoreBoard>(jsonString);
        if (HighScore == null)
        {
            jsonString = CreateNewScoreFile(FilePath);
            HighScore = JsonUtility.FromJson<ScoreBoard>(jsonString);
        }

        int j = 0;
        for (int i = HighScore.ScoreList.Count-1; i >=0; i--)
        {
            GameObject textObject = Instantiate(TextPrefab, PanelTransform);
            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
            text.text = $"{GetPlacementName(j)} \nScore: {HighScore.ScoreList[i].score} \nDate: {HighScore.ScoreList[i].date}";
            j++;
        }
    }

    string GetPlacementName(int id)
    {
        switch (id)
        {
            case 4:
                return "- First Place -";
            case 3:
                return "- Second Place -";
            case 2:
                return "- Third Place -";
            case 1:
                return "- Fourth Place -";
            case 0:
                return "- Fifth Place -";
            default: 
                return "- First Place -";
        }
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
};
