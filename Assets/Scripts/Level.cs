using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public enum LevelType
    {
        TIMER,
        OBSTACLE,
        MOVES,
    };
    public GameGrid gamegrid;
    public HUD hud;

    public int score1Star, score2Star, score3Star;

    protected LevelType type;
    public LevelType Type
    {
        get { return type; }
        set { type = value; }
    }

    protected int currentScore;
    protected bool didWin;
    // Start is called before the first frame update
    void Start()
    {
        hud.SetScore(currentScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void GameWin()
    {
        Debug.Log("You Win.");
        hud.OnGameWin(currentScore);
        gamegrid.GameOver();
        didWin = true;
        StartCoroutine(WaitForGridFill());
    }

    public virtual void GameLose() 
    {
        Debug.Log("You Lose.");
        hud.OnGameLose();
        gamegrid.GameOver();
        didWin = false;
    }

    public virtual void OnMove()
    {
        //Debug.Log("You Move");
    }

    public virtual void OnPieceCleared(GamePiece piece)
    {
        // Update Score
        currentScore += piece.score;
        // Debug.Log("CurrentScore " + currentScore);
        hud.SetScore(currentScore);
    }

    protected virtual IEnumerator WaitForGridFill()
    {
        while (gamegrid.IsFilling)
        {
            yield return 0;
        }

        if (didWin)
        {
            hud.OnGameWin(currentScore);
        } else
        {
            hud.OnGameLose();
        }
    }
}
