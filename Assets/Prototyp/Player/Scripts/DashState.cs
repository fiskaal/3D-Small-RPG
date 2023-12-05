/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : State
{
    float timePassed;
    float clipLength;
    float clipSpeed;
    bool dash;

    public DashState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
        character.animator.applyRootMotion = true;
    }

    public override void Enter()
    {
        base.Enter();

        dash = false;
        //character.animator.applyRootMotion = true;
        timePassed = 0f;
        character.animator.SetTrigger("dash");
        character.animator.SetFloat("speed", 0f);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (specialAttackAction.triggered)
        {
            dash = true;
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timePassed += Time.deltaTime;
        clipLength = character.animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
        clipSpeed = character.animator.GetCurrentAnimatorStateInfo(1).speed;

        if (timePassed >= clipLength / clipSpeed && dash)
        {
            stateMachine.ChangeState(character.dashing);
        }
        if (timePassed >= clipLength / clipSpeed)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("move");
        }

    }
    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = false;
    }
}
*/

using UnityEngine;

public class DashState : State
{
    private float dashSpeed = 10.0f; // Adjust the dash speed as needed
    private float dashDuration = 0.5f; // Adjust the dash duration as needed
    private float dashTimer = 0f;
    private Vector3 dashDirection = Vector3.zero;
    
    float gravityValue;
    bool grounded;
    
    public DashState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        character.animator.SetTrigger("dash");
        
        //reset dash coolDownTimePassed
        character.dashTimePassed = 0f;
        
        // Calculate the dash direction based on input
        Vector2 input = moveAction.ReadValue<Vector2>();
        dashDirection = character.cameraTransform.forward * input.y + character.cameraTransform.right * input.x;
        dashDirection.y = 0f;
        dashDirection.Normalize();

        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(dashDirection), 100 * Time.deltaTime);
        
        dashTimer = 0f;
        
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue +500;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Apply gravity during the dash
        character.Move(Vector3.down, gravityValue * Time.deltaTime);
        // Perform the dash movement without root motion
        character.Move(dashDirection, dashSpeed);

        dashTimer += Time.deltaTime;

        if (dashTimer >= dashDuration)
        {
            stateMachine.ChangeState(character.combatting); // Change to another state when the dash is complete
            character.animator.SetTrigger("move");
        }
    }
}
