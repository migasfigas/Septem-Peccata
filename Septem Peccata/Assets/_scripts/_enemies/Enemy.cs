using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public Main main;
    public int health = 100;
    public bool canAtack;

    //random posições treme bué antes de explodir 

	void Start () {
        canAtack = true;
	}
	
	void Update () {

        if (health == 0)
            Die();


	}

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if(canAtack) main.temptation++;
        }

        if (col.gameObject.CompareTag("crucifix"))
        {
            canAtack = false;
        }

        if (col.gameObject.CompareTag("holy water"))
        {
            health -= 2;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.CompareTag("crucifix"))
        {
            canAtack = true;
        }
    }


    private void Die()
    {
        Destroy(gameObject);
    }
}
