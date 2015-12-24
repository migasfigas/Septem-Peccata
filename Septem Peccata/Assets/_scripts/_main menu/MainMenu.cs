using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private GameObject loadBackground;
    private GameObject loadText;
    private int loadProgress = 0;

	// Use this for initialization
	void Start () {

       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void newGame()
    {
        loadBackground = GameObject.Find("Canvas/loading screen/background").gameObject;
        loadText = GameObject.Find("Canvas/loading screen/loading text").gameObject;

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
        Debug.Log("Hello");

        loadBackground.SetActive(true);
        loadText.SetActive(true);

        loadText.GetComponent<Text>().text = "Loading " + loadProgress + "%";

        AsyncOperation async = SceneManager.LoadSceneAsync(level);

        while (!async.isDone)
        {
            loadProgress = (int)(async.progress * 100);
            loadText.GetComponent<Text>().text = "Loading " + loadProgress + "%";

            yield return null;
        }

        loadBackground.SetActive(false);
        loadText.SetActive(false);
        loadProgress = 0;
    }
}
