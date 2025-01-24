using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using XRAccess.Chirp;
public class DialogueUIManager : MonoBehaviour
{
    public Transform optionParent;
    public GameObject optionObject;
    public GameObject panel;
    public static UnityEvent destroySelf = new UnityEvent();
    public CaptionSource captionSource;
    public float wordTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }
    public void CloseDisplay()
    {
        panel.SetActive(false);
    }
    public void UpdateDisplay(DialogueData dd, float audTime = -1)
    {
        if (!panel.activeInHierarchy)
        {
            panel.SetActive(true);
        }
        destroySelf.Invoke();
        if (audTime <= 0) {
            audTime = dd.line.Split(" ").Length * wordTime;
        }
        captionSource.ShowTimedCaption(DialogueTreeInterpreter.currentlyPlaying.chars[dd.charIDs[dd.charCurrentlySpeaking]].Name+": "+dd.line, audTime);
        if (dd.options.Length == 0)
        {
            GameObject g = Instantiate(optionObject, optionParent);
            OptionData d = new OptionData();
            d.id = -1;
            d.title = DialogueDataReference.inst.defaultResponse;
            g.GetComponent<DialogueOptionManager>().init(d);
        }
        else
        {
            foreach (OptionData d in dd.options)
            {
                GameObject g = Instantiate(optionObject, optionParent);
                g.GetComponent<DialogueOptionManager>().init(d);
            }
        }
    }
}
