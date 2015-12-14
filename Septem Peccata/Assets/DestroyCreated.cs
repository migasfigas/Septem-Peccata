using UnityEngine;
using System.Collections;

public class DestroyCreated : MonoBehaviour {

    public OpenDoors parentScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            parentScript.DestroyGenerated();
        }
    }
}
