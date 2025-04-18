using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YOTO;

public enum Layer
{
    StaticUI,
    MoveUI
}

public enum UIType
{
    Stack = 0,  // 栈式UI，优先级更高
    Normal = 1, // 普通UI，优先级较低
}

public class UIMgr
{
    // 使用UIEnum作为键，因为它是枚举类型，不会有值相等性的问题
    private Dictionary<UIType, UITypeBase> uiTypeMap = new Dictionary<UIType, UITypeBase>();  // 修改为使用UIType作为键
    UIConfig uIConfig = null;
    public GameObject UIRoot;
    public GameObject topTipsLayer;
    
    // 用于存储不同层级的Canvas引用
    private Dictionary<Layer, Dictionary<UIType, Canvas>> uiLayers = new Dictionary<Layer, Dictionary<UIType, Canvas>>();

    private void SetUILayer(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer("UI");
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SetUILayer(obj.transform.GetChild(i).gameObject);
        }
    }

    public void Init()
    {
        uIConfig = new UIConfig();

        // 创建主UIRoot
        UIRoot = new GameObject("UIRoot");
        UIRoot.layer = LayerMask.NameToLayer("UI");
        
        GameObject.DontDestroyOnLoad(UIRoot);
        //生成提示词层
 
        // 创建StaticUI和MoveUI根节点
        foreach (Layer layer in System.Enum.GetValues(typeof(Layer)))
        {
            GameObject layerObj = new GameObject(layer.ToString());
            layerObj.layer = LayerMask.NameToLayer("UI");
            layerObj.transform.SetParent(UIRoot.transform, false);
            
            Dictionary<UIType, Canvas> typeCanvases = new Dictionary<UIType, Canvas>();
            uiLayers[layer] = typeCanvases;
      
            // 为每个层级创建Stack和Normal类型的Canvas
            foreach (UIType type in System.Enum.GetValues(typeof(UIType)))
            {
                GameObject typeObj = new GameObject(type.ToString());
                typeObj.layer = LayerMask.NameToLayer("UI");
                typeObj.transform.SetParent(layerObj.transform, false);
                
                // 添加Canvas组件
                Canvas canvas = typeObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = YOTOFramework.cameraMgr.getUICamera();
                canvas.overrideSorting = true;  // 重要：启用排序覆盖
                canvas.sortingOrder = ((int)layer * 100) + (10 - (int)type * 5);
                CanvasScaler scaler = typeObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(2532, 1170);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0;
                // 添加其他必要组件
                typeObj.AddComponent<GraphicRaycaster>();
                
                typeCanvases[type] = canvas;

                // 为每种UIType创建一个管理器实例
                if (!uiTypeMap.ContainsKey(type))
                {
                    UITypeBase typeBase = null;
                    if (type == UIType.Normal)
                    {
                        typeBase = new NormalPage();
                    }
                    else if (type == UIType.Stack)
                    {
                        typeBase = new StackPage();
                    }
                    if (typeBase != null)
                    {
                        typeBase.Init(typeObj);
                        uiTypeMap.Add(type, typeBase);
                    }
                }
            }
        }
        topTipsLayer = new GameObject("topTipsLayer");
        topTipsLayer.layer = LayerMask.NameToLayer("UI");
  
        topTipsLayer.transform.SetParent(UIRoot.transform, false);
        Canvas tipCanvas = topTipsLayer.AddComponent<Canvas>();     
        tipCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        tipCanvas.worldCamera = YOTOFramework.cameraMgr.getUICamera();
        tipCanvas.overrideSorting = true;  // 重要：启用排序覆盖
        tipCanvas.sortingOrder = (999 *10);
        CanvasScaler scalerTips = topTipsLayer.AddComponent<CanvasScaler>();
        scalerTips.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scalerTips.referenceResolution = new Vector2(2532, 1170);
        scalerTips.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scalerTips.matchWidthOrHeight = 1;
        topTipsLayer.AddComponent<GraphicRaycaster>();
    }

    public void Show(UIEnum uiEnum)
    {
        Debug.Log($"[UIMgr] Show: uiEnum={uiEnum}");
        UIInfo point = uIConfig.uiConfigDic[uiEnum];
        UIPageHandler handler = null;

        // 获取对应类型的UI管理器
        UITypeBase type = uiTypeMap[point.type];
        if (type == null)
        {
            Debug.LogError($"[UIMgr] Failed to get UI type handler for {uiEnum}");
            return;
        }

        if (type.handlers.ContainsKey(uiEnum))
        {
            handler = type.handlers[uiEnum];
            handler.Init(point.key, point.isStatic,uiEnum);
        }
        else
        {
            handler = new UIPageHandler();
            handler.Init(point.key, point.isStatic,uiEnum);
            type.handlers.Add(uiEnum, handler);
        }

        type.Show(uiEnum);
    }

    public void OnUILoaded(GameObject uiObject)
    {
        if (uiObject != null)
        {
            SetUILayer(uiObject);
        }
    }

    public void Hide(UIEnum uiEnum)
    {
        Debug.Log($"[UIMgr] Hide: uiEnum={uiEnum}");
        if (uIConfig.uiConfigDic.TryGetValue(uiEnum, out UIInfo point))
        {
            if (uiTypeMap.TryGetValue(point.type, out UITypeBase type))
            {
                type?.Hide(uiEnum);
            }
        }
    }

    public void ClearUI()
    {
        Debug.Log("[UIMgr] Clearing all UIs");
        
        // 遍历所有UI类型，调用它们的Clear方法
        foreach (var typeBase in uiTypeMap.Values)
        {
            if (typeBase != null)
            {
                typeBase.Clear();
            }
        }
    }
}
