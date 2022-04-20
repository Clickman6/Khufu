using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FsmStack {
    private readonly Stack<Action> _stack;

    public FsmStack() {
        _stack = new Stack<Action>();
    }

    public void Update() {
        GetCurrentState().Invoke();
    }

    private Action GetCurrentState() {
        return _stack.FirstOrDefault();
    }

    public Action PopState() {
        return _stack.Pop();
    }

    public void PushState(Action state) {
        if (GetCurrentState() != state) {
            _stack.Push(state);
        }
    }

    public void ClearState() {
        _stack.Clear();
    }

    public string GetStateName() {
        return $"{GetCurrentState().Method.Name} {_stack.Count.ToString()}";
    }

}
