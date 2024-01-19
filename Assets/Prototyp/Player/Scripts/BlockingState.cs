using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingState : State
{
    bool jump;   
    float gravityValue;
    Vector3 currentVelocity;
    bool grounded;
    float playerSpeed;
    private bool sheateWeapon;
    private bool attack;
    private bool specialAttack;
    private bool dash;

    private bool block;

    Vector3 cVelocity;

    public BlockingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        if (character.blockIsUgraded)
        {
            character.blockVFX.SetActive(true);
        }

        input = Vector2.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        velocity = character.playerVelocity;
        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;

        block = false;
        character.animator.SetBool("moveFromBlock", false);

        character.blockingStateActive = true;
        character._blockBreaker.blocking = true;

        Vector3 cameraForward = character.cameraTransform.forward;
        cameraForward.y = 0; // Set the Y component to zero
        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(cameraForward),50 * Time.deltaTime);

    }

    public override void HandleInput()
    {
        base.HandleInput();
        
        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized +
                   velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
        
        /*
        float newYRotation = character.cameraTransform.rotation.eulerAngles.y;
        // Set the player's rotation to match the camera's y-axis rotation
        character.transform.rotation = Quaternion.Euler(0f, newYRotation, 0f);
        */

        if (blockActionEnd.triggered)
        {
            block = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);
        
        if (block)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("move");
            character.animator.SetBool("moveFromBlock", true);
        }

        if (character.blockBroken)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("move");
            character.animator.SetBool("moveFromBlock", true);
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
       
        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity,ref cVelocity, character.velocityDampTime);
        character.controller.Move(currentVelocity * Time.deltaTime * (playerSpeed/2) + gravityVelocity * Time.deltaTime);
        
        if (velocity.sqrMagnitude>0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity),character.rotationDampTime);
        }
        
        //character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity),character.rotationDampTime);

    }

    public override void Exit()
    {
        base.Exit();

        character.animator.SetBool("blocking", false);
        
        if (character.blockIsUgraded)
        {
            character.blockVFXScript.DisableAfterAnimation();
        }
        
        character._blockBreaker.blocking = false;
        character.blockingStateActive = false;

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}
