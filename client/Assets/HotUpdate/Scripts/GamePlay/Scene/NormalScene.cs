using System.Collections;
using System.Collections.Generic;
using Proto;
using UnityEngine;
using YOTO;
using EventType = YOTO.EventType;

public class NormalScene: VirtualSceneBase
{
    private PlayerEntity player;
    //注册事件
    public override void OnAdd()
    {

    }
    //
    public override void Onload()
    {
        Debug.Log("YTLOG;加载了场景");

    }

  
    //加载常用系统
    public override void OnInit()
    {
    
        //加载事件系统
        EmergencyManager.Instance.Init();
        FlyTextMgr.Instance.AddText("游戏开始!",Vector3.zero,FlyTextType.Normal);
        YOTOFramework.uIMgr.Show(UIEnum.FightingPanel);
        player = new PlayerEntity();
       var org= GameObject.Find("PlayerOrgPos");
        EnemyManager.Instance.Init();
        player.Init(org.transform.position);
     // var obj= TestPoolObject.pool.GetItem(Vector3.zero);
     // obj.InstanceGObj();
     // obj.Location = org.transform.position;
    }

    public override void UnLoad()
    {
        Debug.Log("yoto:unload");
    }

    public override void Update(float dt)
    {
        FlyTextMgr.Instance.Update(dt);
    }
}
