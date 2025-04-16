using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class ProgressLineMgr : Singleton<ProgressLineMgr>
{
    Dictionary<int,ProgressLineCtrl> progressLineDict = new Dictionary<int, ProgressLineCtrl>();
    private int id = 1000;
    public void Init()
    {
        YOTOFramework.Instance.resMgr.LoadUI("ProgressLinePrefab", (prefab) =>
        {
            //prefab
            YOTOFramework.Instance.poolMgr.GetGameObjectPool(GameObjectPoolType.ProgressLine).SetPrefab(prefab.GetComponent<ProgressLineCtrl>());

        });
        
    }

   //获取
    public int GetProgress()
    {
        ProgressLineCtrl proline=     YOTOFramework.Instance.poolMgr.GetGameObjectPool(GameObjectPoolType.ProgressLine).Get<ProgressLineCtrl>(YOTOFramework.Instance.uIMgr.topTipsLayer.transform);
        progressLineDict.Add(id,proline);
        return id++;//返回id
    }
    //刷新
    public void SetProgress(int id, Vector3 worldPos,float progress)
    {
        var proline = progressLineDict[id];
        Canvas canvas = YOTOFramework.Instance.uIMgr.topTipsLayer.GetComponent<Canvas>();
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        
        // 获取主相机和UI相机
        Camera mainCamera = YOTOFramework.Instance.cameraMgr.getMainCamera();
        Camera uiCamera = YOTOFramework.Instance.cameraMgr.getUICamera();
        
        // 转换世界坐标到屏幕坐标
        worldPos+=Vector3.up;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        // 将屏幕坐标转换为UI坐标
        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out uiPos);
        
        // Debug.Log($"World Pos: {worldPos}, Screen Pos: {screenPos}, UI Pos: {uiPos}");
        proline.GetComponent<RectTransform>().anchoredPosition = uiPos;
        proline.Progress(progress);
    }
    //移除
    public void RemovePregress(int id)
    {
        if (progressLineDict.ContainsKey(id))
        {
            var proline = progressLineDict[id];
            progressLineDict.Remove(id); 
            YOTOFramework.Instance.poolMgr.GetGameObjectPool(GameObjectPoolType.ProgressLine)
                .Set<ProgressLineCtrl>(proline);  
        }

   
    }
}
