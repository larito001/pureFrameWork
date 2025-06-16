using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public abstract class BulletEntity :ObjectBase,PoolItem<Transform>
{
    protected BulletBase bulletBase;
    private long startTime;
    public abstract void FireFromTo(Vector3 pos, Vector3 dir);

    protected override void AfterInstanceGObj()
    {
        bulletBase = objTrans.GetComponent<BulletBase>();
        bulletBase.Init(this);

    }

    public void SetStartTime()
    {
        // 使用 DateTime.Now 获取当前时间，然后转换为毫秒
        startTime = System.DateTime.Now.Ticks / 10000; // 每 10000 Ticks 等于 1 毫秒
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

    public virtual void Remove()
    {
        RecoverObject();
    }
    public abstract void TriggerEnter(Collider other);
    public abstract void SetBulletData(Transform parent);
    public abstract void BulletAfterIntoObjectPool();
    public void AfterIntoObjectPool()
    {
        BulletAfterIntoObjectPool();
    }

    public void SetData(Transform parent)
    {
        SetBulletData(parent);
    }
}
