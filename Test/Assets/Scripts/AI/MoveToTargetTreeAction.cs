using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI; // 添加 NavMesh 相关的命名空间

public class MoveToTargetTreeAction : Action
{
    public float speed = 1;
    public SharedTransform target;
    private Animator anim;
    private NavMeshAgent navMeshAgent; // 添加 NavMeshAgent 变量

    public override void OnAwake()
    {
        base.OnAwake();
        this.anim = GetComponent<Animator>();
        this.navMeshAgent = GetComponent<NavMeshAgent>(); // 获取 NavMeshAgent 组件
        target.Value = CuttingMod.Inst.GetTargetTree(gameObject.transform.position);
        var bt = GetComponent<BehaviorTree>();
        bt.SetVariable("TargetTree", target);
        // 设置 NavMeshAgent 的速度
        navMeshAgent.speed = speed;
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure; // 如果目标为空，返回失败
        }

        // 设置 NavMeshAgent 的目标位置
        navMeshAgent.SetDestination(target.Value.position);

        // 检查是否到达目标位置
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                //anim.SetInteger("moving", 0); // 停止移动动画
                return TaskStatus.Success; // 到达目标，返回成功
            }
        }

        anim.SetInteger("moving", 1); // 播放移动动画
        return TaskStatus.Running; // 仍在移动中
    }
}