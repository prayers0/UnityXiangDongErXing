using System;
using System.Collections;
using UnityEngine;

//默认在巡逻，在同一层左右移动
//巡逻过程中如果玩家足够近则追击玩家
//追击过程中，如果玩家足够近则攻击玩家
public class EnemyController : CharacterControllerBase<EnemyView>
{
    [SerializeField] protected float maxHp;

    [SerializeField] private float knockbackRation;
    [SerializeField] private float attackRangle;
    [SerializeField] private float pursueRange;
    [SerializeField] private float pursueAbandonRange;
    [SerializeField] private float destroyDelay;//延迟销毁时间，用来表现死亡动画
    private Coroutine doKnockbackCoroutine;
    private bool pursueState;
    private bool patrolDirIsRight;
    private int currentMapChunkCoord;
    //private void Start()
    //{
    //    Init();
    //}

    public override void Init()
    {
        base.Init();
        currentHP = maxHp;
        pursueState = false;
        patrolDirIsRight = UnityEngine.Random.Range(0, 2) == 0;

    }

    public void Init(int mapChunkCoord)
    {
        this.currentMapChunkCoord = mapChunkCoord;
        Init();
    }

    private void UpdateMapChunk()
    {
        int newMapChunkCoord = MapController.current.GetChunkCoord(transform.position.x);
        if (newMapChunkCoord != currentMapChunkCoord)
        {
            MapController.current.EnemyManager.UpdateEnemyChunk(this, currentMapChunkCoord, newMapChunkCoord);
            currentMapChunkCoord = newMapChunkCoord;
        }
    }

    private void LateUpdate()
    {
        if (CurrentHP <= 0) return;
        UpdateMapChunk();
        if (!MapController.current.hasPlayer) return;
        if (SkillState())
        {
            if (currentSkill.canMove)
            {
                float movDir = view.IsRight() ? 1:-1;
                Move(movDir * currentSkill.moveSpeedMultiply, false, false);
            }
            return;
        }
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
            //距离够近去巡逻
            if (distance < pursueRange)
            {
                pursueState=true;
            }
        }
        else
        {
            //距离够近去攻击
            for(int i = 0; i < skillDatas.Length; i++)
            {
                SkillData skillData = skillDatas[i];
                if (distance >= skillData.attackRange.x && distance < skillData.attackRange.y)//处于技能的范内
                {
                    StopMove();
                    view.SetDir(playerPosX > currentPosX);
                    ReleaseSkill(i);
                    return;
                }
               
            }
           
            //距离太远则切回巡逻
            if (distance > pursueAbandonRange)
            {
                pursueState = false;
                return;
            }

            //当前是一楼，下一节点是二楼
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

    protected override void OnFootStep()
    {
        if (footStepClips.Length == 0) return;
        PlayerSpaceAudio(footStepClips[UnityEngine.Random.Range(0, footStepClips.Length)], 0.3f);
    }

    private void PlayerSpaceAudio(AudioClip audioClip,float volum)
    {
        float dis = MathF.Abs(MapController.current.lastCameraPosx - transform.position.x);
        if (dis == 0) dis = 0.001f;
        AudioManager.Instance.PlayerAudio(audioClip, volum * (1 / dis));
    }

    protected override void Die()
    {
        StopAllCoroutines();
        Invoke(nameof(DoDie), destroyDelay);
        view.PlayerAnimation("Die");
        //关闭碰撞体，刚体
        rigidbody.gravityScale = 0;
        Collider2D[] colliders = gameObject.GetComponentsInChildren<Collider2D>();
        foreach(Collider2D item in colliders)
        {
            item.enabled = false;
        }
    }

    private void DoDie()
    {
        MapController.current.EnemyManager.RemoveEnemy(this, currentMapChunkCoord);
    }
}
