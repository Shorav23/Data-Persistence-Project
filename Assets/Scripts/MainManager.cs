using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

[Serializable]
public class SaveData
{
    public string playerName;
    public int score;
}

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    private string bestPlayerName;
    private string playerName;

    public Text ScoreText;
    public Text BestScore;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int score;
    
    private bool m_GameOver = false;

    public SaveData saveData = new SaveData();
    //saveData.playerName = "Hello";
    // Start is called before the first frame update
    void Start()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/savefile.json");

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        bestPlayerName = saveData.playerName;
        score = saveData.score;

        playerName = MenuHandler.Instance.curPlayerName;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        BestScore.text = "Best Score is by " + bestPlayerName + $" : {score}";

    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > score)
        {
            saveData.playerName = playerName;
            saveData.score = m_Points;
            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }
}
