using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterControllerBase<PlayerView>
{
    public static PlayerController Instance { get; private set; }
    private int currentSkillIndex=0;

    private void Awake()
    {
        Instance = this;
    }
    public override void Init()
    {
        base.Init();
    }

    private void Update()
    {
        if (CanJump()&&Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        float h = Input.GetAxisRaw("Horizontal");
        if(SkillState())//技能状态下 
        {
            //如果当前在空中并且这个技能支持控制则使用空中的速度，否则使用常规技能的移动速度
            if (currentSkill.canMove)
            {
                float moveSpeedMultipy = JumpState() && currentSkill.releaseOnJump ? currentSkill
                .moveSpeedMultiplyOnJump : currentSkill.moveSpeedMultiplyOnJump;

                Move(h * moveSpeedMultipy, currentSkill.fanFilp, false);
            }
            else StopMove();
        }
        else//常规移动
        {
            if(CanMove()) Move(h);
            else StopMove();
            //技能释放
            //是否在UI游戏物体上方
            if (Input.GetMouseButton(0)&&!EventSystem.current.IsPointerOverGameObject()
                && CanReleaseSkill(currentSkillIndex))
            {
                ReleaseSkill(currentSkillIndex);
                currentSkillIndex += 1;
                if (currentSkillIndex >= skillDatas.Length)
                {
                    currentSkillIndex = 0;
                }
            }
        }
        UpdatePlayerPositionData();
    }

    protected override void Die()
    {
        
    }

    protected void UpdatePlayerPositionData()
    {
        GameSceneManager.Instance.UpdatePlayerPositionData(transform.position);
    }

    public void OnDoorStay(int doorCoord, bool isEntrance)
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (isEntrance)
            {
                GameSceneManager.Instance.EnterDungeonMap(doorCoord);
            }
            else
            {
                GameSceneManager.Instance.LoadMainMap();
            }
        }

        //if (isEntrance && Input.GetKey(KeyCode.S))
        //{
        //    GameSceneManager.Instance.EnterDungeonMap(doorCoord);
        //}
        //else if (!isEntrance && Input.GetKey(KeyCode.W))
        //{
        //    GameSceneManager.Instance.LoadMainMap();
        //}
    }

    public void OnUssItem(ItemConfigBase itemConfig, ItemDataBase itemData)
    {
    }
}
