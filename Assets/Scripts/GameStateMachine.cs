using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameStateMachine
{
    public IState currentState { get; private set; }
    public IdleState idleState;
    public GrabbingState grabState;

    public event Action<IState> stateChanged;

    public GameStateMachine() {
        // create an instance for each state and pass in PlayerController
        this.grabState = new GrabbingState();
        this.idleState = new IdleState();
    }

    // set the starting state
    public void initialize(IState state) {
        currentState = state;
        state.Enter();

        // notify other objects that state has changed
        stateChanged?.Invoke(state);
    }

    public void TransitionTo(IState nextState) {
        currentState.Exit();
        currentState = nextState;
        nextState.Enter();

        // notify other objects that state has changed
        stateChanged?.Invoke(nextState);
    }


    public void execute() {
        if (currentState != null) {
            currentState.Execute();
        }
    }

}
