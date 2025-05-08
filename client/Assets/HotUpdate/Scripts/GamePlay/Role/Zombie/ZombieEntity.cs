using System.Collections;
using System.Collections.Generic;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using YOTO;

public class ZombieEntity : BaseEntity
{
    ZombieAnimatorCtrl zombieBase;

    public float HP = 100;
    private Transform target;
    public void SetTarget(Transform t)
    {
        if (zombieBase != null)
        {
            zombieBase.GetComponent<ZombieNavCtrl>().SetTarget(t.position);
            target = null;
        }
        else
        {
           this.target=t;
        }
      
    }
    public override void ResetAll()
    {
        if (zombieBase)
        {
      
            zombieBase.gameObject.SetActive(false);
            YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.NormalZombie).Set<ZombieAnimatorCtrl>(zombieBase);
            zombieBase = null;
        }
     
    }

    public override void OnStart()
    {
      
        zombieBase=    YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.NormalZombie).Get<ZombieAnimatorCtrl>();
        zombieBase.gameObject.SetActive(true);
    
        zombieBase.GetComponent<ZombieColliderCtrl>().entityId = this._entityID;
        zombieBase.EnemyRun();
        zombieBase.GetComponent<ZombieColliderCtrl>().Run();
        HP = 100;
        if (this.target)
        {
            SetTarget(target);
        }
        // zombieBase.GetComponent<AgentCrowdPathingAuthoring>().Group =
        //     GameObject.Find("Crowd Group").GetComponent<CrowdGroupAuthoring>();
    }

    public void Hurt(float hurt)
    {
        HP -= hurt;
        if (HP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        zombieBase.EnemyDie();
        zombieBase.GetComponent<ZombieNavCtrl>().Stop();
        zombieBase.GetComponent<ZombieColliderCtrl>().Stop();
        YOTOFramework.timeMgr.DelayCall(() =>
        {
            YOTOFramework.poolMgr.GetObjectPool(ObjectPoolType.NormalZombieEntity).Set(this);
            
        },2.2f);
        EnemyManager.Instance.RemoveZombie(_entityID);
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
        // Assets/HotUpdate/prefabs/Zombies/NormalZombie.prefab
    }

    public override void YOTOOnHide()
    {
     
    }

    public override void SetPosition(Vector3 pos)
    {
        if (zombieBase)
        {
            zombieBase.transform.position = pos;
        }
     
    }

    public override void SetRotation(Quaternion rot)
    {
        
    }
}
