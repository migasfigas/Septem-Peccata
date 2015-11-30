using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Enemy))]
public class DumbAI : MonoBehaviour {

    public NavMeshAgent agent { get; private set; }
    public Enemy enemy { get; private set; }

    public Transform target;

	private void Start () {

        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();

        agent.updateRotation = false;
        agent.updatePosition = true;
	}
	
	private void Update ()
    {
        if(target != null)
        {
            if(Physics.OverlapSphere(transform.position, 5, 1 << 9).Length > 0)
            { agent.SetDestination(target.position);

                enemy.Move(agent.desiredVelocity, target.transform.position);
            }
        }

        else
        {
            enemy.Move(Vector3.zero, Vector3.zero);
        }
	}

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
