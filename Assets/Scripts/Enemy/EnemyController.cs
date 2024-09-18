using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterControllerBase<EnemyView>
{
    [SerializeField] private float knockbackRation;
    private Coroutine doKnockbackCoroutine;
    private void Start()
    {
        Init();
    }

    public override void Hurt(float damage, ICharacterController source, SkillData.HitData hitData)
    {
        base.Hurt(damage, source, hitData);
        if (doKnockbackCoroutine != null) StopCoroutine(doKnockbackCoroutine);
        doKnockbackCoroutine = StartCoroutine(DoKnockback(source.transform.position, hitData));
    }

    private IEnumerator DoKnockback(Vector3 source, SkillData.HitData hitData)
    {
        float timer = 0;
        bool isRight=source.x>transform.position.x;
        while (timer<hitData.knockbackTime*knockbackRation)
        {
            timer += Time.deltaTime;
            Move(isRight ? -1 : 1 * hitData.knockbackSpeed, false, false);
            view.SetDir(isRight);
            yield return null;
        }
        StopMove();
        doKnockbackCoroutine = null;
    }
}
