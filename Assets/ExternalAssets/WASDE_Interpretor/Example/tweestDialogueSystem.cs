using UnityEngine;
using System.IO;
using System.Collections;
public class tweestDialogueSystem : MonoBehaviour
{
    public TextAsset testdialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(startDealy());
    }

    IEnumerator startDealy(){
        yield return new WaitForEndOfFrame();
        DialogueTreeInterpreter.StartDialogue(testdialogue);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
