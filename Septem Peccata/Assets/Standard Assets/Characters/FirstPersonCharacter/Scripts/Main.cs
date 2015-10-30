using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public enum Quest
    {
        none,
        first,
        second
    };

    public enum NPCs
    {
        none,
        oldMan
    };
    
    public Quest activeQuest;
    public bool chatting;
    public int sanity;

    //GUI
    public GameObject HUD;
    private GameObject questUI;
    private Text sanityUI;

    // Use this for initialization
    void Start () {

        activeQuest = Quest.none;
        sanity = 100;

        questUI = HUD.transform.FindChild("quest").gameObject;
        sanityUI = HUD.transform.FindChild("sanity").GetComponent<Text>();
	}
	
	void Update () {
        setUI();
    }

    private void setUI()
    {
        sanityUI.text = sanity.ToString();

        if(activeQuest != Quest.none)
        {
            switch(activeQuest)
            {
                case Quest.first:
                    questUI.SetActive(true);
                    questUI.GetComponent<Text>().text = "Get creepy capsule some friends.";
                    break;
            }
        }
    }
}
