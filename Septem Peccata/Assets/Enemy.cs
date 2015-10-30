using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public Main main;
    public int health = 100;

	void Start () {
	
	}
	
	void Update () {

        if (health == 0)
            Die();
	}

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
            main.sanity--;

        if (col.gameObject.CompareTag("crucifix"))
            health -= 2;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
