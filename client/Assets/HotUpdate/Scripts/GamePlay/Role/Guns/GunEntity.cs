using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;

public class GunEntity : BaseEntity
{
    private HandRoot handRoot;
    public GunEntity(HandRoot handPos)
    {
        handRoot = handPos;
    }
    private GameObject gun;
    protected override void YOTOOnload()
    {
        YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Role/Gun/SM_Wep_AssaultRifle_02.prefab", Vector3.zero,Quaternion.identity, (obj,pos,rot) =>
        {
            gun = UnityEngine.Object.Instantiate(obj);
            gun.transform.SetParent(handRoot.transform);
            gun.transform.localPosition = Vector3.zero;
            gun.transform.localRotation = Quaternion.identity;
        });
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
        throw new System.NotImplementedException();
    }
}
