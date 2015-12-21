using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Quest {

    Main main;
    Main.QuestType quest;
    GameObject questUI;

    //Primeira quest: apanhar a lanterna
    public GameObject lantern;
    private bool done;

    public Quest(Main main, Main.QuestType quest, GameObject questUI)
    {
        this.main = main;
        this.quest = quest;
        this.questUI = questUI;

        done = false;
    }
    
	void Start () {
	
	}
	
	void Update () {
	
	}

    public void setGUI()
    {
        if (quest != Main.QuestType.none)
        {
            switch (quest)
            {
                case Main.QuestType.lamp:
                    questUI.SetActive(true);
                    if (!done) questUI.GetComponent<Text>().text = "Find a lantern.";
                    else
                    {
                        questUI.GetComponent<Text>().CrossFadeAlpha(1, 0, false);
                        questUI.GetComponent<Text>().text = "Press L to turn it on.";
                    }
                    break;

                case Main.QuestType.hallway:
                    break;

                default:
                    break;
            }

            questUI.GetComponent<Text>().CrossFadeAlpha(0, 2, false);

            if (questUI.GetComponent<Text>().color.a <= 0)
                questUI.SetActive(false);
        }

        if(done)
        {
            main.ActiveQuest = Main.QuestType.none;
        }
    }

    public bool Done
    {
        get { return done; }
        set { done = value; }
    }
}
