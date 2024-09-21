using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ĭ����Ѳ�ߣ���ͬһ�������ƶ�
//Ѳ�߹������������㹻����׷�����
//׷�������У��������㹻���򹥻����
public class EnemyController : CharacterControllerBase<EnemyView>
{
    [SerializeField] private float knockbackRation;
    [SerializeField] private float attackRangle;
    [SerializeField] private float pursueRange;
    private Coroutine doKnockbackCoroutine;
    private bool pursueState;
    private bool patrolDirIsRight;
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        pursueState = false;
        patrolDirIsRight = UnityEngine.Random.Range(0, 2) == 0;

    }

    private void LateUpdate()
    {
        if (!MapController.current.hasPlayer) return;
        if (SkillState()) return;
        float currentPosX = transform.position.x;
        float playerPosX = MapController.current.playerControllerPoxX;

        float distance = MathF.Abs(transform.position.x - playerPosX);
        int currentCellCoord = MapController.current.GetCellCoord(currentPosX);
        bool currentIsSecondLayer = MapController.current.CheckCoordIsSedondLayer(currentCellCoord);
        
        if (!pursueState)
        {
            int nextCellCoord = currentCellCoord + (patrolDirIsRight ? 1 : -1);
            bool nextSecondLayer = MapController.current.CheckCoordIsSedondLayer(nextCellCoord);
            bool changeDir = currentIsSecondLayer !=nextSecondLayer|| MapController.current.IsEmptyCell(nextCellCoord);
            if (changeDir) patrolDirIsRight = !patrolDirIsRight;
            Move(patrolDirIsRight ? 1 : -1);
            //���빻��ȥѲ��
            if (distance < pursueRange)
            {
                pursueState=true;
            }
        }
        else
        {
            //���빻��ȥ����
            if (distance < attackRangle)
            {
                StopMove();
                ReleaseSkill(UnityEngine.Random.Range(0, skillDatas.Length));
                return;
            }

            //��ǰ��һ¥����һ�ڵ��Ƕ�¥
            bool isRight = playerPosX > currentPosX;

            int nextCellCoord = currentCellCoord + (isRight ? 1 : -1);
            bool nextSecondLayer = MapController.current.CheckCoordIsSedondLayer(nextCellCoord);
            bool needJump = !currentIsSecondLayer && nextSecondLayer;
            if (needJump && CanJump())
            {
                Jump();
            }
            else if (CanMove())
            {
                Move(isRight ? 1 : -1);
            }
        }
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
