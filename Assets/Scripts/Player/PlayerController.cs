using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterControllerBase<PlayerView>
{
    public static PlayerController Instance { get; private set; }

    public override float MaxHp => ResManager.Instance.PlayerMaxHp;

    private int currentSkillIndex=0;
    private WeaponConfig weaponConfig;

    private void Awake() 
    {
        Instance = this;
    }

    public void Init(float hp,WeaponConfig weaponConifg)
    {
        base.Init();
        this.currentHP = hp;
        weaponConfig= weaponConifg;
    }

    protected override void OnHPChanged(float newHp)
    {
        GameSceneManager.Instance.SetHp(newHp);
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
        if(SkillState())//����״̬�� 
        {
            //�����ǰ�ڿ��в����������֧�ֿ�����ʹ�ÿ��е��ٶȣ�����ʹ�ó��漼�ܵ��ƶ��ٶ�
            if (currentSkill.canMove)
            {
                float moveSpeedMultipy = JumpState() && currentSkill.releaseOnJump ? currentSkill
                .moveSpeedMultiplyOnJump : currentSkill.moveSpeedMultiplyOnJump;

                Move(h * moveSpeedMultipy, currentSkill.fanFilp, false);
            }
            else StopMove();
        }
        else//�����ƶ�
        {
            if(CanMove()) Move(h);
            else StopMove();
            //�����ͷ�
            //�Ƿ���UI��Ϸ�����Ϸ�
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
        if (Input.GetKeyDown(KeyCode.E))
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
    }

    protected override float GetAttackValue(float skillAttackValueMultiply)
    {
        return (baseAttackValue + weaponConfig.attackValue) * skillAttackValueMultiply;
    }

    public void OnUssItem(ItemConfigBase itemConfig, ItemDataBase itemData)
    {
        if(itemConfig is WeaponConfig)
        {
            weaponConfig = (WeaponConfig)itemConfig;
        }
        else if(itemConfig is ConsunableConfig)
        {
            float addHp = ((ConsunableConfig)itemConfig).hpRegeneration;
            CurrentHP += addHp;
        }
    }

    public void OnMerchantStay(List<ItemConfigBase> items)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //�����л�һ���̵괰�ڣ�����л�����ǿ���״̬��ͬ���򿪱���
            if(UIManager.Instance.ToggleWindow(out UI_ShopWindow ShopWindow))
            {
                ShopWindow.Show(items);
                GameSceneManager.Instance.OpenBagWindow();
            }
        }
    }
}
