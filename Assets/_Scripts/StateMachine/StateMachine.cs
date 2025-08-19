using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//半山腰太挤，你总得去山顶看看//
public class StateMachine
{
    public State currentState { get; private set; }

    public void Initialize(State startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangState(State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
