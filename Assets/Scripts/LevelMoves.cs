using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelMoves : Level
{

    public int numMoves;
    public int targetScore;

    private int movesUsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        type = LevelType.MOVES;
        hud.SetLevelType(type);
        hud.SetScore(currentScore);
        hud.SetTarget(targetScore);
        hud.SetRemaining(numMoves);
        //Debug.Log("Number of moves" + numMoves + " " + "Target Score " +  targetScore);
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
        if (numMoves - movesUsed == 0)
        {
            if(currentScore > targetScore)
            {
                GameWin();
            }
            else GameLose();
        }
    }
}
