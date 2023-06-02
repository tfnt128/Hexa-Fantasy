using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public Text mpText;
    public Unit player;
    public int score = 0;
    int highScore = 0;

    void Start()
    {
        
        highScoreText.text = "HIGHSCORE: " + highScore.ToString();
    }
    private void Update()
    {
        scoreText.text = score.ToString() + " GOLD";
        mpText.text = player.MP.ToString();
    }
}
