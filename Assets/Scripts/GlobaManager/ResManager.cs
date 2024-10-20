using System.Collections.Generic;
using UnityEngine;

public class ResManager : GlobaManagerBase<ResManager>
{
    public List<ItemConfigBase> itemConfigList=new List<ItemConfigBase>();

    private Dictionary<string, ItemConfigBase> itemConfigDic=new Dictionary<string, ItemConfigBase>();

    #region �������͵�����
    public int PlayerDefaultCoinCount = 100;
    public float PlayerMaxHp = 100;

    #endregion
    public override void Init()
    {
        base.Init();
        InItItemConfig();
    }

    private void InItItemConfig()
    {
        //����ӳ���ֵ�
        //itemConfigList = new List<ItemConfigBase>(itemConfigList.Count);
        for(int i = 0; i < itemConfigList.Count; i++)
        {
            
            ItemConfigBase itemConfig = itemConfigList[i];
            itemConfigDic.Add(itemConfig.name, itemConfig);
        }
    }

    public ItemConfigBase GetItemConfig(string name)
    {
        return itemConfigDic[name];
    }
}
