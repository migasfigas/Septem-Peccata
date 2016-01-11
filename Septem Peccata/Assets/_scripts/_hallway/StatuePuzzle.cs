using UnityEngine;
using System.Collections;

public class StatuePuzzle : MonoBehaviour {

    //2 7 4 3 6 0 1
    [SerializeField] private int lastDoor = 0; //cada vez que incrementa modifica a estatua
    [SerializeField] private int[] doorSequence = { 2, 7, 4, 3, 6, 0 };
    [SerializeField] private GameObject statue;
    [SerializeField] private GameObject nextLevelDoor;
    [SerializeField] private Main main;
    
    
    //cenas especificas do nivel
    void Start()
    {
        main = GameObject.Find("main").gameObject.GetComponent<Main>();
    }

    void Update()
    {
        if(main.HallwayQuest.Done == true && statue.GetComponent<BoxCollider>().enabled == true)
        {
            nextLevelDoor = GameObject.Find("hidden door").gameObject;
            nextLevelDoor.GetComponent<BoxCollider>().enabled = true;
            nextLevelDoor.AddComponent<loadNext>();
            nextLevelDoor.GetComponent<loadNext>().Scene = 3;

            statue.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public int[] DoorSequence
    {
        get { return doorSequence; }
    }

    public int LastDoor
    {
        get { return lastDoor; }
        set { lastDoor = value; }
    }

    public GameObject Statue
    {
        get { return statue; }
        set { statue = value; }
    }
}
