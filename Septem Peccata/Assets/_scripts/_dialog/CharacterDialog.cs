using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterDialog : MonoBehaviour {

    public Main main;
    public Main.NPCs currentChar;
    public GameObject interactText;
    public GameObject dialogBox;

    Chitchat character;

    void Start() {

        character = new Chitchat(main, currentChar, dialogBox, interactText);
    }
    
    void Update() {
        character.Update();

        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        //character.OnTriggerEnter(col);
        if (main.ActiveQuest == Main.QuestType.none && !main.LampQuest.Done)
        {
            dialogBox.SetActive(true);
            main.Chatting = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        //character.OnTriggerExit(col);
        if(!interactText.activeSelf && !main.Chatting)
            Destroy(gameObject);
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
