using UnityEngine;

public class DialogueDataReference : MonoBehaviour
{
    public static DialogueDataReference inst;
    public string defaultResponse;
    private void Awake()
    {
        inst = this;
    }
}
