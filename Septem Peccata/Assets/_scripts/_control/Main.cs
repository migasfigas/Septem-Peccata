using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public enum CurrentQuest
    {
        none,
        lamp,
        hallway
    };

    public enum NPCs
    {
        none,
        meMyselfAndI,
        oldMan
    };
    
    public CurrentQuest activeQuest;
    public bool chatting;
    public int temptation;

    //GUI
    public GameObject Canvas;
    private GameObject HUD, pauseUI;

    private GameObject questUI;
    private RectTransform temptationUI;

    public Quest selfQuest;
    public Quest hallwayQuest;
    public Quest currentQuest;

    public bool hadQuest = false;
    public bool pause = false;

    public bool questdone = false;

    public bool playerAttacking = false;

    float sizeX = 0;

    //2 7 4 3 6 0 1
    public int lastDoor = 0; //cada vez que incrementa modifica a estatua
    public int[] doorSequence = { 2, 7, 4, 3, 6, 0 };
    public GameObject statue;
    
    void Start () {

        activeQuest = CurrentQuest.none;
        temptation = 0;

        HUD = Canvas.transform.FindChild("HUD").gameObject;
        pauseUI = Canvas.transform.FindChild("pause").gameObject;

        questUI = HUD.transform.FindChild("quest").gameObject;
        temptationUI = HUD.transform.FindChild("temptation").GetComponent<Image>().transform.FindChild("bar").GetComponent<RectTransform>();

        selfQuest = new Quest(this, CurrentQuest.lamp, questUI);
        hallwayQuest = new Quest(this, CurrentQuest.hallway, questUI);

        sizeX = temptationUI.sizeDelta.x;

        //debug
        selfQuest.Done = questdone;
    }
	
	void Update () {
        setUI();
        getInput();

        if (temptation >= 100)
            PriestDie();

        if(Input.GetKey(KeyCode.X))
        {
            temptation++;
        }
    }

    private void setUI()
    {
        temptationUI.sizeDelta = new Vector2(sizeX * temptation/100, temptationUI.sizeDelta.y);

        switch(activeQuest)
        {
            case CurrentQuest.lamp:
                currentQuest = selfQuest;
                hadQuest = true;
                selfQuest.setGUI();
                break;

            case CurrentQuest.hallway:
                currentQuest = hallwayQuest;
                break;

            default:
                break;
        }
    }

    private void getInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;

            if (pause) onPauseGame();
            else onResumeGame();
        }
    }

    private void onPauseGame()
    {
        Time.timeScale = 0;
        HUD.SetActive(false);
        pauseUI.SetActive(true);
    }

    private void onResumeGame()
    {
        Time.timeScale = 1;
        HUD.SetActive(true);
        pauseUI.SetActive(false);
    }

    private void PriestDie()
    {
        pauseUI.transform.FindChild("text").GetComponent<Text>().text = "is dead";
        pause = true;
        onPauseGame();
    }
}
