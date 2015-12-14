using UnityEngine;
using System.Collections;

public class DoorClose : MonoBehaviour {

    public GameObject parent;
    private OpenDoors parentScript;

	// Use this for initialization
	void Start () {

        parentScript = parent.GetComponent<OpenDoors>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            parentScript.Close4Ever();
        }
    }
}
