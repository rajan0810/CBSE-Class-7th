using UnityEditor;
using UnityEngine;

public class Activity1 : MonoBehaviour
{

     // Puzzle piece status
    public bool piece1 = false;
    public bool piece2 = false;
    public bool piece3 = false;
    public bool piece4 = false;
    public bool piece5 = false;

    // A flag to ensure the event only triggers once
    private bool isPuzzleCompleted = false;

    // Update is called once per frame
    void Update()
    {
        // Check if all puzzle pieces are true and the puzzle is not already completed
        if (!isPuzzleCompleted && AllPuzzlePiecesCompleted())
        {
            isPuzzleCompleted = true; // Mark as completed
            OnPuzzleCompleted(); // Trigger the event
        }
    }

    // Function to check if all puzzle pieces are completed
    private bool AllPuzzlePiecesCompleted()
    {
        return piece1 && piece2 && piece3 && piece4 && piece5;
    }

    // Event to trigger when the puzzle is completed
    private void OnPuzzleCompleted()
    {
        Debug.Log("Puzzle completed! Triggering the next activity...");
    }
}
