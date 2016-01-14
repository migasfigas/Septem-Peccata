using UnityEngine;
using System.Collections;

public class InteractiveObject : MonoBehaviour {

    [SerializeField] private Main main;
    [SerializeField] private GameObject interactText;
    [SerializeField] private FirstPersonController player;
    [SerializeField] private Main.QuestType questObject = Main.QuestType.none;

    private bool colliding = false;
    
	void Start () {

        if (main == null || player == null || interactText == null)
        {
            main = GameObject.Find("main").GetComponent<Main>();
            player = GameObject.Find("player").GetComponent<FirstPersonController>();
            interactText = GameObject.Find("main/Canvas/interact text").gameObject;
        }
	}
	
	void Update () {

        if (main != null && interactText != null && player != null && interactText.activeSelf)
            GetInput();

        if(main != null && main.HallwayQuest != null && main.HallwayQuest.Done)
        {
            transform.position= Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, -36f), Time.deltaTime);
        }
	}

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.E) && colliding)
        {
            switch(gameObject.tag)
            {
                case "lamp":
                    if(!main.Chatting && main.ActiveQuest == Main.QuestType.lamp)
                    {
                        interactText.SetActive(false);
                        main.LampQuest.Done = true;

                        player.lamp.SetActive(true);
                        player.candleLight.enabled = false;
                        player.animator.SetTrigger("has lamp");

                        DestroyObject(gameObject);
                    }
                    break;

                case "statue":
                    if(main.ActiveQuest == Main.QuestType.hallway)
                    {
                        interactText.SetActive(false);
                        main.HallwayQuest.Done = true;
                    }
                    break;

                case "healing statue":
                    if (main.Temptation - 30 > 0)
                        main.Temptation -= 30;
                    else
                        main.Temptation = 0;
                    break;

                default:
                    break;
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
