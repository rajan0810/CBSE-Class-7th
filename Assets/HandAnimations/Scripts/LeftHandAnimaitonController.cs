using UnityEngine;
using UnityEngine.InputSystem;

public class LeftHandAnimaitonController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private InputActionProperty trigger;
    [SerializeField] private InputActionProperty grab;
    [SerializeField] private Animator animator;
    private float tirgger, grip; 
    
    
    void Start()
    { 
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        tirgger = trigger.action.ReadValue<float>(); 
        grip = grab.action.ReadValue<float>();
        animator.SetFloat("Trigger",tirgger);
        animator.SetFloat("Grip", grip);
        //Now what we need to make is the bet way to sort between the two point 
    }
}
