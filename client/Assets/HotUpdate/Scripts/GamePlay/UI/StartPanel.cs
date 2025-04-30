using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YOTO;

public class StartPanel : UIPageBase
{
    public Button startBtn;
    public Button treeBtn;
    public override void OnLoad()
    {
        startBtn.onClick.AddListener(() =>
        {
           
            YOTOFramework.timeMgr.DelayCall(() =>
            {
                YOTOFramework.sceneMgr.LoadScene(Scenes.Normal); 
                CloseSelf();
            },2.2f);
        });
        treeBtn.onClick.AddListener(() =>
        {
            YOTOFramework.uIMgr.Show(UIEnum.SkillTreePanel);
        });
    }

    public override void OnShow()
    {

    }

    public override void OnHide()
    {

    }
}
