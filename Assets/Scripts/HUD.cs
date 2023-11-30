using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public Level level;
    public UnityEngine.UI.Text remainingText;
    public UnityEngine.UI.Text remainingSubtext;
    public UnityEngine.UI.Text targetText;
    public UnityEngine.UI.Text targetSubtext;
    public UnityEngine.UI.Text scoreText;
    public UnityEngine.UI.Image[] stars;

    public GameOver gameOver;

    private int starIndex = 0;
    //private bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i == starIndex)
            {
                stars[i].enabled = true;
            }
            else stars[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScore(int score)
    {
        scoreText.text = "" + score.ToString();
        int visialbeStar = 0;
        if (score >= level.score1Star && score < level.score2Star)
        {
            visialbeStar = 1;
        }
        else if (score >= level.score2Star && score < level.score3Star)
        {
            visialbeStar = 2;
        }
        else if (score >= level.score3Star) visialbeStar = 3;



        for(int i = 0; i < stars.Length; i++)
        {
            if (i == visialbeStar)
            {
                stars[i].enabled = true;
            }
            else stars[i].enabled = false;
        }

        starIndex = visialbeStar;
    }


    public void SetTarget(int target)
    {
        targetText.text = target.ToString();
    }

    public void SetRemaining(int remaining)
    {
        remainingText.text = remaining.ToString();
    }

    public void  SetRemaining(string remaining)
    {
        remainingText.text = remaining;
    }

    public void SetLevelType(Level.LevelType type)
    {
        if (type == Level.LevelType.MOVES)
        {
            remainingSubtext.text = "remaining moves";
            targetSubtext.text = "target score";
        }
        else if (type == Level.LevelType.OBSTACLE)
        {
            remainingSubtext.text = "remaining moves";
            targetSubtext.text = "bubbles remaining";
        }
        else if (type == Level.LevelType.TIMER)
        {
            remainingSubtext.text = "remaining time";
            targetSubtext.text = "target score";
        }
    }

    public void OnGameWin(int score)
    {
        gameOver.ShowWin(score, starIndex);
        //isGameOver = true;

        if (starIndex > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name, 0))
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, starIndex);
        }
    }

    public void OnGameLose()
    {
        gameOver.ShowLose();
        //isGameOver = true;
    }
}
