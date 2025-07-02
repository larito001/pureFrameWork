using System.Collections;
using System.Collections.Generic;
using Proto;
using UnityEngine;
using YOTO;
using EventType = YOTO.EventType;

public class NormalScene : VirtualSceneBase
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
        var org = GameObject.Find("PlayerOrgPos");
        WeatherManager.Instance.Init();
        //加载紧急事件系统
        // EmergencyManager.Instance.Init();
        YOTOFramework.uIMgr.Show(UIEnum.FightingPanel);
        EnemyManager.Instance.Init();
        player = new PlayerEntity();
        player.Init(org.transform.position);
        TowerManager.Instance.Init();
        SceneResManager.Instance.Init();
        YOTOFramework.uIMgr.Show(UIEnum.AimUI);
    }

    public override void UnLoad()
    {
        WeatherManager.Instance.Unload();
        YOTOFramework.uIMgr.ClearUI();
        EnemyManager.Instance.Unload();
        TowerManager.Instance.Unload();
        SceneResManager.Instance.Unload();
        player.Free();
        player = null;
        Debug.Log("yoto:unload");
    }

    public override void Update(float dt)
    {
        FlyTextMgr.Instance.Update(dt);
    }
}