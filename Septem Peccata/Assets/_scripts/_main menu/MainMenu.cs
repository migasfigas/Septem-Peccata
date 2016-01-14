using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour {

    private GameObject loadBackground;
    private GameObject loadText, loadImage;
    private GameObject loadGameButton;
    private int loadProgress = 0;

    private GameObject menuScreen;
    [SerializeField] private GameObject optionsMenu;

    public int resolution = 0, qualitySettings = 0;
    public float music = 1f, sound = 1f;

    string saveGameFile = "savegame.txt";

	// Use this for initialization
	void Start () {

        Cursor.visible = true;

        menuScreen = GameObject.Find("menu screen").gameObject;
        loadGameButton = GameObject.Find("load game").gameObject;

        if(optionsMenu == null)
            optionsMenu = GameObject.Find("options screen").gameObject;

        optionsMenu.GetComponent<CanvasGroup>().alpha = 0;
        foreach (Transform t in optionsMenu.transform)
            t.gameObject.SetActive(false);

        readSettings("settings.txt");

        if (!File.Exists(saveGameFile))
            loadGameButton.GetComponent<Button>().interactable = false;
	}
	
	void Update () {
	
	}

    public void newGame()
    {
        loadBackground = GameObject.Find("Canvas/loading screen/background").gameObject;
        loadText = GameObject.Find("Canvas/loading screen/loading text").gameObject;
        loadImage = GameObject.Find("Canvas/loading screen/loading image").gameObject;

        StartCoroutine(DisplayLoadingScreen(1));
    }

    public void loadGame()
    {
        loadBackground = GameObject.Find("Canvas/loading screen/background").gameObject;
        loadText = GameObject.Find("Canvas/loading screen/loading text").gameObject;
        loadImage = GameObject.Find("Canvas/loading screen/loading image").gameObject;

        if (File.Exists(saveGameFile))
        {
            var sr = File.OpenText(saveGameFile);

            int lvl = int.Parse(sr.ReadLine());


            StartCoroutine(DisplayLoadingScreen(lvl));

            sr.Close();
        }
    }

    public void settings()
    {
        StartCoroutine(Fade(menuScreen, -0.01f));
        StartCoroutine(Fade(optionsMenu, +0.01f));
    }

    public void exit()
    {
        Application.Quit();
    }

    public void optionsQuit()
    {
        StartCoroutine(Fade(optionsMenu, -0.01f));
        StartCoroutine(Fade(menuScreen, +0.01f));

        saveSettings("settings.txt");
    }

    IEnumerator Fade(GameObject group, float incrementation)
    {
        bool fade = true;

        foreach (Transform t in group.transform)
        {
            if (incrementation > 0)
                t.gameObject.SetActive(true);
            else
                t.gameObject.SetActive(false);
        }

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

    IEnumerator DisplayLoadingScreen(int level)
    {
        loadBackground.SetActive(true);
        loadText.SetActive(true);
        loadImage.SetActive(true);

        loadText.GetComponent<Text>().text = loadProgress + "%";

        AsyncOperation async = SceneManager.LoadSceneAsync(level);

        while (!async.isDone)
        {
            loadProgress = (int)(async.progress * 100);
            loadText.GetComponent<Text>().text = loadProgress + "%";

            yield return null;
        }

        loadBackground.SetActive(false);
        loadText.SetActive(false);
        loadImage.SetActive(false);
        loadProgress = 0;
    }

    private void saveSettings(string file)
    {
        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(music);
        sw.WriteLine(sound);
        sw.WriteLine(resolution);
        sw.WriteLine(qualitySettings);

        sw.Close();
    }

    private void readSettings(string file)
    {
        if (File.Exists(file))
        {
            var sr = File.OpenText(file);
            music = float.Parse(sr.ReadLine());
            sound = float.Parse(sr.ReadLine());
            resolution = int.Parse(sr.ReadLine());
            qualitySettings = int.Parse(sr.ReadLine());

            sr.Close();
        }
    }
}
