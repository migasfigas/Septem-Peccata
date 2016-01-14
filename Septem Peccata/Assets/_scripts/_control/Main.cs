using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {

    #region GUI
    [SerializeField] private GameObject canvas;
    private GameObject HUD, pauseUI;
    private GameObject questUI;
    private RectTransform temptationUI;

    private GameObject loadingBackground;
    private GameObject loadingText;
    private GameObject loadingImage;
    private int loadProgress = 0;

    private float sizeX = 0;
    #endregion

    #region Quests
    public enum QuestType
    {
        none,
        lamp,
        hallway,
        platforms
    };

    public enum NPCs
    {
        none,
        meMyselfAndI,
        oldMan
    };
    
    [SerializeField] private QuestType activeQuest;
    private Quest lampQuest, hallwayQuest, platformQuest;

    [SerializeField] private bool chatting;
    #endregion

    #region Player stats
    [SerializeField] private int temptation;   
    public bool pause = false;
    public bool playerAttacking = false;
    #endregion

    [SerializeField] private int currentLevel;

    void Awake()
    {
        //o main nunca é destruido, passa de cena para cena
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        
        canvas = transform.FindChild("Canvas").gameObject;

        HUD = canvas.transform.FindChild("HUD").gameObject;
        pauseUI = canvas.transform.FindChild("pause").gameObject;

        questUI = HUD.transform.FindChild("quest").gameObject;
        temptationUI = HUD.transform.FindChild("temptation").GetComponent<Image>().transform.FindChild("bar").GetComponent<RectTransform>();

        loadingBackground = canvas.transform.Find("loading screen/background").gameObject;
        loadingText = canvas.transform.Find("loading screen/loading text").gameObject;
        loadingImage = canvas.transform.Find("loading screen/loading image").gameObject;

        canvas.transform.FindChild("interact text").gameObject.SetActive(false);

        lampQuest = new Quest(this, QuestType.lamp, questUI);

        sizeX = temptationUI.sizeDelta.x;
        activeQuest = QuestType.none;
        pause = false;
        playerAttacking = false;

        Cursor.visible = false;

        if (currentLevel == 2) {
            lampQuest.Done = true;
            hallwayQuest = new Quest(this, QuestType.hallway, questUI);
        }
        else if(currentLevel == 3)
        {
            lampQuest.Done = true;
            platformQuest = new Quest(this, QuestType.platforms, questUI);
        }
    }

    //é chamado quando um novo nivel é carregado (!= start)
    void OnLevelWasLoaded(int level)
    {
        if (loadingBackground!=null)
        {
            loadingBackground.SetActive(false);
            loadingText.SetActive(false);
            loadingImage.SetActive(false);
        }

        currentLevel = level;
        
        chatting = false;

        switch (level)
        {
            case 0:
                loadingBackground.SetActive(false);
                loadingText.SetActive(false);
                DestroyObject(gameObject);
                break;

            case 1:
                Start();
                break;

            case 2:
                lampQuest.Done = true;
                hallwayQuest = new Quest(this, QuestType.hallway, questUI);
                gameObject.AddComponent<StatuePuzzle>();
                break;

            case 3:
                DestroyObject(gameObject.GetComponent<StatuePuzzle>());
                hallwayQuest = new Quest(this, QuestType.hallway, questUI);
                break;

            default:
                break;
        }
    }

    void Update () {
        setUI();
        getInput();

        if (temptation >= 100)
            PriestDie();

        if(Input.GetKey(KeyCode.X))
            temptation++;
    }

    private void setUI()
    {
        temptationUI.sizeDelta = new Vector2(sizeX * temptation/100, temptationUI.sizeDelta.y);

        switch(activeQuest)
        {
            case QuestType.lamp:
                lampQuest.setGUI();
                break;

            case QuestType.hallway:
                break;

            default:
                break;
        }
    }

    //for fast debugging
    private void getInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;

            if (pause) onPauseGame();
            else onResumeGame();
        }

        if(Input.GetKeyDown(KeyCode.Home))
        {
            StartCoroutine(DisplayLoadingScreen(currentLevel));
            DestroyObject(gameObject);
        }

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            StartCoroutine(DisplayLoadingScreen(0));
        }

        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            StartCoroutine(DisplayLoadingScreen(currentLevel+1));
        }

        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            StartCoroutine(DisplayLoadingScreen(currentLevel - 1));
            DestroyObject(gameObject);
        }
    }
    
    IEnumerator DisplayLoadingScreen(int level)
    {
        loadingBackground.SetActive(true);
        loadingText.SetActive(true);

        loadingText.GetComponent<Text>().text = "Loading " + loadProgress + "%";

        AsyncOperation async = SceneManager.LoadSceneAsync(level);

        while (!async.isDone)
        {
            loadProgress = (int)(async.progress * 100);
            loadingText.GetComponent<Text>().text = "Loading " + loadProgress + "%";

            yield return null;
        }

        loadingBackground.SetActive(false);
        loadingText.SetActive(false);
        loadProgress = 0;

        currentLevel = level;
    }

    private void onPauseGame()
    {
        Time.timeScale = 0;
        StartCoroutine(Fade(HUD, -0.05f));
        StartCoroutine(Fade(pauseUI, +0.05f));
        Cursor.visible = true;
    }

    public void onResumeGame()
    {
        Time.timeScale = 1;
        pause = false;
        StartCoroutine(Fade(pauseUI, -0.05f));
        StartCoroutine(Fade(HUD, +0.05f));
        Cursor.visible = false;
    }

    private void PriestDie()
    {
        pauseUI.transform.FindChild("text").GetComponent<Text>().text = "is dead";
        pause = true;
        onPauseGame();
    }

    IEnumerator Fade(GameObject group, float incrementation)
    {
        bool fade = true;

        while (fade)
        {
            group.GetComponent<CanvasGroup>().alpha += incrementation;

            if (group.GetComponent<CanvasGroup>().alpha <= 0 || group.GetComponent<CanvasGroup>().alpha >= 1)
            {
                fade = false;
            }

            yield return null;
        }
    }


    #region Getters & Setters

    public int Temptation
    {
        get { return temptation; }
        set { temptation = value; }
    }

    public GameObject Canvas
    {
        get { return canvas; }
    }

    public bool Chatting
    {
        get { return chatting; }
        set { chatting = value; }
    }    

    public QuestType ActiveQuest
    {
        get { return activeQuest; }
        set { activeQuest = value; }
    }

    public Quest LampQuest { get { return lampQuest; } }
    public Quest HallwayQuest { get { return hallwayQuest; } }

    public int CurrentLevel { get { return currentLevel; } }
    #endregion
}
