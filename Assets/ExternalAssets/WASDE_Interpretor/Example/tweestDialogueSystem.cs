using UnityEngine;
using System.IO;
public class tweestDialogueSystem : MonoBehaviour
{
    public TextAsset testdialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueTreeInterpreter.StartDialogue(testdialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
