using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YOTO;


public class FlyTextCtrl : PoolBaseGameObject
{
    Vector3 speed;
    float time = 0;
    public TextMeshProUGUI tmp;
    public override void ResetAll()
    {
       
    }
    public void Fly(FlyTextData data)
    {
        transform.localPosition = data.pos;
        tmp.text= data.text;   
     
        //YOTOFramework.poolMgr.GameObjectPool.Set<FlyTextCtrl>(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        speed=new Vector3(0,10,0);


    }

    // Update is called once per frame
    void Update()
    {
        if (!isRecycle)
        {
            transform.position += speed*Time.deltaTime;
            time += Time.deltaTime;
            if (time > 1)
            {
                transform.position = Vector3.zero;
                YOTOFramework.Instance.poolMgr.GetGameObjectPool(GameObjectPoolType.FlyingText).Set<FlyTextCtrl>(this);

                time = 0;
            }
        }
        
    }
}
