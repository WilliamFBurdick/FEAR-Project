using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private Vector3 momentum;
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpHeight);

        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = GetMoveDirection() * 5f * stateMachine.AirControl;
        Vector3 appliedMomentum = momentum * (1f - stateMachine.AirControl);
        Move(appliedMomentum + movement, deltaTime);
        Look(10f, deltaTime);

        if (stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.ChangeState(new PlayerFallState(stateMachine));
            return;
        }
    }
}
