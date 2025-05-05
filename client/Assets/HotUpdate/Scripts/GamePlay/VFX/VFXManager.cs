using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class VFXManager : Singleton<VFXManager>
{
    private Dictionary<string, string> VFXDic = new Dictionary<string, string>()
    {
        {"impact","Assets/HotUpdate/prefabs/Bullet/Impact.prefab"}
    };

    public void Init()
    {
        foreach (var name in VFXDic.Values)
        {
            YOTOFramework.resMgr.LoadGameObject(name,Vector3.zero, Quaternion.identity, LoadObj);
        }

    
    }
    public void PlayVFX(string name,Vector3 pos, Quaternion rot)
    {
        if (VFXDic.ContainsKey(name))
        {
          var vfx = YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletImpact).Get<VFXBase>();
          vfx.transform.position = pos;
          vfx.transform.rotation = rot;
          vfx.PlayVFX();
        }
    }


    private void LoadObj(GameObject obj ,Vector3 pos, Quaternion rot)
    {
        YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletImpact).SetPrefab(obj.GetComponent<VFXBase>());
    }
}
