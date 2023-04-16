using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine stateMachine;
    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected bool Move(Vector3 motion, float deltaTime)
    {
        bool grounded;
        if (Physics.Raycast(stateMachine.transform.position, -stateMachine.transform.up, out RaycastHit hit, .5f, ~LayerMask.NameToLayer("Player")))
        {
            motion = motion.ProjectOntoPlane(hit.normal);
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        return grounded;
    }

    protected bool Move(float deltaTime)
    {
        bool grounded;
        if (Physics.Raycast(stateMachine.transform.position, -stateMachine.transform.up, .5f, ~LayerMask.NameToLayer("Player")))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        Move(Vector3.zero, deltaTime);
        return grounded;
    }

    protected void Look(float sensitivity, float deltaTime)
    {
        float xAmount = stateMachine.Input.LookValue.x * sensitivity * deltaTime;
        float yAmount = stateMachine.Input.LookValue.y * sensitivity * deltaTime;

        // Horizontal look
        stateMachine.transform.eulerAngles += new Vector3(0f, xAmount, 0f);

        // Vertical look
        stateMachine.EyesXRot -= yAmount;
        stateMachine.EyesXRot = Mathf.Clamp(stateMachine.EyesXRot, -stateMachine.DefaultCameraXBounds, stateMachine.DefaultCameraXBounds);

        stateMachine.Eyes.localEulerAngles = new Vector3(stateMachine.EyesXRot, 0f, 0f);
    }

    protected Vector3 GetMoveDirection()
    {
        Vector2 moveIns = stateMachine.Input.MovementValue;
        Vector3 horizMoveDir = stateMachine.transform.TransformDirection(moveIns.x, 0f, moveIns.y).normalized;
        return horizMoveDir;
    }
}
