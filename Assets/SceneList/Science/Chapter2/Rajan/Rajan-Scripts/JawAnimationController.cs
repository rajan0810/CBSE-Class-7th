using UnityEngine;

public class JawAnimationController : MonoBehaviour
{
    public Animator Jaw;
    public AudioSource audio;
    public bool On = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Jaw = GetComponent<Animator>();

        if (Jaw == null)
        {
            Debug.LogError("Jaw Animator not found " + gameObject.name);
        }
    }

    public void PlayJaw()
    {
        if (On)
        {
            if (Jaw != null)
            {
                Jaw.SetTrigger("CloseJaw");
            }
            else
            {
                Debug.LogError("Jaw Animator not found " + gameObject.name);
            }
        }
        else
        {
            if (Jaw != null)
            {
                Jaw.SetTrigger("OpenJaw");
                audio.Play();
            }
            else
            {
                Debug.LogError("Jaw Animator not found " + gameObject.name);
            }
        }
        On = !On;
    }
}
