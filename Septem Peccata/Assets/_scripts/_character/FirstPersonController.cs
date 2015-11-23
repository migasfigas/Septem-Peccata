using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

//controladores: teclado + rato e comando xbox
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]

public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_RunSpeed;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_RunstepLenghten;
    [SerializeField]
    private float m_JumpSpeed;
    [SerializeField]
    private float m_StickToGroundForce;
    [SerializeField]
    private float m_GravityMultiplier;
    [SerializeField]
    private MouseLook m_MouseLook;
    [SerializeField]
    private bool m_UseFovKick;
    [SerializeField]
    private FOVKick m_FovKick = new FOVKick();
    [SerializeField]
    private bool m_UseHeadBob;
    [SerializeField]
    private CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField]
    private LerpControlledBob m_JumpBob = new LerpControlledBob();
    [SerializeField]
    private float m_StepInterval;
    [SerializeField]
    private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField]
    private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField]
    private AudioClip m_LandSound;           // the sound played when character touches back on ground.

    public Main main;

    private Camera m_Camera;
    private bool m_Jump;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;
    private AudioSource m_AudioSource;

    //-------------------------------original code--------------------------------------------//
    public Animator animator = new Animator();
    public Light candleLight;

    private bool rightHandActive;
    public enum weapon
    {
        none,
        crucifix,
        holyWater,
        holyShield,
        devineLight,
        specialObject
    };

    public weapon weaponState = weapon.none; //objeto que o jogador tem na mão atualmente
    private weapon currentWeaponState;
    public weapon maxWeapon = weapon.holyWater; //indica qual a ultima arma que o jogador possui seguindo a ordem da lista
    private bool switchWeapon;

    public GameObject[] weaponObjects;
    public GameObject lamp;

    bool reset = false;
    float animationTime = 0.0f;

    bool switching = false;

    // Use this for initialization
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_FovKick.Setup(m_Camera);
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();
        m_MouseLook.Init(transform, m_Camera.transform);

        rightHandActive = false;
        switchWeapon = false;

        weaponState = weapon.none;

        if (!main.selfQuest.Done)
        {
            animator.SetBool("hide lamp", true);
            lamp.gameObject.SetActive(false);
        }

        else
        {
            animator.SetBool("hide lamp", false);
            animator.SetTrigger("has lamp");
        }

    }


    // Update is called once per frame
    private void Update()
    {
        if (!main.chatting)
            RotateView();

        // the jump state needs to read here to make sure it is not missed
        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            StartCoroutine(m_JumpBob.DoBobCycle());
            PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }


    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }


    private void FixedUpdate()
    {
        float speed = 0;

        //se estiver a falar com uma npc não move
        if (!main.chatting)
            GetInput(out speed);

        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f);

        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;


        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;

            if (m_Jump)
            {
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        WeaponSwitch();
    }


    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }


    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed)) *
                         Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }


    private void PlayFootStepAudio()
    {
        if (!m_CharacterController.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }


    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!m_UseHeadBob)
        {
            return;
        }
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
        {
            m_Camera.transform.localPosition =
                m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                  (speed));
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }
        m_Camera.transform.localPosition = newCameraPosition;
    }


    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // set the desired speed to be walking or running
        speed = m_WalkSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        

        AnimatingAnimator(horizontal, vertical, speed);
        Attack();
    }

    private void AnimatingAnimator(float horizontal, float vertical, float speed)
    {

        if (horizontal != 0 || vertical != 0)
        {
            animator.SetFloat("forward", speed / 5);

            //sincroniza a animação da mão direita com a da mão esquerda
            if (animator.GetLayerWeight(1) > 0 && !reset && animator.GetCurrentAnimatorStateInfo(0).IsName("walk") &&
                !animator.GetBool("attack") && !animator.IsInTransition(1) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("action"))
            {
                animationTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                animator.Play("walk", 1, animationTime);
                reset = true;
            }
        }

        else
        {
            animator.SetFloat("forward", -1);

            if (animator.GetLayerWeight(1) > 0 && !reset && animator.GetCurrentAnimatorStateInfo(0).IsName("idle") &&
                !animator.GetBool("attack") && !animator.IsInTransition(1) && !animator.GetCurrentAnimatorStateInfo(1).IsTag("action"))
            {
                animationTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                animator.Play("idle", 1, animationTime);
                reset = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.L) && main.selfQuest.Done)
        {
            animator.SetBool("hide lamp", !animator.GetBool("hide lamp"));
            candleLight.enabled = !candleLight.enabled;
        }

        if (Input.GetAxis("Fire1") != 0)
        {
            animator.SetBool("attack", true);
            Attack();

            reset = false;
        }

        else
            animator.SetBool("attack", false);
    }

    private void Attack()
    {
        if (currentWeaponState != weapon.none)
        {
            //jogador perde sanidade quando ataca? how 2 do this
            switch (currentWeaponState)
            {
                case weapon.crucifix:
                    transform.Find("FirstPersonCharacter/priest/Padre_-_Make_Human/object_bone/crucifix").gameObject.GetComponent<SphereCollider>().enabled = animator.GetBool("attack");
                    break;

                case weapon.holyWater:
                    transform.Find("FirstPersonCharacter/priest/Padre_-_Make_Human/object_bone/holy_water").gameObject.GetComponent<CapsuleCollider>().enabled = animator.GetBool("attack");
                    break;

                default:
                    break;
            }
        }
    }

    private void RotateView()
    {
        if (!main.pause)
            m_MouseLook.LookRotation(transform, m_Camera.transform);
    }

    //só corre quando o jogador colide com alguma coisa, não o contrário
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);

        //o objeto em questão tem de ter um rigidbody não kinemático because of reasons
        if (hit.gameObject.CompareTag("self quest") && main.activeQuest == Main.CurrentQuest.first)
        {
            main.selfQuest.Done = true;
            DestroyObject(hit.gameObject);
            lamp.SetActive(true);
            candleLight.enabled = false;
            animator.SetTrigger("has lamp");
        }
    }

    private void WeaponSwitch()
    {
        //Get Input From The Mouse Wheel
        // if mouse wheel gives a positive value add 1 to WeaponNumber
        // if it gives a negative value decrease WeaponNumber with 1
        //impede que continue a fazer scroll quando chega a um dos limites

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentWeaponState > weapon.none)
            {
                weaponState--;
                animator.SetTrigger("switch");
            }

            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentWeaponState < maxWeapon)
            {
                weaponState++;
                animator.SetTrigger("switch");
            }
            //sync animações
            reset = false;
        }

        //Desliga a layer da mão direita caso o jogador esconda todas as armas
        if (weaponState != weapon.none && animator.GetLayerWeight(1) <= 1.0f)
        {
            if (animator.GetLayerWeight(1) < 0.5f)
                animator.SetLayerWeight(1, 0.5f);
            else
                animator.SetLayerWeight(1, animator.GetLayerWeight(1) + 0.02f);
        }

        else if (weaponState == weapon.none && animator.GetLayerWeight(1) >= 0.0f)
        {
            if (animator.GetLayerWeight(1) > 0.5f)
                animator.SetLayerWeight(1, animator.GetLayerWeight(1) - 0.02f);
            else
                animator.SetLayerWeight(1, 0);
        }


        SwitchWeapon((int)weaponState - 1);

        currentWeaponState = weaponState;

    }

    private void SwitchWeapon(int weapon)
    {
        Debug.Log((int)weapon);

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("up") && weapon != -1)
        {
            foreach (GameObject weapons in weaponObjects)
                weapons.SetActive(false);

            weaponObjects[weapon].SetActive(true);

            animator.SetInteger("weapon", weapon);
        }

        else
            animator.SetInteger("weapon", weapon);
    }
}