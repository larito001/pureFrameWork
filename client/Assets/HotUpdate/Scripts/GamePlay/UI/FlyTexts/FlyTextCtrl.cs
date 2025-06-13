using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YOTO;


public class FlyTextCtrl : ObjectBase,PoolItem<Transform>
{
    public static  DataObjPool<FlyTextCtrl,Transform> pool=new DataObjPool<FlyTextCtrl, Transform>("FlyTextCtrl", 40);
    Vector3 speed;
    float time = 0;
    public TextMeshProUGUI tmp;
    private bool isInit=false;
    private Transform root;
    public void Fly(FlyTextData data)
    {
        if (isInit)
        {
            objTrans.localPosition = data.pos;
            tmp=objTrans.GetComponent<TextMeshProUGUI>();
            tmp.text= data.text;   
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        speed=new Vector3(0,10,0);


    }

    // Update is called once per frame
    void Update()
    {
        if (isInit)
        {
            objTrans.position += speed*Time.deltaTime;
            time += Time.deltaTime;
            if (time > 1)
            {
                objTrans.position = Vector3.zero;
                RecoverObject();
                time = 0;
            }
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
        isInit = true;
        objTrans.parent = root;
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
