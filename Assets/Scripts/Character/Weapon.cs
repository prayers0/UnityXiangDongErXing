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
    //目前攻击到的目标
    private HashSet<IDamageable> damageables=new HashSet<IDamageable>();
    private void OnTriggerStay2D(Collider2D collision)
    {
        //如果以及检测过则忽视
        IDamageable damageable=collision.GetComponent<IDamageable>();
        //对象不是可受伤对象或是已经攻击过他
        if (damageable == null || damageables.Contains(damageable)) return;
        damageables.Add(damageable);
        onHit?.Invoke(damageable, skillIndex);
    }
}
