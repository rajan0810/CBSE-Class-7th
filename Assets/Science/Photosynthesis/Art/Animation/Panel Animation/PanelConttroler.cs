using UnityEngine;

public class PanelConttroler : MonoBehaviour
{
    bool closed = true;
    public void openclose(){
        if(closed){
            TriggerAnimator("open");
        }
        else{
            TriggerAnimator("close");
        }

    }
    void Start() {
        TriggerAnimator("open");
        
    }

    /// <summary>
    /// Sets a trigger on all Animator components in the children of this GameObject.
    /// </summary>
    /// <param name="triggerName">The name of the trigger to activate.</param>
    private void TriggerAnimator(string triggerName)
    {
        // Get all Animator components in the children of this GameObject
        Animator[] childAnimators = GetComponentsInChildren<Animator>();

        Debug.Log(childAnimators.Length);

        foreach (Animator animator in childAnimators)
        {
            if (animator != null)
            {
                animator.SetTrigger(triggerName);
            }
            else
            {
                Debug.LogWarning($"Animator component missing on {animator.gameObject.name}");
            }
        }
        closed =!closed;
    }

    
}
