using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ColorPiece;
using Random = UnityEngine.Random;

public class GameGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public enum PieceType
    {
        EMPTY,
        NORMAL,
        BUBBLE,
        ROW_CLEAR,
        COLUMN_CLEAR,
        COUNT,
    };

    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    }

    public int xDim, yDim;
    public float fillTime;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    private Dictionary<PieceType, GameObject> piecePrefabDict;
    private GamePiece[,] pieces;

    private bool inverse = false;

    private GamePiece pressedPiece;
    private GamePiece enterdPiece;
    public bool IsFilling { get; private set; }

    void Start()
    {
        piecePrefabDict = new Dictionary<PieceType, GameObject>();

        for (int i = 0; i < piecePrefabs.Length; ++i)
        {
            if (!piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        for (int x = 0; x < xDim; ++x)
        {
            for (int y = 0; y < yDim; ++y)
            {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                // (x, y, 0)
                background.transform.parent = transform;
            }
        }
        pieces = new GamePiece[xDim, yDim];

        for (int x = 0; x < xDim; ++x)
        {
            for (int y = 0; y < yDim; y++)
            {
                // Gen normal piece

                //GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], Vector3.zero, Quaternion.identity);
                //newPiece.name = "Piece(" + x + ", " + y + ")";
                //Console.WriteLine(newPiece.name);
                //newPiece.transform.parent = transform;
                //pieces[x, y] = newPiece.GetComponent<GamePiece>();
                //pieces[x, y].Init(x, y, this, PieceType.NORMAL);

                //if (pieces[x, y].IsMovable())
                //{
                //    pieces[x, y].MovableComponent.Move(x, y);
                //}

                //if (pieces[x, y].IsColored())
                //{
                //    pieces[x, y].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, pieces[x, y].ColorComponent.NumColors));
                //}


                // gen empty & normal piece
                SpawNewPiece(x, y, PieceType.EMPTY);

            }
        }
        //Fill();

        // mid of grid 9x9
        //Destroy(pieces[4, 4].gameObject);
        //SpawNewPiece(4, 4, PieceType.BUBBLE);

        // some pos demo
        Destroy(pieces[1, 4].gameObject);
        SpawNewPiece(1, 4, PieceType.BUBBLE);

        Destroy(pieces[2, 4].gameObject);
        SpawNewPiece(2, 4, PieceType.BUBBLE);

        Destroy(pieces[3, 4].gameObject);
        SpawNewPiece(3, 4, PieceType.BUBBLE);

        Destroy(pieces[5, 4].gameObject);
        SpawNewPiece(5, 4, PieceType.BUBBLE);

        Destroy(pieces[6, 4].gameObject);
        SpawNewPiece(6, 4, PieceType.BUBBLE);

        Destroy(pieces[7, 4].gameObject);
        SpawNewPiece(7, 4, PieceType.BUBBLE);

        Destroy(pieces[4, 0].gameObject);
        SpawNewPiece(4, 0, PieceType.BUBBLE);

        StartCoroutine(Fill());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Fill()
    {
        bool needRefill = true;
        while (needRefill)
        {
            yield return new WaitForSeconds(fillTime * 5);
            while (FillStep())
            {
                inverse = !inverse;
                yield return new WaitForSeconds(fillTime);
            }
            needRefill = ClearAllValidMatches();
        }
    }

    public bool FillStep()
    {
        bool movePiece = false;
        for (int y = yDim - 2; y >= 0; y--)
        {
            for (int loopX = 0; loopX < xDim; ++loopX)
            {
                int x = loopX;
                if (inverse)
                {
                    x = xDim - 1 - loopX;
                }

                GamePiece piece = pieces[x, y];
                if (piece.IsMovable()) {
                    GamePiece pieceBelow = pieces[x, y + 1];

                    if (pieceBelow.Type == PieceType.EMPTY)
                    {
                        Destroy(pieceBelow.gameObject); // destroy empty
                        piece.MovableComponent.Move(x, y + 1, fillTime);
                        pieces[x, y + 1] = piece;
                        SpawNewPiece(x, y, PieceType.EMPTY);
                        movePiece = true;
                    }
                    else
                    {
                        for (int diag = -1; diag <= 1; diag++)
                        {
                            if (diag != 0)
                            {
                                int diagX = x + diag;
                                if (inverse) diagX = x - diag;
                                if (diagX >= 0 && diagX < xDim)
                                {
                                    GamePiece diagonalPiece = pieces[diagX, y + 1];
                                    if (diagonalPiece.Type == PieceType.EMPTY)
                                    {
                                        bool hasPieceAbove = true;
                                        for (int aboveY = y; aboveY >= 0; --aboveY)
                                        {
                                            GamePiece pieceAbove = pieces[diagX, aboveY];
                                            if (pieceAbove.IsMovable()) break;
                                            else if (!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.EMPTY)
                                            {
                                                hasPieceAbove = false;
                                                break;
                                            }
                                        }
                                        if (!hasPieceAbove)
                                        {
                                            Destroy(diagonalPiece.gameObject);
                                            piece.MovableComponent.Move(diagX, y + 1, fillTime);
                                            pieces[diagX, y + 1] = piece;
                                            SpawNewPiece(x, y, PieceType.EMPTY);
                                            movePiece = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int x = 0; x < xDim; ++x)
        {
            GamePiece pieceBelow = pieces[x, 0];
            if (pieceBelow.Type == PieceType.EMPTY)
            {
                Destroy(pieceBelow.gameObject); // destroy empty
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;
                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].MovableComponent.Move(x, 0, fillTime);
                pieces[x, 0].ColorComponent.SetColor((ColorPiece.ColorType)Random.Range(0, pieces[x, 0].ColorComponent.NumColors));
                movePiece = true;
            }
        }
        return movePiece;
    }


    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(transform.position.x - xDim / 2.0f + x, transform.position.y + yDim / 2.0f - y);
    }


    // Function Spaw New Piece

    public GamePiece SpawNewPiece(int x, int y, PieceType type)
    {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[type], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;
        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, type);

        return pieces[x, y];
    }

    public bool isAdjacent(GamePiece piece1, GamePiece piece2)
    {
        return (piece1.X == piece2.X && (int)Math.Abs(piece1.Y - piece2.Y) == 1) 
            || (piece1.Y == piece2.Y && (int)Math.Abs(piece1.X - piece2.X) == 1);
    } 

    public void SwapPieces(GamePiece piece1, GamePiece piece2) 
    {
        if (piece1.IsMovable() && piece2.IsMovable())
        {

            pieces[piece1.X, piece1.Y] = piece2;
            pieces[piece2.X, piece2.Y] = piece1;

            if (GetMatch(piece1, piece2.X, piece2.Y) != null || GetMatch(piece2, piece1.X, piece1.Y) != null)
            {
                int piece1x = piece1.X;
                int piece1y = piece1.Y;

                piece1.MovableComponent.Move(piece2.X, piece2.Y, fillTime);
                piece2.MovableComponent.Move(piece1x, piece1y, fillTime);
                ClearAllValidMatches();

                if (piece1.Type == PieceType.ROW_CLEAR || piece1.Type == PieceType.COLUMN_CLEAR)
                {
                    ClearPiece(piece1.X, piece1.Y);
                }

                if (piece2.Type == PieceType.ROW_CLEAR || piece2.Type == PieceType.COLUMN_CLEAR)
                {
                    ClearPiece(piece2.X, piece2.Y);
                }

                enterdPiece = null;
                pressedPiece = null;
                StartCoroutine(Fill());
            }
            else
            {
                pieces[piece1.X, piece1.Y] = piece1;
                pieces[piece2.X, piece2.Y] = piece2;
            }

            //Console.WriteLine(piece1.X);
            //Console.WriteLine(piece1.Y);
            //Console.WriteLine(piece2.X);
            //Console.WriteLine(piece2.Y);

            //Console.WriteLine("Press swapped");

        }
    }  

    public void PressPiece(GamePiece piece)
    {
        pressedPiece = piece;
    }

    public void EnterPiece(GamePiece piece)
    {
        enterdPiece = piece;
    }

    public void ReleasePiece()
    {
        if (isAdjacent(pressedPiece, enterdPiece))
        {
            SwapPieces(pressedPiece, enterdPiece);
        }
    }

    public List<GamePiece> GetMatch(GamePiece piece, int newX, int newY)
    {
        //if (piece.IsColored())
        //{
        //    ColorPiece.ColorType color = piece.ColorComponent.Color;
        //    List<GamePiece> horizontalPieces = new List<GamePiece>();
        //    List<GamePiece> verticalPieces = new List<GamePiece>();
        //    List<GamePiece> matchingPieces = new List<GamePiece>();

        //    // first check about horizontal
        //    horizontalPieces.Add(piece);
        //    for (int dir = 0; dir <= 1; ++dir)
        //    {
        //        for(int xOffset = 1; xOffset < xDim; ++xOffset)
        //        {
        //            int x;
        //            if (dir == 0)
        //            {
        //                x = newX - xOffset; // left
        //            }
        //            else
        //            {
        //                x = newX + xOffset; // right
        //            }

        //            if (x < 0 || x >= xDim)
        //            {
        //                break;
        //            }

        //            if (pieces[x, newY].IsColored() && pieces[x, newY].ColorComponent.Color == color)
        //            {
        //                horizontalPieces.Add(pieces[x, newY]);
        //            }
        //            else break;
        //        }
        //    }
        //    if (horizontalPieces.Count >= 3)
        //    {
        //        for (int i = 0; i < horizontalPieces.Count; ++i)
        //        {
        //            matchingPieces.Add(horizontalPieces[i]);
        //        }
        //    }


        //    // Traverse vertically if we found a match (for L and T shape)

        //    if (horizontalPieces.Count >= 3)
        //    {
        //        for (int i = 0; i < horizontalPieces.Count; ++i)
        //        {
        //            for (int dir = 0; dir <= 1; ++dir)
        //            {
        //                for (int yOffset = 1; yOffset < yDim; ++yOffset)
        //                {
        //                    int y;
        //                    if(dir == 0) 
        //                    {
        //                        y = newY - yOffset; // up
        //                    }
        //                    else
        //                    {
        //                        y = newY + yOffset; // down
        //                    }

        //                    if (y < 0 || y >= yDim)
        //                    {
        //                        break;
        //                    }

        //                    if (pieces[horizontalPieces[i].X, y].IsColored() && pieces[horizontalPieces[i].X, y].ColorComponent.Color == color)
        //                    {
        //                        verticalPieces.Add(pieces[horizontalPieces[i].X, y]);
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //            if (verticalPieces.Count < 2)
        //            {
        //                verticalPieces.Clear();
        //            }
        //            else
        //            {
        //                //matchingPieces.AddRange(verticalPieces);
        //                for (int j = 0; j < verticalPieces.Count; ++j)
        //                {
        //                    matchingPieces.Add(verticalPieces[j]);
        //                }
        //                break;
        //            }
        //        }
        //    }


        //    if(matchingPieces.Count >= 3)
        //    {
        //        return matchingPieces;
        //    }



        //    // vertical
        //    horizontalPieces.Clear();
        //    verticalPieces.Clear();
        //    verticalPieces.Add(piece);
        //    for (int dir = 0; dir <= 1; ++dir)
        //    {
        //        for (int yOffset = 1; yOffset < yDim; ++yOffset)
        //        {
        //            int y;
        //            if (dir == 0)
        //            {
        //                y = newX - yOffset; // up
        //            }
        //            else
        //            {
        //                y = newX + yOffset; // down
        //            }

        //            if (y < 0 || y >= yDim)
        //            {
        //                break;
        //            }

        //            if (pieces[newX, y].IsColored() && pieces[newX, y].ColorComponent.Color == color)
        //            {
        //                verticalPieces.Add(pieces[newX, y]);
        //            }
        //            else break;
        //        }
        //    }
        //    if (verticalPieces.Count >= 3)
        //    {
        //        for (int i = 0; i < verticalPieces.Count; ++i)
        //        {
        //            matchingPieces.Add(verticalPieces[i]);
        //        }
        //    }


        //    if (verticalPieces.Count >= 3)
        //    {
        //        for (int i = 0; i < verticalPieces.Count; i++)
        //        {
        //            for (int dir = 0; dir <= 1; dir++)
        //            {
        //                for (int xOffset = 1; xOffset < yDim; xOffset++)
        //                {
        //                    int x;

        //                    if (dir == 0)
        //                    { // left
        //                        x = newX - xOffset;
        //                    }
        //                    else
        //                    { // right
        //                        x = newX + xOffset;
        //                    }

        //                    if (x < 0 || x >= xDim)
        //                    {
        //                        break;
        //                    }

        //                    if (pieces[x, verticalPieces[i].Y].IsColored() && pieces[x, verticalPieces[i].Y].ColorComponent.Color == color)
        //                    {
        //                        horizontalPieces.Add(pieces[x, verticalPieces[i].Y]);
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //            }

        //            if (horizontalPieces.Count < 2)
        //            {
        //                horizontalPieces.Clear();
        //            }
        //            else
        //            {
        //                // matching.AddRange(horizontalPieces);
        //                for (int j = 0; j < horizontalPieces.Count; ++j)
        //                {
        //                    matchingPieces.Add(horizontalPieces[j]);
        //                }
        //                break;
        //            }
        //        }
        //    }

        //    if (matchingPieces.Count >= 3)
        //    {
        //        return matchingPieces;
        //    }
        //}
        //return null;
        if (!piece.IsColored()) return null;
        var color = piece.ColorComponent.Color;
        var horizontalPieces = new List<GamePiece>();
        var verticalPieces = new List<GamePiece>();
        var matchingPieces = new List<GamePiece>();

        // First check horizontal
        horizontalPieces.Add(piece);

        for (int dir = 0; dir <= 1; dir++)
        {
            for (int xOffset = 1; xOffset < xDim; xOffset++)
            {
                int x;

                if (dir == 0)
                { // Left
                    x = newX - xOffset;
                }
                else
                { // right
                    x = newX + xOffset;
                }

                // out-of-bounds
                if (x < 0 || x >= xDim) { break; }

                // piece is the same color?
                if (pieces[x, newY].IsColored() && pieces[x, newY].ColorComponent.Color == color)
                {
                    horizontalPieces.Add(pieces[x, newY]);
                }
                else
                {
                    break;
                }
            }
        }

        if (horizontalPieces.Count >= 3)
        {
            matchingPieces.AddRange(horizontalPieces);
        }

        // Traverse vertically if we found a match (for L and T shape)
        if (horizontalPieces.Count >= 3)
        {
            for (int i = 0; i < horizontalPieces.Count; i++)
            {
                for (int dir = 0; dir <= 1; dir++)
                {
                    for (int yOffset = 1; yOffset < yDim; yOffset++)
                    {
                        int y;

                        if (dir == 0)
                        { // Up
                            y = newY - yOffset;
                        }
                        else
                        { // Down
                            y = newY + yOffset;
                        }

                        if (y < 0 || y >= yDim)
                        {
                            break;
                        }

                        if (pieces[horizontalPieces[i].X, y].IsColored() && pieces[horizontalPieces[i].X, y].ColorComponent.Color == color)
                        {
                            verticalPieces.Add(pieces[horizontalPieces[i].X, y]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (verticalPieces.Count < 2)
                {
                    verticalPieces.Clear();
                }
                else
                {
                    matchingPieces.AddRange(verticalPieces);
                    break;
                }
            }
        }

        if (matchingPieces.Count >= 3)
        {
            return matchingPieces;
        }


        // Didn't find anything going horizontally first,
        // so now check vertically
        horizontalPieces.Clear();
        verticalPieces.Clear();
        verticalPieces.Add(piece);

        for (int dir = 0; dir <= 1; dir++)
        {
            for (int yOffset = 1; yOffset < xDim; yOffset++)
            {
                int y;

                if (dir == 0)
                { // Up
                    y = newY - yOffset;
                }
                else
                { // Down
                    y = newY + yOffset;
                }

                // out-of-bounds
                if (y < 0 || y >= yDim) { break; }

                // piece is the same color?
                if (pieces[newX, y].IsColored() && pieces[newX, y].ColorComponent.Color == color)
                {
                    verticalPieces.Add(pieces[newX, y]);
                }
                else
                {
                    break;
                }
            }
        }

        if (verticalPieces.Count >= 3)
        {
            matchingPieces.AddRange(verticalPieces);
        }

        // Traverse horizontally if we found a match (for L and T shape)
        if (verticalPieces.Count >= 3)
        {
            for (int i = 0; i < verticalPieces.Count; i++)
            {
                for (int dir = 0; dir <= 1; dir++)
                {
                    for (int xOffset = 1; xOffset < yDim; xOffset++)
                    {
                        int x;

                        if (dir == 0)
                        { // Left
                            x = newX - xOffset;
                        }
                        else
                        { // Right
                            x = newX + xOffset;
                        }

                        if (x < 0 || x >= xDim)
                        {
                            break;
                        }

                        if (pieces[x, verticalPieces[i].Y].IsColored() && pieces[x, verticalPieces[i].Y].ColorComponent.Color == color)
                        {
                            horizontalPieces.Add(pieces[x, verticalPieces[i].Y]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (horizontalPieces.Count < 2)
                {
                    horizontalPieces.Clear();
                }
                else
                {
                    matchingPieces.AddRange(horizontalPieces);
                    break;
                }
            }
        }

        if (matchingPieces.Count >= 3)
        {
            return matchingPieces;
        }

        return null;
    }


    private bool ClearAllValidMatches()
    {
        bool needsRefill = false;

        for (int y = 0; y < yDim; y++)
        {
            for (int x = 0; x < xDim; x++)
            {
                if (!pieces[x, y].IsClearable()) continue;

                List<GamePiece> match = GetMatch(pieces[x, y], x, y);

                if (match == null) continue;

                //for (int i = 0; i < match.Count; ++i)
                //{
                //    if (ClearPiece(match[i].X, match[i].Y))
                //    {
                //        needsRefill = true;
                //    }
                //}
                PieceType specialPieceType = PieceType.COUNT;
                GamePiece randomPiece = match[Random.Range(0, match.Count)];
                int specialPieceX = randomPiece.X;
                int specialPieceY = randomPiece.Y;

                // Spawning special pieces
                if (match.Count == 4)
                {
                    if (pressedPiece == null || enterdPiece == null)
                    {
                        specialPieceType = (PieceType)Random.Range((int)PieceType.ROW_CLEAR, (int)PieceType.COLUMN_CLEAR);
                    }
                    else if (pressedPiece.Y == enterdPiece.Y)
                    {
                        specialPieceType = PieceType.ROW_CLEAR;
                    }
                    else
                    {
                        specialPieceType = PieceType.COLUMN_CLEAR;
                    }
                } 
                //// Spawning a rainbow piece
                //else if (match.Count >= 5)
                //{
                //    specialPieceType = PieceType.Rainbow;
                //}

                foreach (var gamePiece in match)
                {
                    if (!ClearPiece(gamePiece.X, gamePiece.Y)) continue;

                    needsRefill = true;

                    if (gamePiece != pressedPiece && gamePiece != enterdPiece) continue;

                    specialPieceX = gamePiece.X;
                    specialPieceY = gamePiece.Y;
                }

                // Setting their colors
                if (specialPieceType == PieceType.COUNT) continue;

                Destroy(pieces[specialPieceX, specialPieceY]);
                GamePiece newPiece = SpawNewPiece(specialPieceX, specialPieceY, specialPieceType);

                if ((specialPieceType == PieceType.ROW_CLEAR || specialPieceType == PieceType.COLUMN_CLEAR)
                    && newPiece.IsColored() && match[0].IsColored())
                {
                    newPiece.ColorComponent.SetColor(match[0].ColorComponent.Color);
                }
                //else if (specialPieceType == PieceType.Rainbow && newPiece.IsColored())
                //{
                //    newPiece.ColorComponent.SetColor(ColorType.ANY);
                //}
            }
        }

        return needsRefill;
    }

    public bool ClearPiece(int x, int y)
    {
        if (!pieces[x, y].IsClearable() || pieces[x, y].ClearableComponent.IsBeingCleared) return false;

        pieces[x, y].ClearableComponent.Clear();
        SpawNewPiece(x, y, PieceType.EMPTY);
        ClearObstacles(x, y);
        return true;
    }

    private void ClearObstacles(int x, int y)
    {
        for (int adjacentX = x - 1; adjacentX <= x + 1; adjacentX++)
        {
            if (adjacentX == x || adjacentX < 0 || adjacentX >= xDim) continue;

            if (pieces[adjacentX, y].Type != PieceType.BUBBLE || !pieces[adjacentX, y].IsClearable()) continue;

            pieces[adjacentX, y].ClearableComponent.Clear();
            SpawNewPiece(adjacentX, y, PieceType.EMPTY);
        }

        for (int adjacentY = y - 1; adjacentY <= y + 1; adjacentY++)
        {
            if (adjacentY == y || adjacentY < 0 || adjacentY >= yDim) continue;

            if (pieces[x, adjacentY].Type != PieceType.BUBBLE || !pieces[x, adjacentY].IsClearable()) continue;

            pieces[x, adjacentY].ClearableComponent.Clear();
            SpawNewPiece(x, adjacentY, PieceType.EMPTY);
        }
    }


    public void ClearRow(int row)
    {
        for (int x = 0; x < xDim; x++)
        {
            ClearPiece(x, row);
        }
    }

    public void ClearColumn(int column)
    {
        for (int y = 0; y < yDim; y++)
        {
            ClearPiece(column, y);
        }
    }

    public void ClearColor(ColorType color)
    {
        for (int x = 0; x < xDim; x++)
        {
            for (int y = 0; y < yDim; y++)
            {
                if ((pieces[x, y].IsColored() && pieces[x, y].ColorComponent.Color == color)
                    || (color == ColorType.ANY))
                {
                    ClearPiece(x, y);
                }
            }
        }
    }
}
