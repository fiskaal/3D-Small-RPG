//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float playerSpeed = 5.0f;
    public float crouchSpeed = 2.0f;
    public float sprintSpeed = 7.0f;
    public float jumpHeight = 0.8f; 
    public float gravityMultiplier = 2;
    public float rotationSpeed = 5f;
    public float crouchColliderHeight = 1.35f;

    [Header("Animation Smoothing")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;
    [Range(0, 1)]
    public float velocityDampTime = 0.9f;
    [Range(0, 1)]
    public float rotationDampTime = 0.2f;
    [Range(0, 1)]
    public float airControl = 0.5f;

    public StateMachine movementSM;
    public StandingState standing;
    public JumpingState jumping;
    public CrouchingState crouching;
    public LandingState landing;
    public SprintState sprinting;
    public SprintJumpState sprintjumping;
    public CombatState combatting;
    public AttackState attacking;
    public HeavyAttackState heavyAttacking;
    public DashState dashing;
    public FallingState falling;
    public BlockingState blocking;

    [HideInInspector] public float gravityValue = -9.81f;
    [HideInInspector]
    public float normalColliderHeight;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform cameraTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 playerVelocity;

    [Header("Added controlls")] 
    public bool blockingStateActive;
    public float dashCooldownTime;
    public bool dashIsReady;

    [Header("VFX")] 
    public GameObject blockVFX;
    public BlockVFX blockVFXScript;
    public bool blockIsUgraded;

    [Header("Block")] 
    public BlockBreaker _blockBreaker;
    public bool blockBroken = false;

    [Header("SpecialAttackActions")] 
    public bool fireBallIsActive = false;
    public string currentSpecialAttackTrigger;
    public float currentSpecialAttackCooldown;
    public bool currentSpecialAttackisReady = false;
    public float currentSpecialAttackTimePassed = 0f;

    [Header("Sprint")] 
    public bool isSprinting;

    [Header("EquipmentApperance")] 
    public EquipmentSystem _equipmentSystem;

    [Header("Audio")] 
    public PlayerAudioScript playerAudioScript;
    
    
    //dash coolDownCalculator
    public float dashTimePassed;
    
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM);
        jumping = new JumpingState(this, movementSM);
        crouching = new CrouchingState(this, movementSM);
        landing = new LandingState(this, movementSM);
        sprinting = new SprintState(this, movementSM);
        sprintjumping = new SprintJumpState(this, movementSM);
        combatting = new CombatState(this, movementSM);
        attacking = new AttackState(this, movementSM);
        heavyAttacking = new HeavyAttackState(this, movementSM);
        dashing = new DashState(this, movementSM);
        falling = new FallingState(this, movementSM);
        blocking = new BlockingState(this, movementSM);

        movementSM.Initialize(standing);

        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;
    }

    private void Update()
    {
        movementSM.currentState.HandleInput();

        movementSM.currentState.LogicUpdate();

        if (dashCooldownTime <= dashTimePassed)
        {
            dashIsReady = true;
        }
        else
        {
            dashIsReady = false;
        }

        dashTimePassed += Time.deltaTime;
        
        UpdateSpecialAttackActionCoolDown();

        if (fireBallIsActive)
        {
            Debug.Log("FireBall Is Active");
        }
    }

    private void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }
    
    public void Move(Vector3 moveDirection, float moveSpeed)
    {
        // Calculate the character's movement vector
        Vector3 movement = moveDirection.normalized * moveSpeed * Time.deltaTime;

        // Move the character using the CharacterController
        controller.Move(movement);
    }

    private void UpdateSpecialAttackActionCoolDown()
    {
        if (currentSpecialAttackCooldown <= currentSpecialAttackTimePassed)
        {
            currentSpecialAttackisReady = true;
        }
        else
        {
            currentSpecialAttackisReady = false;
        }
        currentSpecialAttackTimePassed += Time.deltaTime;
    }
}
