using UnityEngine;
public class AttackState : State
{
    float timePassed;
    float clipLength;
    float clipSpeed;
    bool attack;
    private bool dash;
    private bool block;
    private float numberOfAttacks = 2;
    private float attackNumber;

    public AttackState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        character._equipmentSystem.AttackShowWeaponInHand();
        
        block = false;
        attack = false;
        dash = false;
        character.animator.applyRootMotion = true;
        timePassed = 0f;
        character.animator.SetTrigger("attack");
        character.animator.SetFloat("speed", 0f);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (attackAction.triggered)
        {
            attack = true;
        }

        if (dashAction.triggered)
        {
            dash = true;
        }
        
        if (blockActionStart.triggered)
        {
            block = true;
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timePassed += Time.deltaTime;
        clipLength = character.animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
        clipSpeed = character.animator.GetCurrentAnimatorStateInfo(1).speed;

        if (attack && timePassed >= clipLength / clipSpeed && attackNumber < numberOfAttacks)
        {
            stateMachine.ChangeState(character.attacking);
            attackNumber += 1;
        }
        
        if (timePassed >= clipLength / clipSpeed)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("move");
            attackNumber = 0;
        }
        
        if (block)
        {
            if (!character.blockBroken)
            {
                character.animator.SetBool("blocking", true);
                character.animator.SetTrigger("block");
                stateMachine.ChangeState(character.blocking);
                attackNumber = 0;
            }
        }

        
        if (dash && character.dashIsReady)
        {
            //character.animator.SetTrigger("dash");
            stateMachine.ChangeState(character.dashing);
            attackNumber = 0;
        }
        else
        {
            dash = false;
        }
    }
    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = false;
    }
}

