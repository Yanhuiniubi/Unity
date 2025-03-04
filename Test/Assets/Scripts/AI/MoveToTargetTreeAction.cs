using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI; // ��� NavMesh ��ص������ռ�

public class MoveToTargetTreeAction : Action
{
    public float speed = 1;
    public SharedTransform target;
    private Animator anim;
    private NavMeshAgent navMeshAgent; // ��� NavMeshAgent ����

    public override void OnAwake()
    {
        base.OnAwake();
        this.anim = GetComponent<Animator>();
        this.navMeshAgent = GetComponent<NavMeshAgent>(); // ��ȡ NavMeshAgent ���
        target.Value = CuttingMod.Inst.GetTargetTree(gameObject.transform.position);
        var bt = GetComponent<BehaviorTree>();
        bt.SetVariable("TargetTree", target);
        // ���� NavMeshAgent ���ٶ�
        navMeshAgent.speed = speed;
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
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                //anim.SetInteger("moving", 0); // ֹͣ�ƶ�����
                return TaskStatus.Success; // ����Ŀ�꣬���سɹ�
            }
        }

        anim.SetInteger("moving", 1); // �����ƶ�����
        return TaskStatus.Running; // �����ƶ���
    }
}