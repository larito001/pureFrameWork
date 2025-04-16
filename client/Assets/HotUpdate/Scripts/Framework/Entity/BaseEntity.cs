using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YOTO;

public abstract  class BaseEntity 
{
    private static int  ids=0;
    public int  _entityID;
    private bool _isLoaded=false;
    public bool IsLoaded
    {
       get { return _isLoaded; } 
    }
    public  BaseEntity()
    {
        //Debug.Log("���������");
        YOTOOnload();
        _isLoaded = true;
        YOTOFramework.Instance.entityMgr._AddEntity(this);
    }
    protected abstract void YOTOOnload();
    public abstract void YOTOStart();
    public abstract void YOTOUpdate(float deltaTime);
    public abstract void YOTONetUpdate();
    public abstract void YOTOFixedUpdate(float deltaTime);
    public abstract void YOTOOnHide();
    public abstract void SetPosition(Vector3 pos);

    public abstract void SetRotation(Quaternion rot);
    public virtual void Hide()
    {
        YOTOOnHide();
        _isLoaded = false;
    }
    public virtual void Free()
    {
        Hide();
        YOTOFramework.Instance.entityMgr._RemoveEntity(this);
     

    }


}
