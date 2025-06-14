using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YOTO;


public class FlyTextCtrl : ObjectBase,PoolItem<Transform>
{
    public static  DataObjPool<FlyTextCtrl,Transform> pool=new DataObjPool<FlyTextCtrl, Transform>("FlyTextCtrl", 40);
    float speed;
    float time = 0;
    public TextMeshProUGUI tmp;
    private bool isInit=false;
    private Transform root;
    private FlyTextData data;
    private RectTransform rct;
    private bool isFly = false;
    private Vector3 currentPos;
    public void Fly(FlyTextData data)
    {
        currentPos=data.pos; 
        if (isInit)
        {
            // rct.anchoredPosition  = data.pos;
            tmp=objTrans.GetComponent<TextMeshProUGUI>();
            tmp.text= data.text;   
        }
        else
        {
            this.data = data;
            isFly = true;
        }
    }
    protected override void YOTOOnload()
    {

    }

    public override void YOTOStart()
    {
      
    }

    public override void YOTOUpdate(float deltaTime)
    {
        if (isInit)
        {
            // 使用 localPosition 进行移动
            currentPos.y += speed; // 改成你想要的值
            rct.anchoredPosition = currentPos;
            time+=deltaTime;
            if (time > 2)
            {
                // 重置位置时也使用局部坐标
                rct.anchoredPosition = Vector3.zero;
                // 必须同时重置其他变换
                rct.localRotation = Quaternion.identity;
                RecoverObject();
                pool.RecoverItem(this);
                time = 0;
            }
        }
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

    protected override void AfterInstanceGObj()
    {
        speed = 10;

        isInit = true;
        rct=objTrans.GetComponent<RectTransform>();
        rct.SetParent(root,false);
        if (isFly)
        {
            Fly(this.data);
            isFly = false;
        }
    }

    public void AfterIntoObjectPool()
    {
        isInit = false;

    }

    public void SetData(Transform serverData)
    {
        SetInVision(true);
        SetPrefabBundlePath("Assets/HotUpdate/prefabs/FlyTextPrefab.prefab");
        root = serverData;
    }
    
}
