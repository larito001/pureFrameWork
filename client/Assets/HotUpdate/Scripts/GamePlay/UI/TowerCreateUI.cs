using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YOTO;

public class TowerCreateUI : UIPageBase
{
    public Button button;
    
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
                this.transform.position = new Vector3(-1000, -1000, 0);
            }
        }
    }

    private void GenerateTower()
    {
        if (TowerBaseEntity.selectTower != null)
        {
            TowerBaseEntity.selectTower .StartFire();
        }
        
    }

    public override void OnShow()
    {
        button.onClick.AddListener(GenerateTower);
    }

    public override void OnHide()
    {
        button.onClick.RemoveListener(GenerateTower);
    }
}