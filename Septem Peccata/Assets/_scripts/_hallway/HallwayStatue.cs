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

        if (puzzle.LastDoor <= statues.Length)
        {
            lastState = puzzle.LastDoor;

            audioSource = GetComponentInChildren<AudioSource>();
            lastStatue = (GameObject)Instantiate(statues[lastState], transform.position, Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z)));
            audioSource.PlayOneShot(audiues[lastState]);

            if (lastState == statues.Length - 1)
            {
                final = lastStatue.GetComponent<InteractiveObject>();

                main.ActiveQuest = Main.QuestType.hallway;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (puzzle.Statue == null)
        {
            puzzle.Statue = lastStatue;
        }
    }
}
