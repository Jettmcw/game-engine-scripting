using UnityEngine;
using UnityEngine.UI;

public class Connect4 : MonoBehaviour
{
    [SerializeField] GameObject columnPrefab;
    [SerializeField] GameObject piecePrefab;

    [SerializeField] Color[] playerColors;

    private int player = 1;
    [SerializeField] private int[,] grid = new int[7, 6];

    void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            Instantiate(columnPrefab, transform);
        }

        DropPiece(1);
        DropPiece(1);
        DropPiece(1);
    }

    public void DropPiece(int col)
    {
        int row = transform.GetChild(col).childCount;

        if (row == 6) return;

        piecePrefab.GetComponent<Image>().color = playerColors[player - 1];
        Instantiate(piecePrefab, transform.GetChild(col));
        grid[col, row] = player;
    }
}
