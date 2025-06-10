using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class TowerBaseEntity : BaseEntity
{
    private Vector3 pos;
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
        Debug.Log("Update");
        if (timer > 1)
        {
            var e =EnemyManager.Instance.GetEnemey();
            if (e != null)
            {
             
               var dir= e.zombieBase.transform.position-this.pos;
               var bullet= YOTOFramework.poolMgr.GetObjectPool(ObjectPoolType.NormalTowerBulletEntity).Get<NormalTowerBulletEntity>();
               bullet.FireFromTo(this.pos +new Vector3(0,1,0), dir);
               Debug.Log("位置："+pos);
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
    
        this.pos = pos;
    }

    public override void SetRotation(Quaternion rot)
    {
        
    }
}
