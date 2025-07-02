using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using YOTO;

public class GunEntity : BaseEntity
{
    private const string WeaponPath = "Assets/HotUpdate/prefabs/Role/Gun/SM_Wep_AssaultRifle_02.prefab";
    private const string LaserPath = "Assets/HotUpdate/prefabs/Laser/Prefab/Laser.prefab";
    private const string FirePath = "Assets/HotUpdate/prefabs/Bullet/Fire.prefab";
    private HandRoot handRoot;
    private ParticleSystem fire;
    private Transform fireTrans;
    public float fireRate = 0.2f; // 每发子弹的时间间隔
    private float fireTimer = 0f;
    private Vector3 pointPos = new Vector3(9999,99999,9999);
    public bool canFire { get; private set; } // 是否允许发射
    private bool isFiring = false; // 当前是否处于开火状态
    private bool _isAimEnd;
    private bool _isWaiting;
    private Vector3 _gunForward;

    public void SetForward(Vector3 forward)
    {
        _gunForward = forward;
    }
    
    public void SetIsAinEnd(bool isAimEnd)
    {
        _isAimEnd = isAimEnd;
    }
    public void SetIsWaiting(bool isWaiting)
    {
        _isWaiting = isWaiting;
    }
    public Transform firePos { get; private set; }

    public GunEntity(HandRoot handRoot)
    {
        this.handRoot = handRoot;
    }

    private GameObject gun;
    private LineRenderer laser;
    private Transform laserPoint;
    
    int enemyTriggerLayer = LayerMask.NameToLayer("EnemyTrigger"); // 获取 EnemyTrigger 层的索引
    protected override void YOTOOnload()
    {
        // VFXManager.Instance.Init();
        YOTOFramework.resMgr.LoadGameObject(WeaponPath, LoadWeaponComplete);
    }

    private void LoadWeaponComplete(GameObject obj)
    {
        gun = UnityEngine.Object.Instantiate(obj);
        gun.transform.SetParent(handRoot.transform);
        canFire = true;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
        firePos = gun.transform.Find("FirePos");
        YOTOFramework.resMgr.LoadGameObject(LaserPath, LoadLaserComplete);
        YOTOFramework.resMgr.LoadGameObject(FirePath, LoadFireComplete);
    }

    private void LoadLaserComplete(GameObject obj)
    {
        laser = GameObject.Instantiate(obj).GetComponent<LineRenderer>();
        laser.positionCount = 2;
        laserPoint = laser.GetComponentInChildren<Transform>();
        laserPoint.parent = null;
    }

    private void LoadFireComplete(GameObject obj)
    {
        fireTrans = UnityEngine.Object.Instantiate(obj, Vector3.zero, Quaternion.identity).transform;
        fire = fireTrans.GetComponentInChildren<ParticleSystem>();
        fireTrans.transform.parent = firePos;
        fireTrans.transform.localPosition = Vector3.zero;
        fireTrans.transform.localRotation = Quaternion.identity;
        fire.Stop();
    }
    public void TryShot(UnityAction shootCallBack)
    {
        if (!canFire) return;

        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            shootCallBack();
            SpawnBullet();
        }
    }

    // 子弹生成接口（你来实现）
    protected virtual void SpawnBullet()
    {
        fire.Stop();
        fireTrans.forward = _gunForward;
        fire.Play();
        // 获取子弹对象
        var bullet = NormalGunBullet.pool.GetItem(firePos);
        bullet.InstanceGObj();
        bullet.FireFromTo(firePos.position, _gunForward);
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

        if (laser)
        {
            // 设置 LineRenderer 点的数量（2个点：起点和终点）
            if (_isAimEnd&&canFire&&!_isWaiting)
            {
                if (!laser.enabled)
                {
                    laser.enabled = true;
                }
                // 设置起点
                Vector3 start = fireTrans.position;
                Vector3 direction = _gunForward;
             
                LayerMask layerMask = ~(1 << enemyTriggerLayer); // 排除 EnemyTrigger 层
                RaycastHit hit;
                if (Physics.Raycast(start, direction, out hit, 100f,layerMask))
                {
                    // 如果射线击中物体，设置laser终点为击中点
                    laser.SetPosition(0, start);
                    laser.SetPosition(1, hit.point);
                    laserPoint.position = hit.point;
                }
                else
                {
                    // 如果没有击中，设置终点为最大距离点
                    laser.SetPosition(0, start);
                    laser.SetPosition(1, start + direction * 100f);
                    laserPoint.position = pointPos;
                }
            }
            else
            {
                if (laser.enabled)
                {
                    laser.enabled = false;
                }
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

    public void Reload()
    {
        canFire = false;
    }

    public void ReloadEnd()
    {
        canFire = true;
    }

    public void UnuseWeapon()
    {
        canFire = false;
        gun.SetActive(false);
    }

    public void UseWeapon()
    {
        canFire = true;
        gun.SetActive(true);
        
    }
}