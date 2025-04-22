using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using YOTO;

public class GunEntity : BaseEntity
{

    private PlayerEntity player;
    private HandRoot handRoot;
    private TwoBoneIKConstraint leftHand;
    public GunEntity(TwoBoneIKConstraint leftHand ,HandRoot handRoot)
    {
        this.leftHand = leftHand;
        this.handRoot = handRoot;
    }
    private GameObject gun;
    protected override void YOTOOnload()
    {
        YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Role/Gun/SM_Wep_AssaultRifle_02.prefab", Vector3.zero,Quaternion.identity, (obj,pos,rot) =>
        {
            gun = UnityEngine.Object.Instantiate(obj);
            gun.transform.SetParent(handRoot.transform);
            var LeftHandTarget = gun.transform.Find("LeftHandTarget");
           
            leftHand.data.target = LeftHandTarget;
            gun.transform.localPosition = Vector3.zero;
            gun.transform.localRotation = Quaternion.identity;
            player.builder.Build();
        });
    }

    public void Init(PlayerEntity entity)
    {
        player = entity;

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
