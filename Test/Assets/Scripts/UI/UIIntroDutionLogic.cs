using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIIntroDutionLogic : UI3DLogicBase
{
    private UI3DInfo _info;
    private Coroutine _coroutine;
    private TextMeshProUGUI txt;
    public override void OnHide()
    {
        base.OnHide();
        GameMod.Inst.StopCoroutine(_coroutine);
        _coroutine = null;
    }

    public override void OnInit()
    {
        base.OnInit();
        txt = GetUIComponentInchildren<TextMeshProUGUI>("txtName");
    }

    public override void OnShow(object param)
    {
        base.OnShow(param);
        _info = param as UI3DInfo;
        gameObject.transform.position = new Vector3(_info.BasePos.x, _info.BasePos.y + _info.Height, _info.BasePos.z);
        _coroutine = GameMod.Inst.StartCoroutine(TurnToPlayer());

        txt.text = _info.Cfg.Desc;
    }

    IEnumerator TurnToPlayer()
    {
        while (true)
        {
            // ����UIԪ�غ������֮��ķ�������
            Vector3 direction = GameMod.Inst.PlayerObj.transform.position - gameObject.transform.position;

            // ����������ͶӰ��XZƽ�棨Y���ֵ��Ϊ0��
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);

            // ��������������������ת
            Quaternion rotation = Quaternion.LookRotation(flatDirection);

            // Ӧ����ת
            gameObject.transform.rotation = rotation * Quaternion.Euler(0, 180, 0);
            yield return null;
        }
    }
}
