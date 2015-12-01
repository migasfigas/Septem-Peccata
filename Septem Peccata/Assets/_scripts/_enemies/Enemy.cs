using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public Main main;
    private Animator animator;
    public int health = 100;
    public bool canAtack;
    public  float damping = 3;

    //random posições treme bué antes de explodir 
    //resolver ontriggerexit (crucifix desliga collider não deteta saida)

	private void Start ()
    {
        animator = GetComponent<Animator>();
        canAtack = true;
	}
	
	private void Update ()
    {
        if (health == 0)
            Die();

        if (!main.playerAttacking)
            canAtack = true;
	}

    public void Move(Vector3 move, Vector3 target)
    {
        transform.Translate(move * Time.deltaTime);

        Vector3 lookPos = target - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime*damping);

        UpdateAnimator(move);
    }

    private void UpdateAnimator(Vector3 move)
    {
        if (move != Vector3.zero && animator.GetFloat("forward") == 0)
            animator.SetFloat("forward", 1);
        else if (move == Vector3.zero && animator.GetFloat("forward") == 1)
            animator.SetFloat("forward", 0);
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if(canAtack) main.temptation++;
        }

        if (col.gameObject.CompareTag("crucifix") && main.playerAttacking)
        {
            canAtack = false;
        }

        if (col.gameObject.CompareTag("holy water") && main.playerAttacking)
        {
            health -= 2;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
