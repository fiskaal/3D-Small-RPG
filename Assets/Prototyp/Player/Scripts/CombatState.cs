using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State
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
    
    public CombatState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        character.animator.ResetTrigger("move");
        
        block = false;
        jump = false;
        sheateWeapon = false;
        attack = false;
        specialAttack = false;
        dash = false;
        input = Vector2.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;

        character.animator.applyRootMotion = false;
        
        velocity = character.playerVelocity;
        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
        
        character.animator.SetBool("inCombat", true);
        character.animator.SetBool("blocking", false);
        character.animator.ResetTrigger("land");
    }

    public override void HandleInput()
    {
        base.HandleInput();
        
        if (jumpAction.triggered)
        {
            jump = true;
        }

        if (drawWeaponAction.triggered)
        {
            sheateWeapon = true;
        }

        if (attackAction.triggered)
        {
            attack = true;
        }

        if (specialAttackSlotAction.triggered)
        {
            if (character.currentSpecialAttackTrigger != null && character.currentSpecialAttackisReady && character.fireBallIsActive)
            {
                character.animator.SetTrigger(character.currentSpecialAttackTrigger);
                character.currentSpecialAttackTimePassed = 0f;
            }
        }

        if (dashAction.triggered)
        {
            dash = true;
        }

        if (blockActionStart.triggered)
        {
            block = true;
        }

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized +
                   velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if (sheateWeapon)
        {
            character.animator.SetBool("inCombat", false);
            character.animator.SetTrigger("sheatWeapon");
            stateMachine.ChangeState(character.standing);
        }
            
        if (attack && !sheateWeapon)
        {
            character.animator.SetBool("moveBool", false);
            //character.animator.SetTrigger("attack");
            stateMachine.ChangeState(character.attacking); 
        }

        if (specialAttack)
        {
            //character.animator.SetTrigger(("specialAttack"));
            //stateMachine.ChangeState(character.heavyAttacking);
        }

        if (dash && character.dashIsReady && !sheateWeapon)
        {
            character.animator.SetTrigger(("dash"));
            stateMachine.ChangeState(character.dashing);
        }
        else
        {
            dash = false;
        }
        
        if (jump)
        {
            character.animator.SetTrigger("jump");
            stateMachine.ChangeState(character.jumping);
        }

        if (block && !sheateWeapon)
        {
            if (!character.blockBroken)
            {
                character.animator.SetBool("blocking", true);
                character.animator.SetTrigger("block");
                stateMachine.ChangeState(character.blocking);
            }
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
