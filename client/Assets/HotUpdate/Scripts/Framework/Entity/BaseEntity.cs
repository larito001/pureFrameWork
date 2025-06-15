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
        _entityID=ids++;
        //Debug.Log("���������");
        YOTOOnload();
        _isLoaded = true;
        YOTOFramework.entityMgr._AddEntity(this);
    }
    protected abstract void YOTOOnload();
    public abstract void YOTOStart();
    public abstract void YOTOUpdate(float deltaTime);
    public abstract void YOTONetUpdate();
    public abstract void YOTOFixedUpdate(float deltaTime);
    public abstract void YOTOOnHide();
    private void Hide()
    {
        YOTOOnHide();
        _isLoaded = false;
    }
    public virtual void Free()
    {
        Hide();
        YOTOFramework.entityMgr._RemoveEntity(this);
    
    }


}
