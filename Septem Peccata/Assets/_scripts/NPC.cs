using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour {

    public Main main;
    public Main.NPCs currentChar;
    public GameObject interactText;
    public GameObject dialogBox;

    Chitchat character;

    void Start() {

        character = new Chitchat(main, currentChar, dialogBox, interactText);
        character.Start();
    }
    
    void Update() {
        character.Update();
    }

    private void OnTriggerEnter(Collider col)
    {
        character.OnTriggerEnter(col);
    }

    private void OnTriggerExit(Collider col)
    {
        character.OnTriggerExit(col);
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
