using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class BulletEntity :BaseEntity
{
    private BulletBase bulletBase;
    private long startTime;
    public void FireFromTo(Vector3 pos,Vector3 dir)
    {
        if(!bulletBase) return ;
        bulletBase.trail.enabled = false;
        // 设置子弹位置：角色前方 + Y轴偏移（比如 +1.0f 高一点）
        bulletBase.transform.position = pos;
        bulletBase.trail.Clear();
        bulletBase.trail.time = 0.1f; 
        // 设置子弹朝向
        bulletBase.transform.rotation = Quaternion.LookRotation(dir);
        bulletBase.trail.enabled = true;
        // 添加刚体
        Rigidbody temp;
        if (!bulletBase.gameObject.TryGetComponent<Rigidbody>(out  temp))
        {
            temp =bulletBase.gameObject.AddComponent<Rigidbody>();
        }
        temp.velocity = dir * 50f;
    }

    public long GetStartTime()
    {
        return startTime;
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

    public override void ResetAll()
    {
        if (!bulletBase) return;
        
        YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletObject).Set<BulletBase>(bulletBase);
        bulletBase = null;
    }

    public override void OnStart()
    {
        bulletBase= YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletObject).Get<BulletBase>();
        // 使用 DateTime.Now 获取当前时间，然后转换为毫秒
        startTime = System.DateTime.Now.Ticks / 10000; // 每 10000 Ticks 等于 1 毫秒
    }
}
