using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public int SinglePointValue = 38;

    [Space]
    [SerializeField] int Score;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI BestScoreText;

    [Space]
    [SerializeField] GameObject WaitScreen;

    [Space]
    [SerializeField] GameObject GameOverScreen;

    
    void Start(){

        gameManager = this;

        WaitScreen.SetActive(true);
        GameOverScreen.SetActive(false);

        UpdateScores(0);
        SetScores();
    }

    void UpdateScores(int score){

        Score = score;
        ScoreText.text = $"Score: {Score}";
    }

    public void AddScore(){

        Score += SinglePointValue;   
        UpdateScores(Score);

    }

    public void EndGame(){

        SetScores();
        GameOverScreen.SetActive(true);
    }

    void SetScores(){

        if(PlayerPrefs.HasKey("SCORE"))
        {
            PlayerPrefs.SetInt("SCORE", Mathf.Max(PlayerPrefs.GetInt("SCORE"), Score));

        }

        else{
            
            PlayerPrefs.SetInt("SCORE", 0);
        }

        BestScoreText.text = $"Score: {Score} <br>Best Score: {PlayerPrefs.GetInt("SCORE")}";
    }

    public void Home(){

        SceneManager.LoadScene(0);
    }

    public void RestartGame(){

        SceneManager.LoadScene(1);
    }

    public void StartGame(){

        WaitScreen.SetActive(false);
    }

    public void Pause(){

        WaitScreen.SetActive(true);
    }
}