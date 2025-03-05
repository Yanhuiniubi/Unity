using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI; // ��� NavMesh ��ص������ռ�

public class MoveToTargetTreeAction : Action
{
    public SharedTransform target;
    private Animator anim;
    private NavMeshAgent navMeshAgent; // ��� NavMeshAgent ����
    public override void OnStart()
    {
        base.OnStart();
        this.anim = GetComponent<Animator>();
        this.navMeshAgent = GetComponent<NavMeshAgent>(); // ��ȡ NavMeshAgent ���
        target.Value = CuttingMod.Inst.GetTargetTree(gameObject.transform.position);
        var bt = GetComponent<BehaviorTree>();
        bt.SetVariable("TargetTree", target);
        // ���� NavMeshAgent ���ٶ�
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position; // ��������õ����������
            navMeshAgent.enabled = true; // ���� NavMeshAgent
        }
        else
        {
            Debug.LogError("Failed to place object on NavMesh!");
        }
        anim.SetTrigger("walk"); // �����ƶ�����
        navMeshAgent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure; // ���Ŀ��Ϊ�գ�����ʧ��
        }

        // ���� NavMeshAgent ��Ŀ��λ��
        navMeshAgent.SetDestination(target.Value.position);
        // ����Ƿ񵽴�Ŀ��λ��
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            return TaskStatus.Success; // ����Ŀ�꣬���سɹ�
        }
        return TaskStatus.Running; // �����ƶ���
    }
    public override void OnPause(bool paused)
    {
        base.OnPause(paused);
        if (paused)
            anim.SetTrigger("idle");
        else
            anim.SetTrigger("walk");
    }
}