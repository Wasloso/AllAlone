﻿using System;
using System.Collections.Generic;

// Notes
// 1. What a finite state machine is
// 2. Examples where you'd use one
//     AI, Animation, Game State
// 3. Parts of a State Machine
//     States & Transitions
// 4. States - 3 Parts
//     Tick - Why it's not Update()
//     OnEnter / OnExit (setup & cleanup)
// 5. Transitions
//     Separated from states so they can be re-used
//     Easy transitions from any state

public class StateMachine
{
    private static readonly List<Transition> EmptyTransitions = new(0);
    private readonly List<Transition> _anyTransitions = new();

    private readonly Dictionary<Type, List<Transition>> _transitions = new();
    private List<Transition> _currentTransitions = new();
    public IState CurrentState { get; set; }

    public void Tick()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        CurrentState?.Tick();
    }

    public void SetState(IState state)
    {
        if (state == CurrentState)
            return;

        CurrentState?.OnExit();
        CurrentState = state;

        _transitions.TryGetValue(CurrentState.GetType(), out _currentTransitions);
        if (_currentTransitions == null)
            _currentTransitions = EmptyTransitions;

        CurrentState.OnEnter();
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _anyTransitions.Add(new Transition(state, predicate));
    }

    private Transition GetTransition()
    {
        foreach (var transition in _anyTransitions)
            if (transition.Condition())
                return transition;

        foreach (var transition in _currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }

    public void ClearTransitions()
    {
        _transitions.Clear();
        _anyTransitions.Clear();
    }

    private class Transition
    {
        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }

        public Func<bool> Condition { get; }
        public IState To { get; }
    }
}