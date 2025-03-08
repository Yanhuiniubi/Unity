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
        UINPCQuestionLogic.OnLoggerPause += OnPause;
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
            return TaskStatus.Failure; // ���Ŀ��Ϊ�գ�����ʧ��
        }
        if (pause)
        {
            anim.SetTrigger("idle");
            navMeshAgent.isStopped = true;
            return TaskStatus.Running;
        }
        else
        {
            anim.SetTrigger("walk"); // �����ƶ�����
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(target.Value.position);
        }
        // ���� NavMeshAgent ��Ŀ��λ��
        // ����Ƿ񵽴�Ŀ��λ��
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.isStopped = true;
            return TaskStatus.Success; // ����Ŀ�꣬���سɹ�
        }
        return TaskStatus.Running; // �����ƶ���
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