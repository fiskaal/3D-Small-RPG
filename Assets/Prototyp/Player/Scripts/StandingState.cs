using UnityEngine;

public class StandingState: State
{
    
    float gravityValue;
    bool jump;   
    bool crouch;
    Vector3 currentVelocity;
    bool grounded;
    bool sprint;
    float playerSpeed;
    private bool drawWeapon;
    private bool falling;
    private bool attack;

    Vector3 cVelocity;

    private float fallingTimer;

    public StandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
    {
        base.Enter();

        attack = false;
        jump = false;
        crouch = false;
        sprint = false;
        drawWeapon = false;
        falling = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        character.animator.applyRootMotion = false;
        
        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;

        fallingTimer = 0f;
        
        character.animator.SetBool("inCombat", false);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (jumpAction.triggered)
        {
            jump = true;
		}
		if (crouchAction.triggered)
		{
            crouch = true;
		}
		if (sprintInAction.triggered)
		{
            sprint = true;
		}
        if (drawWeaponAction.triggered)
        {
            drawWeapon = true;
        }

        if (attackAction.triggered)
        {
            attack = true;
        }
                
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
     
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if (sprint)
		{
            stateMachine.ChangeState(character.sprinting);
        }    
        if (jump)
        {
            stateMachine.ChangeState(character.jumping);
        }
		if (crouch)
		{
            stateMachine.ChangeState(character.crouching);
        }

        if (drawWeapon)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("drawWeapon");
        }

        if (attack)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("drawWeapon");
        }

        if (falling)
        {
            float fallTime = 1f;
            if (fallTime <= fallingTimer)
            {
                stateMachine.ChangeState((character.falling));
                character.animator.SetTrigger("fall");
            }
            fallingTimer += Time.deltaTime;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }

        if (Physics.Raycast(character.transform.position, Vector3.down, 2))
        {
            falling = false;
        }
        else
        {
            falling = true;
        }
       
        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity,ref cVelocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);
  
		if (velocity.sqrMagnitude>0)
		{
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity),character.rotationDampTime);
        }
        
    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

}
