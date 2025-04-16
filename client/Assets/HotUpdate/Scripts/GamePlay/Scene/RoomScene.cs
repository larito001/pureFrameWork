using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class RoomScene : VirtualSceneBase
{


    public override void OnAdd()
    {
  
    }

    public override void Onload()
    {

    }

    public override void OnInit()
    {
        Debug.Log("YTLOG;初始化了场景");
        YOTOFramework.Instance.uIMgr.Show(UIEnum.LoginUI);
    }

    public override void UnLoad()
    {

    }

    public override void Update(float dt)
    {

    }
}
