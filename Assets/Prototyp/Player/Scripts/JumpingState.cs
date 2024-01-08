using UnityEngine;

public class JumpingState:State
{
    bool grounded;

    float gravityValue;
    float jumpHeight;
    float playerSpeed;

    Vector3 airVelocity;

    private float clipLength;
    private float clipSpeed;
    private float timePassed;
    private float jumpingTime;

    private bool sprint;
    
    public JumpingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
	{
		base.Enter();

		grounded = false;
        gravityValue = character.gravityValue;
        jumpHeight = character.jumpHeight;
        playerSpeed = character.playerSpeed;
        gravityVelocity.y = 0;

        character.animator.SetFloat("speed", 0);
        character.animator.SetTrigger("jump");
        Jump();

        timePassed = 0f;
        jumpingTime = 0.7f;

        sprint = false;
	}
	public override void HandleInput()
	{
		base.HandleInput();

        input = moveAction.ReadValue<Vector2>();

        if (sprintOutAction.triggered)
        {
	        character.isSprinting = false;
        }
    }

	public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (grounded)
		{
            //stateMachine.ChangeState(character.landing);
            if (character.animator.GetBool("inCombat"))
            {
	            stateMachine.ChangeState(character.combatting);
            }
            else
            {
	            if (!character.isSprinting)
	            {
		            stateMachine.ChangeState(character.standing);
	            }
	            else
	            {
		            stateMachine.ChangeState(character.sprinting);
	            }
            }

            character.animator.SetTrigger("land");

        }
        
        if (!grounded && timePassed >= jumpingTime)
        {
	        //stateMachine.ChangeState((character.falling));
        }

        
        
        timePassed += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
		if (!grounded)
		{

            velocity = character.playerVelocity;
            airVelocity = new Vector3(input.x, 0, input.y);

            velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
            velocity.y = 0f;
            airVelocity = airVelocity.x * character.cameraTransform.right.normalized + airVelocity.z * character.cameraTransform.forward.normalized;
            airVelocity.y = 0f;
            character.controller.Move(gravityVelocity * Time.deltaTime+ (airVelocity*character.airControl+velocity*(1- character.airControl))*playerSpeed*Time.deltaTime);
        }
		
		if (velocity.sqrMagnitude > 0)
		{
			character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
		}

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
    }

    void Jump()
    {
        gravityVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

}

