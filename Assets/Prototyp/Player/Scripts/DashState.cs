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
    }

    public override void Enter()
    {
        base.Enter();

        dash = false;
        character.animator.applyRootMotion = true;
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