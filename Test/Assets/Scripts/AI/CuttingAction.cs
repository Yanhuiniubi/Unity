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
        UINPCQuestionLogic.OnLoggerPause += OnPause;
        target = (SharedTransform)bt.GetVariable("TargetTree");
    }
    private float preTime = 0;
    public override TaskStatus OnUpdate()
    {
        if (!pause)
            anim.SetTrigger("attack");
        else
        {
            anim.SetTrigger("idle");
            return TaskStatus.Running;
        }
        Vector3 direction = target.Value.position - transform.position;
        direction.y = 0; // ���� Y ��
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        preTime += Time.deltaTime;
        if (preTime >= 10f)
        {
            preTime = 0;
            if (target != null && target.Value != null && target.Value.gameObject != null)
            {
                // ȷ��Ŀ������ǳ����е�ʵ��
                if (!target.Value.gameObject.scene.IsValid())
                {
                    Debug.LogError("Cannot destroy an Asset! Target must be a scene GameObject.");
                    return TaskStatus.Success;
                }

                // ����ɾ���߼�
                CuttingMod.Inst.DeleteTree(target.Value.gameObject);

                // ���ٳ����е� GameObject
                UnityEngine.GameObject.Destroy(target.Value.gameObject);
            }
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnd()
    {
        base.OnEnd();
        UINPCQuestionLogic.OnLoggerPause -= OnPause;
        pause = false;
    }
    bool pause;
    private void OnPause(bool paused,string name)
    {
        if (name.Equals(gameObject.name))
        {
            pause = paused;
        }
    }
}
