using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct UIInfo
{
    public UIInfo(UIType t, Layer l, string k,bool s=false)
    {
        isStatic = s;
        type = t;
        key = k;
        layer = l;
    }
    public bool isStatic;
    public UIType type;
    public string key;//ab���������������
    public Layer layer;

    public override bool Equals(object obj)
    {
        if (!(obj is UIInfo)) return false;
        UIInfo other = (UIInfo)obj;
        return isStatic == other.isStatic && 
               type == other.type && 
               key == other.key && 
               layer == other.layer;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + isStatic.GetHashCode();
        hash = hash * 23 + type.GetHashCode();
        hash = hash * 23 + (key != null ? key.GetHashCode() : 0);
        hash = hash * 23 + layer.GetHashCode();
        return hash;
    }
}
public enum UIEnum
{
    None = 0,
    StaticLoadingUI,

}
public class UIConfig
{
    /// <summary>
    ///������ע��UI
    /// </summary>
    public readonly Dictionary<UIEnum, UIInfo> uiConfigDic = new Dictionary<UIEnum, UIInfo>() {
         { UIEnum.StaticLoadingUI, new UIInfo( UIType.Normal,Layer.StaticUI,"StaticLoadingPage",true) },
          
    };

}