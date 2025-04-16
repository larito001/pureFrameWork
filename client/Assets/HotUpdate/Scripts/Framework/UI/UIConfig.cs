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
    LoginUI,
    LoadingUI,
    BattleMainUI,
    RoomListUI,
    RoomUI,
    StockUI,
    LoanUI,
    InsuranceUI,
    TurnTableUI,
    ShopUI,
    BagUI,
    DateUI,
    EndCircleUI
}
public class UIConfig
{
    /// <summary>
    ///������ע��UI
    /// </summary>
    public readonly Dictionary<UIEnum, UIInfo> uiConfigDic = new Dictionary<UIEnum, UIInfo>() {
         { UIEnum.StaticLoadingUI, new UIInfo( UIType.Normal,Layer.StaticUI,"StaticLoadingPage",true) },
        { UIEnum.LoginUI, new UIInfo( UIType.Normal,Layer.StaticUI,"LoginPage") } ,
         { UIEnum.LoadingUI, new UIInfo( UIType.Normal,Layer.StaticUI,"LoadingPage") } ,
          { UIEnum.BattleMainUI, new UIInfo( UIType.Normal,Layer.StaticUI,"BattleMainPage") } ,
          { UIEnum.RoomListUI, new UIInfo( UIType.Normal,Layer.StaticUI,"RoomListPage") } ,
          { UIEnum.RoomUI, new UIInfo( UIType.Normal,Layer.StaticUI,"RoomPage") } ,
          
          { UIEnum.StockUI, new UIInfo( UIType.Stack,Layer.StaticUI,"StockPage") } ,
          { UIEnum.LoanUI, new UIInfo( UIType.Stack,Layer.StaticUI,"LoanPage") } ,
          { UIEnum.InsuranceUI, new UIInfo( UIType.Stack,Layer.StaticUI,"InsurancePage") } ,
          { UIEnum.TurnTableUI, new UIInfo( UIType.Stack,Layer.StaticUI,"TurntablePage") } ,
          { UIEnum.ShopUI, new UIInfo( UIType.Stack,Layer.StaticUI,"ShopPage") } ,
          { UIEnum.BagUI, new UIInfo( UIType.Normal,Layer.StaticUI,"BagPage") } ,
          { UIEnum.DateUI, new UIInfo( UIType.Stack,Layer.StaticUI,"DatePage") } ,
          { UIEnum.EndCircleUI, new UIInfo( UIType.Normal,Layer.StaticUI,"EndCirclePage") } 
          
    };

}