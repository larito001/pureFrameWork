using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronResEntity : SceneResEntity
{
    public static DataObjPool<IronResEntity, Vector3> pool =
        new DataObjPool<IronResEntity, Vector3>("IronResEntity", 20);

    public override void Collect()
    {
        canCollectNum--;
         
        FlyTextMgr.Instance.AddText("Iron+1", this.Location, FlyTextType.IronRes);
        PlayerResManager.Instance.AddIronNum(1);
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
        SetPrefabBundlePath("Assets/HotUpdate/prefabs/IronRes/Prefabs/IronRes.prefab");  
    }
}
