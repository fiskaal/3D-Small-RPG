using UnityEngine;

public class LandingState:State
{
    float timePassed;
    float landingTime;

    public LandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
	{
		character = _character;
		stateMachine = _stateMachine;
	}

    public override void Enter()
	{
		base.Enter();
        timePassed = 0f;
        character.animator.SetTrigger("land");
        landingTime = 0.2f;
        
        //gravityVelocity.y += 5 * Time.deltaTime;
    }

    public override void LogicUpdate()
    {
        
        base.LogicUpdate();
		if (timePassed >= landingTime)
		{
			if (character.animator.GetBool("inCombat"))
			{
				character.animator.SetTrigger("move");
				stateMachine.ChangeState(character.combatting);
			}
			else
			{
				character.animator.SetTrigger("move");
				stateMachine.ChangeState(character.standing);
			}
        }
        timePassed += Time.deltaTime;
    }



}

