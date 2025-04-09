using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BON_State
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}
