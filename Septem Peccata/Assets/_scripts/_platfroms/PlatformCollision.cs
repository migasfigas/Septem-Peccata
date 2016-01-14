using UnityEngine;
using System.Collections;

public class PlatformCollision : MonoBehaviour {

    private ShakingPlatform cameraShake;
    private bool fall = false;
    
	void Start () {
        cameraShake = GameObject.Find("player/FirstPersonCharacter").GetComponent<ShakingPlatform>();
	}
	
	void Update () {

        if(cameraShake == null) cameraShake = GameObject.Find("player/FirstPersonCharacter").GetComponent<ShakingPlatform>();

        if (fall)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -30, transform.position.z), Time.deltaTime);
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player") && !fall)
        {
            cameraShake.waitingtime -= Time.deltaTime;

            if (cameraShake.waitingtime <= 0 && cameraShake.shake == 0)
            {
                cameraShake.shake = 1;
            }

            if (cameraShake.waitingtime <= -1)
            {
                fall = true;
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            cameraShake.shake = 0;
            cameraShake.waitingtime = 1f;
        }
    }
}
