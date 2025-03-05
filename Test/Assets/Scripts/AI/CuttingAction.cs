using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CuttingAction : Action
{
    private BehaviorTree bt;
    private SharedTransform target;
    private Animator anim;
    public override void OnStart()
    {
        base.OnStart();
        this.anim = GetComponent<Animator>();
        bt = GetComponent<BehaviorTree>();
        target = (SharedTransform)bt.GetVariable("TargetTree");
        anim.SetTrigger("attack");
    }
    private float preTime = -1;
    public override TaskStatus OnUpdate()
    {
        Vector3 direction = target.Value.position - transform.position;
        direction.y = 0; // 忽略 Y 轴
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        if (preTime == -1)
            preTime = Time.time;
        if (Time.time - preTime >= 10f)
        {
            preTime = -1;
            if (target != null && target.Value != null && target.Value.gameObject != null)
            {
                // 确保目标对象是场景中的实例
                if (!target.Value.gameObject.scene.IsValid())
                {
                    Debug.LogError("Cannot destroy an Asset! Target must be a scene GameObject.");
                    return TaskStatus.Success;
                }

                // 调用删除逻辑
                CuttingMod.Inst.DeleteTree(target.Value.gameObject);

                // 销毁场景中的 GameObject
                UnityEngine.GameObject.Destroy(target.Value.gameObject);
            }
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnPause(bool paused)
    {
        base.OnPause(paused);
        if (paused)
            anim.SetTrigger("idle");
        else
            anim.SetTrigger("attack");
    }
}
