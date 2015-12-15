using UnityEngine;
using System.Collections;

public class InteractiveObject : MonoBehaviour {

    public Main main;
    public GameObject interactText;
    public FirstPersonController player;
    bool colliding = false;

    public Main.CurrentQuest questObject = Main.CurrentQuest.none;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (main != null && interactText != null && player != null && interactText.activeSelf)
            GetInput();
	}

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && colliding)
        {
            if(!main.chatting && main.activeQuest == Main.CurrentQuest.lamp)
            {

                interactText.SetActive(false);
                main.selfQuest.Done = true;

                player.lamp.SetActive(true);
                player.candleLight.enabled = false;
                player.animator.SetTrigger("has lamp");

                DestroyObject(gameObject);
            }

            else if(main.activeQuest == Main.CurrentQuest.hallway)
            {
                interactText.SetActive(false);
                main.hallwayQuest.Done = true;

                DestroyObject(gameObject);
            }
        }
       
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            interactText.SetActive(true);
            colliding = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        interactText.SetActive(false);
        colliding = false;
    }
}
