using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Falling2UrImpendingDoom : MonoBehaviour {

    Main main;

	// Use this for initialization
	void Start () {

        main = GameObject.Find("main").GetComponent<Main>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(main.CurrentLevel);
            Main[] mains = GameObject.FindObjectsOfType<Main>();

            foreach (Main m in mains)
                Destroy(m.gameObject);
        }
    }
}
