using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class TowerBaseEntity : BaseEntity
{
    public override void ResetAll()
    {
        
    }

    public override void OnStart()
    {
        
    }

    protected override void YOTOOnload()
    {
        YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Bullet/Bullet.prefab", Vector3.zero,Quaternion.identity, (obj,pos,rot) =>
        {
            YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletObject).SetPrefab(obj.GetComponent<BulletBase>());
        });
   
    }

    public override void YOTOStart()
    {
       
    }

    private float timer = 0;
    public override void YOTOUpdate(float deltaTime)
    {
        if (timer > 1)
        {
            var e =EnemyManager.Instance.GetEnemey();
            if (e != null)
            {
             
               var dir= e.zombieBase.transform.position-new Vector3(-30,-1,-30);
               var bullet= YOTOFramework.poolMgr.GetObjectPool(ObjectPoolType.NormalTowerBulletEntity).Get<NormalTowerBulletEntity>();
               bullet.FireFromTo(new Vector3(-30,1,-30), dir);
            }
            
   
            timer = 0;
        }
        timer+=deltaTime;
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

    public override void SetPosition(Vector3 pos)
    {
        
    }

    public override void SetRotation(Quaternion rot)
    {
        
    }
}
