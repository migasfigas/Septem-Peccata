using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () {

        activeQuest = Quest.none;

	}
	
	// Update is called once per frame
	void Update () {
        
    }


}
