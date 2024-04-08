using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Connect4Manager : MonoBehaviour
{
    [SerializeField] GameObject boardObject;
    [SerializeField] TextMeshProUGUI winText, resetText;

    private Connect4Board board;
    private bool gameOver;
    private IEnumerable<(InputAction, Action)> inputMethods;

    private void Awake()
    {
        board = boardObject.GetComponent<Connect4Board>();

        Action[] inputActions = new Action[] { MoveLeft, MoveRight, Drop, RestartGame };
        inputMethods = new Connect4Controls().Player.Get().Zip(inputActions, (a, b) => (a, b));
    }

    // Enables all inputs and attaches their actions as event handlers
    private void OnEnable()
    {
        foreach ((InputAction input, Action action) in inputMethods)
        {
            input.Enable();
            input.performed += _ => action();
        }
    }

    // Disables all inputs and detaches their event handlers
    private void OnDisable()
    {
        foreach ((InputAction input, Action action) in inputMethods)
        {
            input.Disable();
            input.performed -= _ => action();
        }
    }

    // Moves the held piece to the nearest column to the left if there's one available
    private void MoveLeft()
    {
        var openLeftColumns = board.OpenColumns.Where(x => x < board.Column);
        if (!openLeftColumns.Any()) return; // If there are no open columns to the left, do nothing
        board.Column = openLeftColumns.Last(); // Move the held piece to the nearest option
    }

    // Moves the held piece to the nearest column to the left if there's one available
    private void MoveRight()
    {
        var openRightColumns = board.OpenColumns.Where(x => x > board.Column);
        if (!openRightColumns.Any()) return; // If there are no open columns to the right, do nothing
        board.Column = openRightColumns.First(); // Move the held piece to the nearest option
    }

    // Drops the player's piece into the current column
    private void Drop()
    {
        if (gameOver || !board.OnValidColumn) return; // If the game is over or the column is full, the player can't drop a piece

        Vector2 targetPosition = board.DropPiece(); // Drop the piece and get its position

        bool gameWon = board.CheckForWin(targetPosition); // Check if the player won the game
        bool boardFull = board.OpenColumns.Length == 0; // Check if the board is full (meaning a draw)

        if (gameWon)
        {
            board.HighlightWin(); // Highlight the winning pieces

            // Change the values of the ending text to represent the victory
            winText.color = board.PlayerColor;
            winText.text = $"Player {board.Player + 1} wins!";

            AudioManager.Play(1);

            EndGame();
        }
        else if (boardFull)
        {
            // Change the values of the ending text to represent the draw
            winText.color = resetText.color;
            winText.text = "It's a draw!";

            AudioManager.Play(2);

            EndGame();
        }

        board.Player = (board.Player + 1) % board.PlayerCount; // Switch to the next player
    }

    // Restarts the game by resetting game state and UI elements
    private void RestartGame()
    {
        if (!gameOver) return; // The game can only reset if it's already ended

        gameOver = false; // Mark the game as not over
        board.HeldPieceActive = true; // Enable held piece

        // Hide game-end text
        winText.gameObject.SetActive(false);
        resetText.gameObject.SetActive(false);

        // Reset the player & column position to default values
        board.Player = 0;
        board.Column = 3;

        board.Clear(); // Clear the board
    }

    // Ends the game and displays the appropriate UI elements
    private void EndGame()
    {
        gameOver = true; // Mark the game as over
        board.HeldPieceActive = false; // Hide held piece

        // Show game-end text
        winText.gameObject.SetActive(true);
        resetText.gameObject.SetActive(true);
    }
}
