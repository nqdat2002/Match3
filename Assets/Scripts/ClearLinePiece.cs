using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLinePiece : ClearablePiece
{
    // Start is called before the first frame update
    public bool isRow;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Clear()
    {
        base.Clear();
        if (isRow)
        {
            piece.GameGridRef.ClearRow(piece.Y);
        }
        else
        {
            piece.GameGridRef.ClearColumn(piece.X);
        }
    }
}
