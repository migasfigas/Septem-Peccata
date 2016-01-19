using UnityEngine;
using System.Collections;

public class OpenDoors : MonoBehaviour
{
    [SerializeField] private Main main;
    public StatuePuzzle puzzle;
    public int doorNumber;

    public GameObject[] otherDoors = new GameObject[7]; //lista de portas em cada corredor

    float smooth = 2.5f;
    float DoorOpenAngle = 90.0f;
    public bool open;

    private Vector3 defaultRot;
    private Vector3 defautRotGlobal;
    private Vector3 defaultPos;
    private Vector3 openRot;

    public GameObject interactText;
    public GameObject hallwayPrefab;
    public GameObject building;
    private AudioSource audioSource;

    private bool created;
    private bool closed4ever;
    private bool colliding;
    private bool deleted;
    private bool destroy;
    private bool changed;

    public bool right; //de que lado está a porta

    private GameObject generated; //cada porta gera um novo corredor

    void Start()
    {
        main = GameObject.Find("main").gameObject.GetComponent<Main>();
        puzzle = main.GetComponent<StatuePuzzle>();
        interactText = main.transform.Find("Canvas/interact text").gameObject;

        defaultRot = transform.localEulerAngles;
        defautRotGlobal = transform.eulerAngles;
        defaultPos = transform.FindChild("body").position;
        
        openRot = new Vector3(defaultRot.x, defaultRot.y - DoorOpenAngle, defaultRot.z);
        audioSource = GetComponent<AudioSource>();

        created = false;
        open = false;
        deleted = false;
        colliding = false;
        closed4ever = false;
        destroy = false;
        changed = false;

        if (main.ActiveQuest == Main.QuestType.hallway)
        {
            enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    
    void Update()
    {
        if (open)
        {
            //Open door
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, openRot, Time.deltaTime * smooth);
        }

        else if(!open && transform.localEulerAngles.y < defaultRot.y)
        {
            //Close door 4ever
            transform.localEulerAngles = Vector3.Slerp(transform.localEulerAngles, defaultRot, Time.deltaTime * smooth);

            if (closed4ever && deleted && gameObject.transform.localEulerAngles.y > 170)
            {
                DestroyObject(building);
                GetComponent<BoxCollider>().enabled = false;

                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("last door standing");
                foreach (GameObject target in gameObjects)
                {
                    GameObject.Destroy(target);
                }

                transform.parent.gameObject.tag = "last door standing";

                for(int i = 0; i < otherDoors.Length; i++)
                    DestroyObject(otherDoors[i].gameObject);

               deleted = false;
            }

            //close generated
            else if(!closed4ever && destroy && gameObject.transform.localEulerAngles.y > 170)
            {
                if (changed)
                {
                    puzzle.LastDoor--;
                    changed = false;
                }

                GameObject.Find(generated.name + "/model/statue").GetComponent<HallwayStatue>().DestroyStatue();
                DestroyObject(generated);
                destroy = false;
                created = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && colliding)
        {
            open = !open;

            audioSource.Play();

            if (!created)
            {
                if(!right)
                    generated = (GameObject)Instantiate(hallwayPrefab, new Vector3(defaultPos.x, transform.parent.position.y + 0.001f, defaultPos.z), Quaternion.Euler(new Vector3(0, defautRotGlobal.y + 90, 0)));
                else
                    generated = (GameObject)Instantiate(hallwayPrefab, new Vector3(defaultPos.x, transform.parent.position.y + 0.001f, defaultPos.z), Quaternion.Euler(new Vector3(0, defautRotGlobal.y - 90, 0)));

                transform.position += new Vector3(0.03f, 0, 0);

                if (puzzle.DoorSequence.Length >= puzzle.LastDoor && puzzle.DoorSequence[puzzle.LastDoor] == doorNumber)
                {
                    puzzle.LastDoor++;
                    changed = true;
                }
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


    //quando o jogador passa a porta para o outro lado a porta é fechada para sempre (não pode voltar para trás)
    public void Close4Ever()
    {
        open = false;
        closed4ever = true;
        deleted = true;

        puzzle.Statue.gameObject.tag = "last door standing";

        audioSource.Play();

    }

    //se o jogador abre a porta e volta para trás o nível gerado é destruído
    public void DestroyGenerated()
    {
        if (generated != null)
        {
            open = false;
            destroy = true;
            puzzle.Statue.gameObject.tag = "last door standing";
        }
    }
}
