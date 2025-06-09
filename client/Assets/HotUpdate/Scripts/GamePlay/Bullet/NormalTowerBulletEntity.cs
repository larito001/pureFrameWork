using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class NormalTowerBulletEntity : BulletEntity
{
    public override void Remove()
    {
        base.Remove();
        if (!this.__isRecycle)
        {
            VFXManager.Instance.PlayVFX("impact",bulletBase.transform.position,bulletBase.transform.rotation);
            YOTOFramework.poolMgr.GetObjectPool(ObjectPoolType.NormalTowerBulletEntity).Set<NormalTowerBulletEntity>(this);
        }
    }
}
