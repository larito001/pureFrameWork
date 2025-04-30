using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : PoolBaseGameObject
{
    public ParticleSystem particle;
    public TrailRenderer trail;

    private void OnTriggerEnter(Collider other)
    {
        ZombieColliderCtrl ctrl;
       if ( other.TryGetComponent<ZombieColliderCtrl>(out ctrl))
       {
           EnemyManager.Instance.Hurt(ctrl.entityId,999);
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
