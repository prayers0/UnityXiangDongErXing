using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Action<IDamageable, int> onHit;
    private int skillIndex;

    public void Init(Action<IDamageable,int> onHit)
    {
        this.onHit = onHit;
        StopCheck();
    }

    public void SetSkill(int skillIndex)
    {
        this.skillIndex = skillIndex;
    }

    public void StartCheck()
    {
        gameObject.SetActive(true);
    }

    public void StopCheck()
    {
        gameObject.SetActive(false);
        damageables.Clear();
    }
    //Ŀǰ��������Ŀ��
    private HashSet<IDamageable> damageables=new HashSet<IDamageable>();
    private void OnTriggerStay2D(Collider2D collision)
    {
        //����Լ����������
        IDamageable damageable=collision.GetComponent<IDamageable>();
        //�����ǿ����˶�������Ѿ���������
        if (damageable == null || damageables.Contains(damageable)) return;
        damageables.Add(damageable);
        onHit?.Invoke(damageable, skillIndex);
    }
}
