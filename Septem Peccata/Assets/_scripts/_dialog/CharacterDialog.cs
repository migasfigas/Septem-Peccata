using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterDialog : MonoBehaviour {

    public Main main;
    public Main.NPCs currentChar;
    public GameObject interactText;
    public GameObject dialogBox;

    [Multiline] public string[] suggestionDialog;
    [Multiline] public string[] doneDialog;

    bool colliding = false;

    Chitchat character;

    void Start() {

        if(main == null) main = GameObject.Find("main").GetComponent<Main>();
        if (interactText == null) interactText = GameObject.Find("main/Canvas/interact text").gameObject;
        if(dialogBox == null) dialogBox = GameObject.Find("main/Canvas/dialog box").gameObject;

        character = new Chitchat(main, currentChar, dialogBox, interactText, suggestionDialog, doneDialog);
    }
    
    void Update() {

        if (colliding)
        {
            character.Update();

            if (character.chatDone)
            {
                if (currentChar == Main.NPCs.meMyselfAndI)
                    main.ActiveQuest = Main.QuestType.lamp;

                colliding = false;
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            dialogBox.SetActive(true);
            main.Chatting = true;
            colliding = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (!interactText.activeSelf && !main.Chatting)
        {
            Destroy(gameObject);
        }
    }

    public void buttonAccept()
    {
        character.buttonAccept();
    }

    public void buttonCancel()
    {
        character.buttonCancel();
    }
}
