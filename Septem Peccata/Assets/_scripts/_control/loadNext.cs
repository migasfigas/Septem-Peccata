using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadNext : MonoBehaviour {

    [SerializeField] private GameObject interactText;
    [SerializeField] private int scene;

    private GameObject loadBackground;
    private GameObject loadText, loadImage;
    private int loadProgress = 0;

    private bool colliding = false;

    // Use this for initialization
    void Start () {

        interactText = GameObject.Find("main/Canvas/interact text").gameObject;

        loadBackground = GameObject.Find("Canvas/loading screen/background").gameObject;
        loadText = GameObject.Find("Canvas/loading screen/loading text").gameObject;
        loadImage = GameObject.Find("Canvas/loading screen/loading image").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
	
        if(colliding && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(DisplayLoadingScreen(scene));
        }
	}

    IEnumerator DisplayLoadingScreen(int level)
    {
        interactText.SetActive(false);
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

    public void OnTriggerEnter(Collider col)
    {
        colliding = true;
        interactText.SetActive(true);
    }

    public void OnTriggerExit(Collider col)
    {
        colliding = false;
        interactText.SetActive(false);
    }

    public int Scene
    {
        get { return scene; }
        set { scene = value; }
    }
}
