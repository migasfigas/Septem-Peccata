using UnityEngine;
using System.Collections;

public class HallwayStatue : MonoBehaviour {

    public Main main;
    InteractiveObject final;
    AudioSource audioSource;

    public GameObject [] statues = new GameObject[6];
    public AudioClip[] audiues = new AudioClip[6];
    public int lastState;
    GameObject lastStatue;

	// Use this for initialization
	void Start () {

        if (main.lastDoor <= statues.Length)
        {
            lastState = main.lastDoor;

            audioSource = GetComponentInChildren<AudioSource>();
            lastStatue = (GameObject)Instantiate(statues[lastState], transform.position, transform.rotation);
            audioSource.PlayOneShot(audiues[lastState]);

            if (lastState == statues.Length - 1)
            {
                final = lastStatue.GetComponent<InteractiveObject>();

                final.main = GameObject.Find("control").GetComponent<Main>();
                final.player = GameObject.Find("player").GetComponent<FirstPersonController>();

                final.interactText = main.Canvas.transform.FindChild("interact text").gameObject;
                main.activeQuest = Main.CurrentQuest.hallway;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (main.statue == null)
        {
            main.statue = lastStatue;
        }
    }
}
