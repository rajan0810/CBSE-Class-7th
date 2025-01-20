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
        // Update puzzle pieces based on child objects' states
        UpdatePuzzlePieceStatus();

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

    // Function to update the puzzle piece status based on child objects
    private void UpdatePuzzlePieceStatus()
    {
        // Assuming child objects have a script called "ChildPuzzlePiece"
        // Loop through all child objects
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i); // Get the child object
            XRSocketInteractorTagCompare childScript = child.GetComponent<XRSocketInteractorTagCompare>(); // Get the child script

            if (childScript != null) // Check if the child has the script
            {
                // Update the puzzle piece booleans based on the child's script
                switch (i)
                {
                    case 0: piece1 = childScript.isComplete; break; // For the first child
                    case 1: piece2 = childScript.isComplete; break; // For the second child
                    case 2: piece3 = childScript.isComplete; break; // For the third child
                    case 3: piece4 = childScript.isComplete; break; // For the fourth child
                    case 4: piece5 = childScript.isComplete; break; // For the fifth child
                    default: Debug.LogWarning("More children than puzzle pieces!"); break;
                }
            }
        }
    }
}
