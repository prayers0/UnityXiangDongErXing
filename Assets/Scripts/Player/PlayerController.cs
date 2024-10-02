using UnityEngine;

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
            if (Input.GetMouseButton(0) && CanReleaseSkill(currentSkillIndex))
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
        GameManager.Instance.gameData.playerPos = new SVector3(transform.position);

    }
}
