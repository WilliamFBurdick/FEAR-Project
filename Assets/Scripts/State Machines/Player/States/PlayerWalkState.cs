using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.Input.CrouchEvent += OnCrouch;
        stateMachine.Input.JumpEvent += OnJump;
    }

    public override void Exit()
    {
        stateMachine.Input.CrouchEvent -= OnCrouch;
        stateMachine.Input.JumpEvent -= OnJump;
    }

    public override void Tick(float deltaTime)
    {
        Vector2 lookIn = stateMachine.Input.LookValue;
        Look(10f, deltaTime);
        // Move player
        bool grounded = Move(GetMoveDirection() * 5f, deltaTime);
        if (!stateMachine.Controller.isGrounded && !grounded) { stateMachine.ChangeState(new PlayerFallState(stateMachine)); }
    }

    private void OnJump()
    {
        stateMachine.ChangeState(new PlayerJumpState(stateMachine));
    }

    private void OnCrouch()
    {
        stateMachine.ChangeState(new PlayerCrouchState(stateMachine));
    }
}
