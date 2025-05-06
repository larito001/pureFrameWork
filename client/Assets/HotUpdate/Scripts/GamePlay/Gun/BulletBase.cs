using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : PoolBaseGameObject
{
    public ParticleSystem particle;
    public TrailRenderer trail;

    private BulletEntity bulletEntity;
    public void Init(BulletEntity entity)
    {
        bulletEntity=entity;
    }
    private void OnTriggerEnter(Collider other)
    { 
        ZombieColliderCtrl ctrl;
       if ( other.TryGetComponent<ZombieColliderCtrl>(out ctrl))
       {
           EnemyManager.Instance.Hurt(ctrl.entityId,44);
           if (bulletEntity != null)
           {
               bulletEntity.Remove();
               bulletEntity=null;
           }
       }
       else if (other.gameObject.layer == 6)
       {
           if (bulletEntity != null)
           {
               bulletEntity.Remove();
               bulletEntity=null;  
           }
  
       }
       
    }

    private void Start()
    {
        
    }

    public override void ResetAll()
    {
        trail.time = 0;
    }

    public override void OnStart()
    {


    }
}
