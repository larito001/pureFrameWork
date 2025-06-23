using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public abstract class SceneResEntity : ObjectBase,PoolItem<Vector3>
{
    protected int canCollectNum = 3;


    public abstract void Collect();

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
    protected override void AfterInstanceGObj()
    {
        var res=  ObjTrans.GetComponent<SceneResBase>();
        res.Init(this);
    }

    public void AfterIntoObjectPool()
    {
    }
    
    public void SetData(Vector3 serverData)
    {
        SetResData();
    }

    protected abstract void SetResData();


}
