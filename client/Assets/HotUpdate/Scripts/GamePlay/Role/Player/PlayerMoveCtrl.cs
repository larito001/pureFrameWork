
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using YOTO;

public class PlayerMoveCtrl : CtrlBase
{
    Rigidbody rigidbody=null;
    Vector3 velocity;
    IEnumerator sprintIE;
    IEnumerator fvxIE;
    List<VFXBase> vfxs= new List<VFXBase>();
    private bool canMove = true;
    
    
    public override void Init(PlayerEntity character)
    {
        base.Init(character);
        characterBase.character.TryGetComponent<Rigidbody>(out rigidbody);
        if (!rigidbody)
        {
            rigidbody = characterBase.character.AddComponent<Rigidbody>();
        }
        YOTOFramework.Instance.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/VFX/smoke_1.prefab",Vector3.zero ,Quaternion.identity,(obj,pos,rot) => {
            YOTOFramework.Instance.poolMgr.GetGameObjectPool(GameObjectPoolType.Smoke_FVX).SetPrefab(obj.GetComponent<VFXBase>());
        });
   
    }
   public void Sprint()
    {
        if (sprintIE==null)
        {
            sprintIE = SprintIE();
            fvxIE = FVXIE();
            StartCoroutine(sprintIE);
            StartCoroutine(fvxIE);
        }
    }
    IEnumerator FVXIE()
    {
        WaitForSeconds wait =    new WaitForSeconds(0.1f);
        while (true) {
            VFXBase vfx = YOTOFramework.Instance.poolMgr.GetGameObjectPool(GameObjectPoolType.Smoke_FVX).Get<VFXBase>();
            vfx.transform.position = transform.position;
            vfx.PlayVFX();
            vfxs.Add(vfx);
            yield return wait;
        }
       
    }
    IEnumerator SprintIE()
    {
        float timer = 0;
        Vector3 dir = velocity;



        while (timer<0.5)
        {
            //Debug.Log("�����");
            timer += Time.deltaTime;
            
            yield return null;
            if (rigidbody)
            {
                rigidbody.AddForce(dir * 40);
            }

        }
        timer = 0;
        StopCoroutine(fvxIE);
        while (timer<0.5f)
        {
            timer += Time.deltaTime;
            yield return null;
     
        }


        for (int i = 0; i < vfxs.Count; i++)
        {
            YOTOFramework.Instance.poolMgr.GetGameObjectPool(GameObjectPoolType.Smoke_FVX).Set<VFXBase>(vfxs[i]);
        }
        vfxs.Clear();
        fvxIE = null;
        sprintIE = null;

    }

   
    public override void YOTOUpdate(float deltaTime)
    {
        if (!canMove)
        {
            return;
        }
        
        if (rigidbody)
        {
            if (characterBase.isSpinting)
            {
                characterBase.isSpinting = false;
                Sprint();
            }
            velocity = characterBase.animationVelocity;
            velocity.y = rigidbody.velocity.y;
            rigidbody.velocity = velocity;
            rigidbody.rotation = characterBase.animationRotate;
        }

    }

    public override void YOTONetUpdate()
    {
        
    }
}
