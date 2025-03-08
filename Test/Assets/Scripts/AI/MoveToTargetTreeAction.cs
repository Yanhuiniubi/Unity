using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI; // 添加 NavMesh 相关的命名空间

public class MoveToTargetTreeAction : Action
{
    public SharedTransform target;
    private Animator anim;
    private NavMeshAgent navMeshAgent; // 添加 NavMeshAgent 变量
    public override void OnStart()
    {
        base.OnStart();
        UINPCQuestionLogic.OnLoggerPause += OnPause;
        this.anim = GetComponent<Animator>();
        this.navMeshAgent = GetComponent<NavMeshAgent>(); // 获取 NavMeshAgent 组件
        target.Value = CuttingMod.Inst.GetTargetTree(gameObject.transform.position);
        var bt = GetComponent<BehaviorTree>();
        bt.SetVariable("TargetTree", target);
        // 设置 NavMeshAgent 的速度
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position; // 将物体放置到导航网格点
            navMeshAgent.enabled = true; // 启用 NavMeshAgent
        }
        else
        {
            Debug.LogError("Failed to place object on NavMesh!");
        }
        anim.SetTrigger("walk"); // 播放移动动画
        navMeshAgent.isStopped = false;
    }
    public override void OnEnd()
    {
        base.OnEnd();
        UINPCQuestionLogic.OnLoggerPause -= OnPause;
        pause = false;
    }
    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure; // 如果目标为空，返回失败
        }
        if (pause)
        {
            anim.SetTrigger("idle");
            navMeshAgent.isStopped = true;
            return TaskStatus.Running;
        }
        else
        {
            anim.SetTrigger("walk"); // 播放移动动画
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target.Value.position);
        }
        // 设置 NavMeshAgent 的目标位置
        // 检查是否到达目标位置
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            return TaskStatus.Success; // 到达目标，返回成功
        }
        return TaskStatus.Running; // 仍在移动中
    }
    bool pause;
    private void OnPause(bool paused, string name)
    {
        if (name.Equals(gameObject.name))
        {
            pause = paused;
        }
    }
}