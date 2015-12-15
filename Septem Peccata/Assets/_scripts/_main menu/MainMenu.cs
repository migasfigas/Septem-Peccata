using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void newGame()
    {
        SceneManager.LoadScene("_scene_01");

    }

    public void loadGame()
    {

    }

    public void settings()
    {
        Application.Quit();
    }

    public void exit()
    {

    }
}
