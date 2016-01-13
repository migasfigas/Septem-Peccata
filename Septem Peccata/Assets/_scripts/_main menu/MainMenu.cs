using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private GameObject loadBackground;
    private GameObject loadText, loadImage;
    private int loadProgress = 0;

    private GameObject menuScreen;
    [SerializeField] private GameObject optionsMenu;

	// Use this for initialization
	void Start () {

        menuScreen = GameObject.Find("menu screen").gameObject;
        if(optionsMenu == null)
            optionsMenu = GameObject.Find("options screen").gameObject;
	}
	
	// Update is called once per frame
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

    }

    public void settings()
    {
        menuScreen.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    }

    public void optionsQuit()
    {
        optionsMenu.SetActive(false);
        menuScreen.SetActive(true);
    }

    IEnumerator DisplayLoadingScreen(int level)
    {
        Debug.Log("Hello");

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
}
