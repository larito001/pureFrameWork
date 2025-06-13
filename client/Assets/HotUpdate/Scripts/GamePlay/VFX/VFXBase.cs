using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class VFXBase : PoolBaseGameObject
{
    public List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    public override void ResetAll()
    {
     this.gameObject.SetActive(false);
    }

    public override void OnStart()
    {
        this.gameObject.SetActive(true);
    }

    public void PlayVFX()
    {
        if (particleSystems.Count>0)
        {
            YOTOFramework.timeMgr.DelayCall(Remove,particleSystems[0].main.duration);
        }
        for (var i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].Play();
        }
    }

    private void Remove()
    { 
        // if(!this.__isRecycle)
        // YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletImpact).Set(this);
    }
}
