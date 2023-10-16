using UnityEngine;
public class SprintJumpState:State
{
    float timePassed;
    float jumpTime;

    public SprintJumpState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
	{
		base.Enter();
        character.animator.applyRootMotion = true;
        timePassed = 0f;
        character.animator.SetTrigger("sprintJump");

        jumpTime = 0f;
    }

	public override void Exit()
	{
		base.Exit();
        character.animator.applyRootMotion = false;
    }

	public override void LogicUpdate()
    {
        
        base.LogicUpdate();
        
        jumpTime = (character.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / character.animator.GetCurrentAnimatorStateInfo(0).speed - 0.1f);
        
		if (timePassed> jumpTime)
		{
            character.animator.SetTrigger("move");
            stateMachine.ChangeState(character.sprinting);
        }
        timePassed += Time.deltaTime;
    }



}

