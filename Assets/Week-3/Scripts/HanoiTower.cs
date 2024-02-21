using UnityEngine;
using static UnityEngine.Mathf;
using static System.Array;
using static UnityEngine.GameObject;

public class HanoiTower : MonoBehaviour
{
    [SerializeField] private GameObject WinText; //Text displayed when player wins
    [SerializeField] private GameObject[] Cursors; //All the cursor GameObjects

    //One two-dimensional array to keep track of all three pegs
    private readonly int[][] pegs = { new int[] { 4, 3, 2, 1 }, new int[4], new int[4] };

    //Keeps track of what peg the user has selected
    private int currentPeg;

    //Used by - and + buttons to move cursor & selection
    public void MoveCursor(int steps)
    {
        Cursors[currentPeg].SetActive(false);
        currentPeg = Clamp(currentPeg + steps, 0, 2); //Changes currentPeg & makes sure it's within bounds
        Cursors[currentPeg].SetActive(true);
    }

    //Used by < and > buttons to move discs
    public void MoveDisc(int steps)
    {
        int newPeg = currentPeg + steps;
        if (newPeg < 0 || newPeg > 2) return; //Check to see if moving disc within bounds

        int[] fromArray = pegs[currentPeg];
        int fromIndex = GetTopNumberIndex(fromArray);

        int[] toArray = pegs[newPeg];
        int toIndex = GetIndexOfFreeSlot(toArray);

        if (fromIndex == -1 || !CanAddToPeg(fromArray[fromIndex], toArray)) return; //Checks if there's a disc to move, and if there's a valid space

        toArray[toIndex] = fromArray[fromIndex]; //Put the disc size in its destination
        fromArray[fromIndex] = 0; //Remove the disc from its original position

        Find($"DiscSize{toArray[toIndex]}").transform.SetParent(Find($"Peg{newPeg + 1}").transform); //Moves the display discs

        if (pegs[1][3] == 1) WinText.SetActive(true); //If the second peg is filled to the top, show the win text
    }

    //Determines whether or not a disc of a certain size is allowed on a certain peg
    bool CanAddToPeg(int value, int[] peg) => GetIndexOfFreeSlot(peg) == 0 || peg[GetTopNumberIndex(peg)] > value;
    
    //Returns the index of the top disc on the peg
    int GetTopNumberIndex(int[] peg) => FindLastIndex(peg, i => i != 0);

    //Returns the first open (0) index of the peg
    int GetIndexOfFreeSlot(int[] peg) => FindIndex(peg, i => i == 0);
}
