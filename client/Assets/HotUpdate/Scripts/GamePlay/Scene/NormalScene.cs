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
        //加载事件系统
        EmergencyManager.Instance.Init();
        YOTOFramework.uIMgr.Show(UIEnum.FightingPanel);
        player = new PlayerEntity();
        var org = GameObject.Find("PlayerOrgPos");
        EnemyManager.Instance.Init();
        player.Init(org.transform.position);
        for (int i = 0; i < 5; i++)
        {
            TowerManager.Instance.GenerateTowerBase(org.transform.position+new Vector3(0,-4,0)+new Vector3(5,0,0)*i); 
            var wood=  SceneResEntity.pool.GetItem(Vector3.zero);
            wood.Location = org.transform.position+new Vector3(0,-4,5) + new Vector3(5, 0, 0) * i;
            wood.InstanceGObj();
      
        }

  
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