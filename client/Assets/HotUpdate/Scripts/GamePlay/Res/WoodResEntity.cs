using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodResEntity : SceneResEntity
{
    public static DataObjPool<WoodResEntity, Vector3> pool =
        new DataObjPool<WoodResEntity, Vector3>("WoodResEntity", 20);
    public override void Free()
    {
        base.Free();
        RecoverObject();
        pool.RecoverItem(this);
    }
    public override void Collect()
    {
        canCollectNum--;
         
        FlyTextMgr.Instance.AddText("wood+1", this.Location, FlyTextType.WoodRes);
        PlayerResManager.Instance.AddWoodNum(1);
        if (canCollectNum == 0)
        {
            Remove();
        }
    }

    public override void Remove()
    {
        base.Remove();
        pool.RecoverItem(this);
    }

    protected override void SetResData()
    {
        SetInVision(true);
        SetPrefabBundlePath(".prefabAwesomeFreeScans/WoodLogs/Prefabs/LowEndPC/SmallWood.prefab");  
    }
}
