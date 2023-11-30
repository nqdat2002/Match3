using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacles : Level
{
    public int numMoves;
    public GameGrid.PieceType[] obstacleTypes;
    private int movesUsed = 0;
    private int numObstaclesLeft;


    // Start is called before the first frame update
    void Start()
    {
        type = LevelType.OBSTACLE;
        for (int i = 0; i < obstacleTypes.Length; i++)
        {
            numObstaclesLeft += gamegrid.GetPiecesOfType(obstacleTypes[i]).Count;
        }

        hud.SetLevelType(type);
        hud.SetScore(currentScore);
        hud.SetTarget(numObstaclesLeft);
        hud.SetRemaining(numMoves);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnMove()
    {
        base.OnMove();
        movesUsed += 1;
        //Debug.Log("Moves remaining :" + (numMoves - movesUsed));
        hud.SetRemaining(numMoves - movesUsed);
        if (numMoves - movesUsed == 0 && numObstaclesLeft > 0)
        {
            GameLose();
        }
    }

    public override void OnPieceCleared(GamePiece piece)
    {
        base.OnPieceCleared(piece); 

        for (int i = 0; i < obstacleTypes.Length; ++i)
        {
            if (obstacleTypes[i] == piece.Type)
            {
                numObstaclesLeft -= 1;
                hud.SetTarget(numObstaclesLeft);
                if (numObstaclesLeft == 0)
                {
                    currentScore += 1000 * (numMoves - movesUsed);
                    //Debug.Log("Current score: " + currentScore);
                    hud.SetScore(currentScore);
                    GameWin();
                }
            }
        }
    }
}
