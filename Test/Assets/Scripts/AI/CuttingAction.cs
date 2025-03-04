using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CuttingAction : Action
{
    private Animator anim;
    public override void OnAwake()
    {
        base.OnAwake();
        this.anim = GetComponent<Animator>();
    }
    private float preTime = -1;
    public override TaskStatus OnUpdate()
    {
        if (preTime == -1)
            preTime = Time.time;
        if (Time.time - preTime >= 10f)
        {
            var bt = GetComponent<BehaviorTree>();
            SharedTransform transform = (SharedTransform)bt.GetVariable("TargetTree");
            CuttingMod.Inst.DeleteTree(transform.Value.gameObject);
            UnityEngine.GameObject.Destroy(transform.Value.gameObject);
            return TaskStatus.Success;
        }
        anim.SetInteger("moving", 4);
        return TaskStatus.Running;
    }
}
