using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YOTO;

public class TowerCreateUI : UIPageBase
{
    public Button fire;
    public Button bullet;
    private Camera cam;
    public override void OnLoad()
    {
        cam=YOTOFramework.cameraMgr.getUICamera();
    }

    private void Update()
    {
        if (TowerBaseEntity.selectTower != null )
        {
            // 1. 将世界坐标转换为屏幕坐标
            Vector3 screenPosition = cam.WorldToScreenPoint(TowerBaseEntity.selectTower.Location);
            
            // 2. 检查目标是否在摄像机前方（避免显示背面物体）
            if (screenPosition.z > 0)
            {

                
                // 4. 设置UI位置
                this.transform.position = screenPosition;
            }
            else
            {
                // 如果物体在摄像机后方，将UI移到屏幕外
                this.transform.position = new Vector3(-10000, -10000, 0);
            }
        }
        else
        {
            // 如果物体在摄像机后方，将UI移到屏幕外
            this.transform.position = new Vector3(-10000, -10000, 0);
        }
    }

    private void GenerateTowerFire()
    {
        if (TowerBaseEntity.selectTower != null)
        {

            if (PlayerResManager.Instance.CheckWoodEnough(3))
            {
                PlayerResManager.Instance.UseWoodRes(3);
                TowerBaseEntity.selectTower .GenerateTower(TowerEnum.Fire);
                CloseSelf(); 
            }
            else
            {
                FlyTextMgr.Instance.AddText("Wood not Enough!",TowerBaseEntity.selectTower.Location);
            }
   
        }
        
    }
    private void GenerateTowerNormal()
    {
        if (TowerBaseEntity.selectTower != null)
        {
            if (PlayerResManager.Instance.CheckIronEnough(3))
            {
                PlayerResManager.Instance.UseIronRes(3);
                TowerBaseEntity.selectTower .GenerateTower(TowerEnum.NormalBullet);
                CloseSelf();
            }
            else
            {
                FlyTextMgr.Instance.AddText("Iron not Enough!",TowerBaseEntity.selectTower.Location);
            }

       
        }
        
    }
    public override void OnShow()
    {
        fire.onClick.AddListener(GenerateTowerFire);
        bullet.onClick.AddListener(GenerateTowerNormal);
    }

    public override void OnHide()
    {
        bullet.onClick.RemoveListener(GenerateTowerNormal);
        fire.onClick.RemoveListener(GenerateTowerFire);
    }
}