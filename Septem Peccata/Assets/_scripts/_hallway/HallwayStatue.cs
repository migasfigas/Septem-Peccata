using UnityEngine;
using System.Collections;

public class HallwayStatue : MonoBehaviour {

    [SerializeField] private Main main;

    public StatuePuzzle puzzle;
    InteractiveObject final;
    AudioSource audioSource;

    public GameObject [] statues = new GameObject[6];
    public AudioClip[] audiues = new AudioClip[6];
    public int lastState;
    GameObject lastStatue;

	// Use this for initialization
	void Start () {

        main = GameObject.Find("main").gameObject.GetComponent<Main>();
        puzzle = main.GetComponent<StatuePuzzle>();

        if (puzzle.lastDoor <= statues.Length)
        {
            lastState = puzzle.lastDoor;

            audioSource = GetComponentInChildren<AudioSource>();
            lastStatue = (GameObject)Instantiate(statues[lastState], transform.position, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z)));
            audioSource.PlayOneShot(audiues[lastState]);

            if (lastState == statues.Length - 1)
            {
                final = lastStatue.GetComponent<InteractiveObject>();

                final.main = GameObject.Find("control").GetComponent<Main>();
                final.player = GameObject.Find("player").GetComponent<FirstPersonController>();

                final.interactText = main.Canvas.transform.FindChild("interact text").gameObject;
                main.ActiveQuest = Main.QuestType.hallway;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (puzzle.statue == null)
        {
            puzzle.statue = lastStatue;
        }
    }
}
