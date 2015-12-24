using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private GameObject loadingBackground;
    private GameObject loadingText;
    private int loadProgress = 0;

	// Use this for initialization
	void Start () {

        loadingBackground = GameObject.Find("Canvas/loading screen/background").gameObject;
        loadingText = GameObject.Find("Canvas/loading screen/loading text").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void newGame()
    {
        StartCoroutine(DisplayLoadingScreen(1));
    }

    public void loadGame()
    {

    }

    public void settings()
    {
        
    }

    public void exit()
    {
        Application.Quit();
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
    }
}
