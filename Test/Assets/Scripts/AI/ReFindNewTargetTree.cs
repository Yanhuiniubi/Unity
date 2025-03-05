using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class ReFindNewTargetTree : Action
{
    private Animator anim;
    public override void OnStart()
    {
        base.OnAwake();
        this.anim = GetComponent<Animator>();
        anim.SetTrigger("idle");

    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
