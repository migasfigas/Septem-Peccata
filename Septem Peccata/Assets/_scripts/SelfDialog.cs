using UnityEngine;
using System.Collections;

public class SelfDialog : MonoBehaviour {

    public Main main;
    public GameObject interactText, dialogBox;

    Chitchat dialog;

	// Use this for initialization
	void Start () {
        dialog = new Chitchat(main, Main.NPCs.meMyselfAndI, dialogBox, interactText);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
