using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YOTO;

public class FightingPanel : UIPageBase
{
    public Button settingUI;
    public override void OnLoad()
    {
        settingUI.onClick.AddListener(() =>
        {
            YOTOFramework.uIMgr.Show(UIEnum.FightingEndPanel);
        });
    }

    public override void OnShow()
    {
    }

    public override void OnHide()
    {
      
    }
}
