
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
        // YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/VFX/smoke_1.prefab",Vector3.zero ,Quaternion.identity,(obj,pos,rot) => {
        // YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.Smoke_FVX).SetPrefab(obj.GetComponent<VFXBase>());
        // });
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
                // Sprint();
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
