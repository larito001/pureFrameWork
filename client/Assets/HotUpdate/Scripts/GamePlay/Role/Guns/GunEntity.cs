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
    
    public float fireRate = 0.2f; // 每发子弹的时间间隔
    private float fireTimer = 0f;

    private bool canFire = true; // 是否允许发射
    private bool isFiring = false; // 当前是否处于开火状态
    public GunEntity(TwoBoneIKConstraint leftHand ,HandRoot handRoot)
    {
        this.leftHand = leftHand;
        this.handRoot = handRoot;
    }
    private GameObject gun;
    protected override void YOTOOnload()
    {
        YOTOFramework.resMgr.LoadGameObject("Assets/HotUpdate/prefabs/Bullet/Bullet.prefab", Vector3.zero,Quaternion.identity, (obj,pos,rot) =>
        {
            YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletObject).SetPrefab(obj.GetComponent<BulletBase>());
        });
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

    public void TryShot()
    {
        if (!canFire) return;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;

            SpawnBullet();
        }
    }
    // 子弹生成接口（你来实现）
    protected virtual void SpawnBullet()
    {
        // 获取子弹对象
        var bullet = YOTOFramework.poolMgr.GetGameObjectPool(GameObjectPoolType.BulletObject).Get<BulletBase>();

        // 设置发射方向
        var shootDirection = player.character.transform.forward;

        // 设置子弹位置：角色前方 + Y轴偏移（比如 +1.0f 高一点）
        Vector3 spawnPos = player.character.transform.position + shootDirection * 1.0f + Vector3.up * 1.0f;
        bullet.transform.position = spawnPos;

        // 设置子弹朝向
        bullet.transform.rotation = Quaternion.LookRotation(shootDirection);

        // 添加刚体
        var rig = bullet.gameObject.AddComponent<Rigidbody>();
        rig.velocity = shootDirection * 50f;
    }
    public override void YOTOStart()
    {
  
    }

    public override void YOTOUpdate(float deltaTime)
    {
        // 计时器增加
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
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
        throw new System.NotImplementedException();
    }
}
