using UnityEngine;
using System.Collections;

public class OpenDoors : MonoBehaviour
{
    public Main main;
    public GameObject plane;
    public int doorNumber;

    public GameObject[] otherDoors = new GameObject[7];

    float smooth = 2.0f;
    float DoorOpenAngle = 90.0f;
    public bool open;

    private Vector3 defaultRot;
    private Vector3 defaultPos;
    private Vector3 openRot;

    public GameObject interactText;
    public GameObject hallwayPrefab;
    public GameObject building;

    private bool created;
    private bool closed4ever = false;
    private bool colliding;
    private bool deleted;

    public bool right;
    public bool destroy;

    private GameObject generated;

    void Start()
    {
        defaultRot = transform.eulerAngles;
        defaultPos = transform.position;
        
        openRot = new Vector3(defaultRot.x, defaultRot.y - DoorOpenAngle, defaultRot.z);

        created = false;
        open = false;
        deleted = false;
        colliding = false;
        closed4ever = false;
        destroy = false;
    }
    
    void Update()
    {
        if (open)
        {
            //Open door
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
        }

        else if(!open && transform.eulerAngles.y < defaultRot.y)
        {
            //Close door
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
            

            if (closed4ever && deleted && gameObject.transform.localEulerAngles.y > 179)
            {
                DestroyObject(building);
                GetComponent<BoxCollider>().enabled = false;
                Destroy(GameObject.FindWithTag("last door standing"));

                transform.parent.gameObject.tag = "last door standing";

                for(int i = 0; i < otherDoors.Length; i++)
                    DestroyObject(otherDoors[i].gameObject);

               deleted = false;
            }

            else if(!closed4ever && destroy && gameObject.transform.localEulerAngles.y > 179)
            {
                DestroyObject(generated);
                destroy = false;
                created = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && colliding)
        {
            open = !open;

            if (!created)
            {
                if(!right)
                    generated = (GameObject)Instantiate(hallwayPrefab, new Vector3(defaultPos.x, 0, defaultPos.z), Quaternion.Euler(new Vector3(0, defaultRot.y + 90, 0)));
                else
                    generated = (GameObject)Instantiate(hallwayPrefab, new Vector3(defaultPos.x, 0, defaultPos.z), Quaternion.Euler(new Vector3(0, defaultRot.y - 90, 0)));

                plane.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);

                created = true;
            }
        }
    }
    
    //Activate the Main function when player is near the door
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            interactText.SetActive(true);
            colliding = true;
        }
    }

    //Deactivate the Main function when player is go away from door
    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            interactText.SetActive(false);
            colliding = false;
        }
    }

    public void Close4Ever()
    {
        open = false;
        closed4ever = true;
        deleted = true;
    }

    public void DestroyGenerated()
    {
        open = false;
        destroy = true;
    }
}
