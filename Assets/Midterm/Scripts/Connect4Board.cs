using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static System.Linq.Enumerable;
using static UnityEngine.Quaternion;
using static UnityEngine.Vector2;
using static DG.Tweening.DOTween;

public class Connect4Board : MonoBehaviour
{
    private readonly Vector2[] directions = new Vector2[] { right, up, one, new Vector2(1, -1) };

    [SerializeField] GameObject columnPrefab, piecePrefab;
    [SerializeField] Color[] playerColors;
    [SerializeField] int cols, rows, winLength, column;

    private GameObject heldPiece, emptyRect;
    private readonly HashSet<Transform> winningPieces = new();

    private int player;

    private Transform CurrentColumnCursor => transform.GetChild(column).Find("Cursor");

    private Transform CurrentColumnPieces => GetColumnPieces(column);

    public int PlayerCount => playerColors.Length;

    public int[] OpenColumns => Range(0, cols).Where(col => GetColumnPieces(col).childCount < rows).ToArray();

    public int Column
    {
        get => column;
        set
        {
            column = value;
            heldPiece.transform.SetParent(CurrentColumnCursor, false);
        }
    }

    public bool OnValidColumn => CurrentColumnPieces.childCount < rows;

    public bool HeldPieceActive
    {
        set => heldPiece.SetActive(value);
    }

    public int Player
    {
        get => player;
        set
        {
            player = value;
            heldPiece.GetComponent<Image>().color = playerColors[player];
        }
    }

    public Color PlayerColor => playerColors[player];

    // Prepares the game when it begins running
    private void Awake()
    {
        // Creates all of the board's columns
        for (int i = 0; i < cols; i++)
        {
            Instantiate(columnPrefab, transform);
        }

        // Creates an empty Game Object used for falling animations
        emptyRect = new GameObject().AddComponent<RectTransform>().gameObject;

        // Makes a piece belonging to the first player and positions it above the board
        piecePrefab.GetComponent<Image>().color = playerColors[0];
        heldPiece = Instantiate(piecePrefab, CurrentColumnCursor);
    }

    // Drops the player's piece and returns the position where it fell
    public Vector2 DropPiece()
    {
        // Places an empty game object in the column layout group & forces it to reposition immediately
        GameObject destination = Instantiate(emptyRect, CurrentColumnPieces);
        LayoutRebuilder.ForceRebuildLayoutImmediate(CurrentColumnPieces.GetComponent<RectTransform>());

        // Clones the held piece at its original position & with the empty game object as its parent
        GameObject fallingPiece = Instantiate(heldPiece, heldPiece.transform.position, identity, destination.transform);

        // Completes any other drop animations that are still happening
        CompleteAll();

        // Makes the cloned piece fall to its new position in the column layout group
        var tween = fallingPiece.transform.DOLocalMoveY(0, 0.3f);
        tween.SetEase(Ease.InQuad);
        tween.OnComplete(() => AudioManager.Play(0));

        // Returns the position of where the piece will fall
        return new Vector2(column, destination.transform.GetSiblingIndex());
    }


    // Checks if the current player has won
    public bool CheckForWin(Vector2 droppedPos)
    {
        foreach (Vector2 direction in directions) // Check for a win in all four directions
        {
            for (int offset = 0; offset < winLength; offset++) // Check for a win at different offsets
            {
                // Get a list of positions that could make a win with this direction & offset
                var posChecks = Range(-offset, winLength).Select(delta => droppedPos + direction * delta);

                // If all the pieces at these positions belong to the player, they're "winning pieces"
                if (posChecks.All(CheckPosition)) winningPieces.UnionWith(posChecks.Select(GetPiece));
            }
        }

        // If there are any winning pieces, the player has won
        return winningPieces.Any();
    }

    // Highlights where the player won
    public void HighlightWin()
    {
        foreach (Transform piece in winningPieces) // Iterate through all the winning pieces
        {
            piece.GetChild(0).gameObject.SetActive(true); // Set the piece's highlight to active
        }
    }

    // Deletes all of the pieces currently in the board
    public void Clear()
    {
        KillAll(); // Stop all animations from the previous game
        foreach (Transform col in transform) // Iterate through each column
        {
            foreach (Transform piece in col.Find("Pieces")) // Iterate through the column
            {
                Destroy(piece.gameObject); // Destroy the game object
            }
        }
        winningPieces.Clear(); // There are no winning pieces anymore
    }

    // Returns a given column's "pieces" layout object
    private Transform GetColumnPieces(int col) => transform.GetChild(col).Find("Pieces");

    // Returns the piece object at a vector position, or null if it doesn't exist
    private Transform GetPiece(Vector2 pos)
    {
        // Determines if X is in bounds
        int x = (int)pos.x;
        if (x < 0 || x >= cols) return null;

        // Determines if Y is in bounds
        int y = (int)pos.y;
        Transform colPieces = GetColumnPieces(x);
        if (y < 0 || y >= colPieces.childCount) return null;

        // Returns the piece transform
        return colPieces.GetChild(y).GetChild(0);
    }

    // Checks if a piece at a certain position belongs to the current player
    private bool CheckPosition(Vector2 pos)
    {
        Transform piece = GetPiece(pos);
        return piece != null && piece.GetComponent<Image>().color == playerColors[player];
    }
}