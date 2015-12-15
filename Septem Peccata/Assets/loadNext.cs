using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadNext : MonoBehaviour {

    public GameObject interactText;
    bool colliding = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(colliding && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("_statue level");
        }
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
}
