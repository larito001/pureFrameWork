using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public enum FlyTextType
{
    Normal,
    Quick,
    Slow,
}
public struct FlyTextData
{
   public string text;
   public Vector3 pos;
   public FlyTextType flyTextType;
}
public class FlyTextMgr : Singleton<FlyTextMgr>
{
    private Queue<FlyTextData> flyTextQueue = new Queue<FlyTextData>();
    private int generateNumPerFrame=1;
    private bool isInit=false;
    public void Init(int generateNumPerFrame=1)
    {
        if (isInit) return;
        this.generateNumPerFrame = generateNumPerFrame;
        YOTOFramework.resMgr.LoadUI("Assets/HotUpdate/prefabs/FlyTextPrefab.prefab", (prefab) =>
        {
            //prefab
            Debug.Log("������prefab;" + prefab);
            YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.FlyingText).SetPrefab(prefab.GetComponent<FlyTextCtrl>());
          

        });
        isInit=true;
     
    }
    public void AddText(string text,Vector3 pos, FlyTextType type=FlyTextType.Normal)
    {
        //todo:���Ըĳɶ����
        FlyTextData data = new FlyTextData();
        data.text= text;
        data.pos = pos;
        data.flyTextType = type;
        flyTextQueue.Enqueue(data);
    }
    private int index;
    public void Update(float deltaTime)
    {
          
            for(index = 0; flyTextQueue.Count > 0&& index < generateNumPerFrame; index++)
            {
                FlyTextCtrl textBatcher = YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.FlyingText).Get<FlyTextCtrl>(YOTOFramework.uIMgr.GetLayer(UILayerEnum.Tips).layerRoot.transform);
                textBatcher.Fly(flyTextQueue.Dequeue());
            }
               
    }

}
