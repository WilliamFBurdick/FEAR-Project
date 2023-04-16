using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerBaseState
{
    private float crouchHeightScale = 0.5f;
    public PlayerCrouchState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Controller.height *= crouchHeightScale;
        stateMachine.Controller.center = new Vector3(stateMachine.Controller.center.x, stateMachine.Controller.center.y * crouchHeightScale, stateMachine.Controller.center.z);
        stateMachine.Eyes.localPosition = new Vector3(stateMachine.Eyes.localPosition.x, stateMachine.Eyes.localPosition.y * crouchHeightScale, stateMachine.Eyes.localPosition.z);

        stateMachine.Input.CrouchEvent += OnCrouch;
        stateMachine.Input.JumpEvent += OnJump;
    }

    public override void Exit()
    {
        stateMachine.Controller.height *= (1 / crouchHeightScale);
        stateMachine.Controller.center = new Vector3(stateMachine.Controller.center.x, stateMachine.Controller.center.y * (1 / crouchHeightScale), stateMachine.Controller.center.z);
        stateMachine.Eyes.localPosition = new Vector3(stateMachine.Eyes.localPosition.x, stateMachine.Eyes.localPosition.y * (1 /crouchHeightScale), stateMachine.Eyes.localPosition.z);

        stateMachine.Input.CrouchEvent -= OnCrouch;
        stateMachine.Input.JumpEvent -= OnJump;
    }

    public override void Tick(float deltaTime)
    {
        Vector2 lookIn = stateMachine.Input.LookValue;
        Look(10f, deltaTime);
        // Move player
        bool grounded = Move(GetMoveDirection() * 4f, deltaTime);
        if (!stateMachine.Controller.isGrounded && !grounded) { stateMachine.ChangeState(new PlayerFallState(stateMachine)); }
    }

    private void OnCrouch()
    {
        stateMachine.ChangeState(new PlayerWalkState(stateMachine));
    }

    private void OnJump()
    {
        stateMachine.ChangeState(new PlayerWalkState(stateMachine));
    }
}
