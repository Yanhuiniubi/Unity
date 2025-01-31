using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraSport
{
    public static void StartRotate(Camera camera, Transform target,Action OnFinish, float rotationSpeed = 120f, float
        radius = 5f, float angle = 0f)
    {
        GameMod.Inst.SetGameState(eGameState.OpenUI);
        GameMod.Inst.StartCoroutine(StartRotateIE(camera, target, OnFinish,rotationSpeed, radius, angle));
    }
    static IEnumerator StartRotateIE(Camera camera, Transform target, Action OnFinish,float rotationSpeed = 120f, float
        radius = 5f,float angle = 0f)
    {
        while (true)
        {
            angle += rotationSpeed * Time.deltaTime;

            // �ýǶ��� 0 �� 360 ��֮��ѭ��
            if (angle >= 360f)
            {
                angle = 0f;
                GameMod.Inst.SetGameState(eGameState.Normal);
                OnFinish?.Invoke();
                yield break;
            }

            // �����������λ��
            float x = target.position.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = target.position.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = target.position.y + 2f; // ���Ե������ֵ������������ĸ߶�

            // �����������λ��
            camera.transform.position = new Vector3(x, y, z);

            // ʼ�������������Ŀ��
            camera.transform.LookAt(target);
            yield return null;
        }
    }
}
