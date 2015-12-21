using UnityEngine;
using System.Collections;

public class InteractiveObject : MonoBehaviour {

    public Main main;
    public GameObject interactText;
    public FirstPersonController player;
    bool colliding = false;

    public Main.QuestType questObject = Main.QuestType.none;

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
            if(!main.Chatting && main.ActiveQuest == Main.QuestType.lamp)
            {

                interactText.SetActive(false);
                main.LampQuest.Done = true;

                player.lamp.SetActive(true);
                player.candleLight.enabled = false;
                player.animator.SetTrigger("has lamp");

                DestroyObject(gameObject);
            }

            else if(main.ActiveQuest == Main.QuestType.hallway)
            {
                interactText.SetActive(false);
                main.HallwayQuest.Done = true;

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
