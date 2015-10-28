﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCInteracting: MonoBehaviour {

    public Main main;

    bool colliding = false;
    public GameObject screenText;
    public GameObject dialogBox;

    public Text dialogText;
    public Image dialogImage;

    private Button cancelButton, acceptButton;
    private GameObject buttons;

    int clicks = 0;
    int maxClicks;

    public string[] dialog = { "hello there", "friend", "i dont know what to say", "no one gave me a script", "i'm just a lonely capsule in the middle of nowhere", "go find me some friends please?" };

	// Use this for initialization
	void Start () {

        dialogText = dialogBox.transform.FindChild("dialog").GetComponent<Text>();
        dialogImage = dialogBox.transform.FindChild("text box").GetComponent<Image>();
        buttons = dialogBox.transform.FindChild("buttons").gameObject;
        

	}
	
	// Update is called once per frame
	void Update () {
    
        if(screenText.activeSelf || dialogBox.activeSelf)
            GetInput();
        
	}

    public void buttonAccept()
    {
        dialogBox.SetActive(false);
        buttons.SetActive(false);
        main.chatting = false;

        main.activeQuest = Main.Quest.first;

        clicks = 0;      
    }

    public void buttonCancel()
    {
        dialogBox.SetActive(false);
        buttons.SetActive(false);
        main.chatting = false;

        clicks = 0;
    }
        
    private void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            screenText.SetActive(false);
            dialogBox.SetActive(true);
            main.chatting = true;
        }

        if(dialogBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {                
                clicks++;
                setText(clicks);
            }

            else if (clicks == 0)
                setText(0);
        }
    }

    private void setText(int click)
    {
        if (click >= 0 && click < dialog.Length)
        {
            dialogText.text = dialog[click];
        }

        if (click == dialog.Length - 1) 
        {
            buttons.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player") && !dialogBox.activeSelf)
        {
            screenText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        screenText.SetActive(false);
    }
}
