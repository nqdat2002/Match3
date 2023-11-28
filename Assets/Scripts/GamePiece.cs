using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    // Start is called before the first frame update
    private int x, y;
    public int X { 
        get {
            return x;
        }
        set
        {
            if(IsMovable())
                x = value;
        }
    }
    public int Y { 
        get {
            return y;
        }
        set
        {
            if (IsMovable())
                y = value;
        }
    }


    private GameGrid.PieceType type;
    public GameGrid.PieceType Type
    {
        get { return type; }
    }


    private GameGrid gameGrid;

    public GameGrid GameGridRef { get { return gameGrid; } }

    // movable component
    private MovablePiece movableComponent;

    public MovablePiece MovableComponent
    {
        get { return movableComponent; }
        set { movableComponent = value; }
    }


    // colorPiece component
    private ColorPiece colorComponent;
    public ColorPiece ColorComponent
    {
        get { return colorComponent; }
    }

    private ClearablePiece clearableComponent;

    public ClearablePiece ClearableComponent
    {
        get { return clearableComponent; }
    }

    void Awake()
    {
        movableComponent = GetComponent<MovablePiece>();
        colorComponent = GetComponent<ColorPiece>();
        clearableComponent = GetComponent<ClearablePiece>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int _x, int _y, GameGrid _gamegrid, GameGrid.PieceType _type)
    {
        x = _x;
        y = _y;
        type = _type;
        gameGrid = _gamegrid;
    }
    void OnMouseEnter()
    {
        gameGrid.EnterPiece(this);
    }

    void OnMouseDown()
    {
        gameGrid.PressPiece(this);
    }
    void OnMouseUp()
    {
        gameGrid.ReleasePiece();
    }

    public bool IsMovable()
    {
        return movableComponent != null;
    }

    public bool IsColored()
    {
        return colorComponent != null;
    }

    public bool IsClearable()
    {
        return clearableComponent != null;
    }

}
