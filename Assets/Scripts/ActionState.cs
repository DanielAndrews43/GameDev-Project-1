using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public abstract class ActionState
{

    protected PlayerController controller;

    public ActionState(PlayerController controller) {
        this.controller = controller;
    }
    
    public abstract string Type{ get; }

    public abstract void Enter();
    
    public virtual void Update(){}

    public virtual void FixedUpdate(){}
    
    public abstract void Exit();
}
