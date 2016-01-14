using UnityEngine;
using System.Collections;

public class TouchMyOrb : MonoBehaviour {

    [SerializeField] private int currentCheckpoint;
    [SerializeField] private GameObject nextCheckpoint;
    [SerializeField] private GameObject parent;

    private bool colliding = false;
    private float nextCheckpointPosition;
    private Renderer rend;

	void Start () {
        rend = GetComponent<Renderer>();

        nextCheckpointPosition = nextCheckpoint.transform.position.y;
        parent = GameObject.Find("tiny platforms " + currentCheckpoint).gameObject;
	}
	
	void Update () {

        if (nextCheckpoint != null && colliding)
        {
            nextCheckpoint.transform.position = Vector3.Lerp(nextCheckpoint.transform.position, 
                new Vector3(nextCheckpoint.transform.position.x, -3.7f, nextCheckpoint.transform.position.z), Time.deltaTime);

            parent.transform.position = Vector3.Lerp(parent.transform.position, 
                new Vector3(parent.transform.position.x, nextCheckpointPosition, parent.transform.position.z), Time.deltaTime);
        }
    }

    IEnumerator FadeOrb()
    {
        while(rend.material.GetColor("_Color").a > 0.01f)
        {
            rend.material.SetColor("_Color", new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, rend.material.color.a-0.025f));

            yield return new WaitForSeconds(0.1f);
        }

        DestroyObject(gameObject);
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            StartCoroutine(FadeOrb());
            colliding = true;
        }
    }
}
