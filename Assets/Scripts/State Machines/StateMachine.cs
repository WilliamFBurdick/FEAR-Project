using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State currentState;

    protected void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
