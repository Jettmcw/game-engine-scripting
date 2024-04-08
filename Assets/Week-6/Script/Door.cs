using UnityEngine;

public class Door : MonoBehaviour
{
    public bool opened;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Vector3 hingePosition;

    private void Awake()
    {
        MazeGameManager.resetGame.AddListener(Restart); //Door should close when the game restarts

        transform.GetPositionAndRotation(out startPosition, out startRotation); //Find the starting position of the door
        hingePosition = transform.Find("Hinge").position; //Find the position of the door's hinge
    }
    
    //Opens the door by rotating it around its hinge
    public void Open()
    {
        opened = true;
        transform.RotateAround(hingePosition, Vector3.down, 90);
    }

    //Closes the door
    public void Restart()
    {
        opened = false;
        transform.SetPositionAndRotation(startPosition, startRotation);
    }
}
