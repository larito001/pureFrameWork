using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class VFXBase : PoolBaseGameObject
{
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    public override void ResetAll()
    {
     
    }

    public override void OnStart()
    {
        
    }

    public void PlayVFX()
    {
        for (var i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].Play();
            if (i == 0)
            {
                YOTOFramework.timeMgr.DelayCall(Remove,particleSystems[i].duration);
            }
    
        }
    }

    private void Remove()
    { 
        if(!this.__isRecycle)
        YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletImpact).Set(this);
    }
}
