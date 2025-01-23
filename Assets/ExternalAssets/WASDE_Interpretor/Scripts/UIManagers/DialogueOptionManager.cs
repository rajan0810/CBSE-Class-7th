using UnityEngine;
using TMPro;
public class DialogueOptionManager : MonoBehaviour
{

    public int optionNumber;
    public TMP_Text optionText;

    private void Start()
    {
        DialogueUIManager.destroySelf.AddListener(die);
    }
    public void UpdateText(string s) {
        if (s == null || s == "") {
            s = DialogueDataReference.inst.defaultResponse;
        }
        optionText.text = s;
    }
    public void init(OptionData od) {
        optionNumber = od.id;
        UpdateText(od.title);
    }

    public void onPress() {
            DialogueTreeInterpreter.moveTo(optionNumber);
    }
    public void die() {
        DialogueUIManager.destroySelf.RemoveListener(die);
        Destroy(gameObject);
    }

}
