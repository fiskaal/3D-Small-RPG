using UnityEngine;
public class AttackState : State
{
    float timePassed;
    float clipLength;
    float clipSpeed;
    bool attack;
    private bool dash;
    public AttackState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        
        attack = false;
        dash = false;
        character.animator.applyRootMotion = true;
        timePassed = 0f;
        character.animator.SetTrigger("attack");
        character.animator.SetFloat("speed", 0f);

        /*
        float newYRotation = character.cameraTransform.rotation.eulerAngles.y;
        // Set the player's rotation to match the camera's y-axis rotation
        character.transform.rotation = Quaternion.Euler(0f, newYRotation, 0f);
        */
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
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timePassed += Time.deltaTime;
        clipLength = character.animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
        clipSpeed = character.animator.GetCurrentAnimatorStateInfo(1).speed;

        if (attack)
        {
            stateMachine.ChangeState(character.attacking);
        }
        if (timePassed >= clipLength / clipSpeed)
        {
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("move");
        }

        /*
        if (dash)
        {
            character.animator.SetTrigger("dash");
            stateMachine.ChangeState(character.dashing);
        }
        */
    }
    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = false;
    }
}

