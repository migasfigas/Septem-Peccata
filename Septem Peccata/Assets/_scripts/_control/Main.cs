using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public enum CurrentQuest
    {
        none,
        first,
        second
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
    private Text temptationUI;

    public Quest selfQuest;
    public Quest currentQuest;

    public bool hadQuest = false;
    public bool pause = false;
    
    void Start () {

        activeQuest = CurrentQuest.none;
        temptation = 0;

        HUD = Canvas.transform.FindChild("HUD").gameObject;
        pauseUI = Canvas.transform.FindChild("pause").gameObject;

        questUI = HUD.transform.FindChild("quest").gameObject;
        temptationUI = HUD.transform.FindChild("temptation").GetComponent<Text>();

        selfQuest = new Quest(this, CurrentQuest.first, questUI);
	}
	
	void Update () {
        setUI();
        getInput();

        if (temptation >= 100)
            PriestDie();
    }

    private void setUI()
    {
        temptationUI.text = temptation.ToString();

        switch(activeQuest)
        {
            case CurrentQuest.first:
                currentQuest = selfQuest;
                hadQuest = true;
                selfQuest.setGUI();
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
