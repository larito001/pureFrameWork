using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class SceneResEntity : ObjectBase,PoolItem<Vector3>
{
    private int canCollectNum = 3;
    public static DataObjPool<SceneResEntity, Vector3> pool =
        new DataObjPool<SceneResEntity, Vector3>("SceneResEntity", 20);

    public void Collect()
    {
        canCollectNum--;
         
        FlyTextMgr.Instance.AddText("wood+1", this.Location, FlyTextType.Normal);
        PlayerResManager.Instance.AddWoodNum(1);
        if (canCollectNum == 0)
        {
            Remove();
        }
    }

    protected override void YOTOOnload()
    {
    }

    public override void YOTOStart()
    {
    }

    public override void YOTOUpdate(float deltaTime)
    {
        
    }

    public override void YOTONetUpdate()
    {
    }

    public override void YOTOFixedUpdate(float deltaTime)
    {
    }

    public override void YOTOOnHide()
    {
    }


    public void Remove()
    {
        pool.RecoverItem(this);
        RecoverObject();
    }
    protected override void AfterInstanceGObj()
    {
        var towerBase=  ObjTrans.GetComponent<SceneResBase>();
        towerBase.Init(this);
    }

    public void AfterIntoObjectPool()
    {
    }
    
    public void SetData(Vector3 serverData)
    {
        SetInVision(true);
        SetPrefabBundlePath(".prefabAwesomeFreeScans/WoodLogs/Prefabs/LowEndPC/SmallWood.prefab");
    }
    

}
