using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct UIInfo
{
    public UIInfo(UIEnum e, UILayerEnum l, string k)
    {

        uiEnum = e;
        key = k;
        layer = l;
    }
    public UIEnum uiEnum;
    public string key;//ab���������������
    public UILayerEnum layer;

    public override bool Equals(object obj)
    {
        if (!(obj is UIInfo)) return false;
        UIInfo other = (UIInfo)obj;
        return layer == other.layer && 
               key == other.key && 
               layer == other.layer;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + layer.GetHashCode();
        hash = hash * 23 + (key != null ? key.GetHashCode() : 0);
        hash = hash * 23 + layer.GetHashCode();
        return hash;
    }
}
public enum UIEnum
{
    None = 0,
    StaticLoadingUI,
    StartPanel,
    SkillTreePanel,
    FightingPanel,
    FightingEndPanel
}
public class UIConfig
{
    /// <summary>
    ///������ע��UI
    /// </summary>
    public readonly Dictionary<UIEnum, UIInfo> uiConfigDic = new Dictionary<UIEnum, UIInfo>() {
         { UIEnum.FightingEndPanel, new UIInfo( UIEnum.FightingEndPanel,UILayerEnum.Normal,"Assets/HotUpdate/prefabs/UI/FightingEndPanel.prefab") },
         { UIEnum.StartPanel, new UIInfo( UIEnum.StartPanel,UILayerEnum.Normal,"Assets/HotUpdate/prefabs/UI/StartPanel.prefab") },
         { UIEnum.SkillTreePanel, new UIInfo(UIEnum.SkillTreePanel,UILayerEnum.Normal,"Assets/HotUpdate/prefabs/UI/SkillTreePanel.prefab") },
         { UIEnum.FightingPanel, new UIInfo( UIEnum.FightingPanel,UILayerEnum.Normal,"Assets/HotUpdate/prefabs/UI/FightingPanel.prefab") },
    };

}