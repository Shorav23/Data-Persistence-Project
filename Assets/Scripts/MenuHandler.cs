using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

public class MenuHandler : MonoBehaviour
{
    public Text BestScore;
    private SaveData saveData;
    private int score;
    public string playerName;
    public string curPlayerName;

    public InputField inputField;
    // Start is called before the first frame update

    public static MenuHandler Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        string path = Application.persistentDataPath + "/savefile.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);
                score = saveData.score;
                playerName = saveData.playerName;
            }

                    inputField.text = playerName;
            
    }

    // Update is called once per frame
    void Update()
    {
        
        BestScore.text = "Best Score is by " + playerName + $" : {score}";
    }

    public void OnClickStart()
    {
        curPlayerName = inputField.text.ToString();
        SceneManager.LoadScene(0);
    }
}
