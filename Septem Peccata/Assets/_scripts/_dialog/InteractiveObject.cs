using UnityEngine;
using System.Collections;

public class InteractiveObject : MonoBehaviour {

    public Main main;
    public GameObject interactText;
    public FirstPersonController player;

    public Main.CurrentQuest questObject = Main.CurrentQuest.none;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (interactText.activeSelf)
            GetInput();
	}

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && !main.chatting && main.activeQuest == Main.CurrentQuest.first)
        {
            interactText.SetActive(false);
            main.selfQuest.Done = true;

            player.lamp.SetActive(true);
            player.candleLight.enabled = false;
            player.animator.SetTrigger("has lamp");

            DestroyObject(gameObject);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            interactText.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        interactText.SetActive(false);
    }
}
