using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testData : YOTOScrollViewDataBase
{
    
}
public class SkillPanel : UIPageBase
{
    public YOTOScrollView scrollView;
    public GameObject item;
    
    public override void OnLoad()
    {
        scrollView.Initialize(item,20);
    }

    public override void OnShow()
    {
        var testList = new List<testData>();
        for (var i = 0; i < 6; i++)
        {
            testList.Add(new testData());
        }
        scrollView.SetData(testList);
    }

    public override void OnHide()
    {
    
    }
}
