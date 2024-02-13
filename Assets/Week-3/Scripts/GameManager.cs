using TMPro;
using UnityEngine;

namespace Battleship {
    public class GameManager : MonoBehaviour
    {
        //Random number generator used for making random board on reset
        readonly System.Random rand = new();

        //Create the starting grid
        [SerializeField]
        private int[,] grid = new int[,]
        {
            { 1, 1, 0, 0, 1 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 1 },
            { 1, 0, 1, 0, 0 },
            { 1, 0, 1, 0, 1 }
        };

        //Grid keeping track of where we've fired already
        private bool[,] hits;

        //Number of rows & columns on the board
        private int nRows;
        private int nCols;

        //Currently selected row and column
        private int row;
        private int col;

        //Current score & time
        private int score;
        private int time;

        [SerializeField] Transform gridRoot;
        [SerializeField] GameObject cellPrefab;
        [SerializeField] GameObject winLabel;
        [SerializeField] TextMeshProUGUI timeLabel;
        [SerializeField] TextMeshProUGUI scoreLabel;

        //Called when the program starts
        private void Awake()
        {
            nRows = grid.GetLength(0);
            nCols = grid.GetLength(1);
            hits = new bool[nRows, nCols];

            //Fills the grid with cells
            for (int i = 0; i < nRows * nCols; i++) {
                Instantiate(cellPrefab, gridRoot);
            }

            //Selects top-right cell and starts the timer
            SelectCurrentCell();
            InvokeRepeating(nameof(IncrementTime), 1f, 1f);
        }

        //Property that immediately gets the current cell based on the current row & column
        Transform CurrentCell => gridRoot.GetChild(row * nCols + col);

        //Makes the current cell's "cursor" image visible (when selected)
        void SelectCurrentCell()
        {
            CurrentCell.Find("Cursor").gameObject.SetActive(true);
        }

        //Makes the current cell's "cursor" image invisible (when unselected)
        void UnselectCurrentCell() {
            CurrentCell.Find("Cursor").gameObject.SetActive(false);
        }

        //Moves "cursor" to the left or right by integer amount
        public void MoveHorizontal(int amt)
        {
            UnselectCurrentCell();
            col += amt;
            col = Mathf.Clamp(col, 0, nCols - 1); //Constricts selection to bounds of grid
            SelectCurrentCell();
        }

        //Moves "cursor" to the left or right by integer amount
        public void MoveVertical(int amt)
        {
            UnselectCurrentCell();
            row += amt;
            row = Mathf.Clamp(row, 0, nRows - 1); //Constricts selection to bounds of grid
            SelectCurrentCell();
        }

        //Makes the current cell's "hit" image visible
        void ShowHit()
        {
            CurrentCell.Find("Hit").gameObject.SetActive(true);
        }

        //Makes the current cell's "miss" image visible
        void ShowMiss()
        {
            CurrentCell.Find("Miss").gameObject.SetActive(true);
        }

        //Increment the score and visible score counter's text
        void IncrementScore()
        {
            score++;
            scoreLabel.text = $"Score: {score}";
        }

        //Lets the player fire
        public void Fire()
        {
            if (hits[row, col]) return; //If we already shot our current cell, do nothing

            hits[row, col] = true; //Marks that we've shot it already

            //Show the appropriate image based on ship/no ship
            if (grid[row, col] == 1)
            {
                ShowHit();
                IncrementScore(); //Give a point
                TryEndGame(); //See if game is over
            }
            else ShowMiss();
        }

        //Checks to see if the game is over
        public void TryEndGame()
        {
            //Iterate through all the cells
            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    if (grid[row, col] == 1 && !hits[row, col]) return; //If any cells have ships but haven't been shot, don't end the game
                }
            }
            //Give a "You Win!" message and stop the timer
            winLabel.SetActive(true);
            CancelInvoke(nameof(IncrementTime));
        }

        //Increment the time and update the visible timer's text
        public void IncrementTime()
        {
            time++;
            timeLabel.text = $"{time / 60}:{time % 60:00}";
        }

        //Restarts the game
        public void Restart()
        {
            //Remove the cursor
            UnselectCurrentCell();
            //Iterate through all the cells
            for (int row = 0; row < nRows; row++)
            {
                for (int col = 0; col < nCols; col++)
                {
                    Transform cell = gridRoot.GetChild(row * nCols + col); //Find current cell
                    cell.Find("Hit").gameObject.SetActive(false); //Hide its "hit" image
                    cell.Find("Miss").gameObject.SetActive(false); //Hide its "miss" image
                    hits[row, col] = false; //Reset its "hit" to false
                    grid[row, col] = rand.Next(2); //Give it a random value
                }
            }
            //Return the cursor to the top right
            row = 0;
            col = 0;
            SelectCurrentCell();
            //Reset the score
            score = 0;
            scoreLabel.text = "Score: 0";
            //Reset the timer
            time = 0;
            timeLabel.text = "0:00";
            //Hide the win message
            winLabel.SetActive(false);
            //Start the timer again
            CancelInvoke(nameof(IncrementTime));
            InvokeRepeating(nameof(IncrementTime), 1f, 1f);
        }
    }
}