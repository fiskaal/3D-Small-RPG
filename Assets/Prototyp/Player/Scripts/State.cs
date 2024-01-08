using UnityEngine;
using UnityEngine.InputSystem;

public class State
{
    public Character character;
    public StateMachine stateMachine;

    protected Vector3 gravityVelocity;
    protected Vector3 velocity;
    protected Vector2 input;

    public InputAction moveAction;
    public InputAction lookAction;
    public InputAction jumpAction;
    public InputAction crouchAction;
    public InputAction sprintInAction;
    public InputAction sprintOutAction;
    public InputAction drawWeaponAction;
    public InputAction attackAction;
    public InputAction specialAttackSlotAction;
    public InputAction heavyattackAction1;
    public InputAction dashAction;
    public InputAction blockActionStart;
    public InputAction blockActionEnd;


    public State(Character _character, StateMachine _stateMachine)
	{
        character = _character;
        stateMachine = _stateMachine;

        moveAction = character.playerInput.actions["Move"];
        lookAction = character.playerInput.actions["Look"];
        jumpAction = character.playerInput.actions["Jump"];
        crouchAction = character.playerInput.actions["Crouch"];
        sprintInAction = character.playerInput.actions["SprintIn"];
        sprintOutAction = character.playerInput.actions["SprintOut"];
        drawWeaponAction = character.playerInput.actions["DrawWeapon"];
        attackAction = character.playerInput.actions["Attack"];
        specialAttackSlotAction = character.playerInput.actions["HeavyAttack"];
        heavyattackAction1 = character.playerInput.actions["HeavyAttack1"];
        dashAction = character.playerInput.actions["Dash"];
        blockActionStart = character.playerInput.actions["BlockStart"];
        blockActionEnd = character.playerInput.actions["BlockEnd"];

        if (crouchAction.triggered)
        {
            Debug.Log("CrouchTriggered");
        }
        
        if (moveAction.triggered)
        {
            Debug.Log("MoveTriggered");
        }
        
        if (sprintInAction.triggered)
        {
            Debug.Log("SprintTriggered");
        }

        if (blockActionStart.triggered)
        {
            Debug.Log("BlockTriggered");
        }

    }

    public virtual void Enter()
    {
        Debug.Log("enter state: "+this.ToString());
    }

    public virtual void HandleInput()
    {
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Exit()
    {
    }
}

