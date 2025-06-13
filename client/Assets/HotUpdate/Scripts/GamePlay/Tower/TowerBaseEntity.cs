using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class TowerBaseEntity : ObjectBase,PoolItem<Vector3>
{
    public static  DataObjPool<TowerBaseEntity,Vector3> pool=new DataObjPool<TowerBaseEntity, Vector3>("TowerBaseEntity", 20);
    private Vector3 pos;
    protected override void YOTOOnload()
    {
        
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
               var  bullet = NormalGunBullet.pool.GetItem(Vector3.zero);
               bullet.InstanceGObj();
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

    protected override void AfterInstanceGObj()
    {
        
    }

    public void AfterIntoObjectPool()
    {
     
    }

    public void SetData(Vector3 serverData)
    {

    }
}
